using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MirrorOfErised.models.Repos;

namespace MirrorOfErised.models.Services
{
    public class GoogleCalendarService: GoogleService
    {
        public GoogleCalendarService(HttpClient client, IAuthTokenRepo authTokenRepo, IConfiguration configuration) : base(client, authTokenRepo, configuration)
        {
        }
        
        public async Task<string> GetCalendarEvents(AuthToken token)
        {
            return await AuthenticatedGet($"calendar/v3/calendars/primary/events?timeMin={DateTime.Now:yyyy-MM-ddTHH:mm:ssZ}",
                token);
        }
    }
}
