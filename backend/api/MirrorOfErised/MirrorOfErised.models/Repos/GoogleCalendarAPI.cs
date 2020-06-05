using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MirrorOfErised.models.Repos
{
    public class GoogleCalendarAPI 
    {
        private readonly HttpClient _client;

        public GoogleCalendarAPI(HttpClient client , IAuthTokenRepo authTokenRepo, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _client = client;
            AuthTokenRepo = authTokenRepo;
            UserManager = userManager;
            _configuration = configuration;
            _client.BaseAddress = new Uri("https://www.googleapis.com/");
        }

        public IAuthTokenRepo AuthTokenRepo { get; }
        public UserManager<IdentityUser> UserManager { get; }
        public IConfiguration _configuration { get; }

        public async Task<string> ListFiles(string accessToken, string refreshToken, Func<string, Task> tokenRefreshed)
        {
            var policy = CreateTokenRefreshPolicy(tokenRefreshed);

            var response = await policy.ExecuteAsync(context =>
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"calendar/v3/calendars/primary/events?timeMin={DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
                requestMessage.Headers.Add("Authorization", $"Bearer {context["access_token"]}");  //context["access_token"]

                
/*                requestMessage.par.Add("timeMax", DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH\\:mm\\:ss.ffzzz"));   
                requestMessage.Properties.Add("timeMin", DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.ffzzz"));*/
/*
                requestMessage.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "timeMax", DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH\\:mm\\:ss.ffzzz") },
                        { "timeMin", DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.ffzzz")},
                    });*/

                return _client.SendAsync(requestMessage);
            }, new Dictionary<string, object>
        {
            {"access_token", accessToken},
            {"refresh_token", refreshToken}
        });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private AsyncRetryPolicy<HttpResponseMessage> CreateTokenRefreshPolicy(Func<string, Task> tokenRefreshed)
        {
            var policy = Policy
                .HandleResult<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(1, async (result, retryCount, context) =>
                {
                    if (context.ContainsKey("refresh_token"))
                    {
                        var newAccessToken = await RefreshAccessToken(context["refresh_token"].ToString());
                        if (newAccessToken != null)
                        {
                            await tokenRefreshed(newAccessToken);
                            dynamic Response = JsonConvert.DeserializeObject(newAccessToken);
                            context["access_token"] = Response.access_token;
                        }
                    }
                });

            return policy;
        }

        private async Task<string> RefreshAccessToken(string refreshToken)
        {
            var refreshMessage = new HttpRequestMessage(HttpMethod.Post, "/oauth2/v4/token")
            {
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                {
                new KeyValuePair<string, string>("client_id", Environment.GetEnvironmentVariable("CLIENTID")),
                new KeyValuePair<string, string>("client_secret", Environment.GetEnvironmentVariable("CLIENTSECRET")),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token")
                })
            };

            var response = await _client.SendAsync(refreshMessage);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadAsStringAsync();
/*                TokenResponse Trespo =  TokenResponse.FromHttpResponseAsync(response);
*/                return tokenResponse;
            }

            // return null if we cannot request a new token
            return null;
        }
    }
}
