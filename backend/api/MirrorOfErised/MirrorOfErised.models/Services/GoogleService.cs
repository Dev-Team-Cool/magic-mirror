using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MirrorOfErised.models.Repos;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace MirrorOfErised.models.Services
{
    public class GoogleService
    {
        protected readonly HttpClient Client;
        private readonly IAuthTokenRepo _authTokenRepo;

        public GoogleService(HttpClient client, IAuthTokenRepo authTokenRepo)
        {
            Client = client;
            Client.BaseAddress = new Uri("https://www.googleapis.com/");
            _authTokenRepo = authTokenRepo;
        }

        public async Task<string> AuthenticatedGet(string url, AuthToken token)
        {
            var policy = CreateRetryPolicy(token);

            var response = await policy.ExecuteAsync(context =>
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                    requestMessage.Headers.Add("Authorization",
                        $"Bearer {context["access_token"]}");
                    
                    return Client.SendAsync(requestMessage);
                },
                new Dictionary<string, object>
                {
                    {"access_token", token.Token},
                    {"refresh_token", token.RefreshToken}
                });

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsStringAsync();
        }

        private AsyncRetryPolicy<HttpResponseMessage> CreateRetryPolicy(AuthToken token)
        {
            var policy = Policy
                .HandleResult<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(1, async (result, retryCount, context) =>
                {
                    if (context.ContainsKey("refresh_token"))
                    {
                        Console.WriteLine("Unauthorized");
                        var newAccessToken = await RequestNewAccessToken(token);
                        if (newAccessToken != null)
                        {
                            context["access_token"] = newAccessToken.Token;
                        }
                    }
                });

            return policy;
        }

        private async Task<AuthToken> SaveUpdatedToken(AuthToken token, dynamic newTokenObject)
        {
            token.Token = newTokenObject.access_token;
            token.ExpireDate = DateTime.Now.AddSeconds((int)newTokenObject.expires_in);
            await _authTokenRepo.UpdateTokenAsync(token);

            return token;
        }

        public async Task<AuthToken> RequestNewAccessToken(AuthToken token)
        {
            var refreshMessage = new HttpRequestMessage(HttpMethod.Post, "/oauth2/v4/token")
            {
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("client_id", Environment.GetEnvironmentVariable("CLIENTID")),
                    new KeyValuePair<string, string>("client_secret", Environment.GetEnvironmentVariable("CLIENTSECRET")),
                    new KeyValuePair<string, string>("refresh_token", token.RefreshToken),
                    new KeyValuePair<string, string>("grant_type", "refresh_token")
                })
            };

            var response = await Client.SendAsync(refreshMessage);

            if (response.IsSuccessStatusCode)
            {
                var newAccessToken = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(newAccessToken))
                {
                    dynamic newAccessTokenJson = JsonConvert.DeserializeObject(newAccessToken);
                    return await SaveUpdatedToken(token, newAccessTokenJson); 
                }
            }

            // return null if we cannot request a new token
            return null;
        }
    }
}