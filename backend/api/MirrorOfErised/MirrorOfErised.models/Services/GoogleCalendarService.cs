using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MirrorOfErised.models.Repos;

namespace MirrorOfErised.models.Services
{
    public class GoogleCalendarService: GoogleService
    {
        public GoogleCalendarService(HttpClient client, IAuthTokenRepo authTokenRepo) : base(client, authTokenRepo)
        {
        }
        
        public async Task<string> GetCalendarEvents(AuthToken token)
        {
            return await AuthenticatedGet($"calendar/v3/calendars/primary/events?timeMin={DateTime.Now:yyyy-MM-ddTHH:mm:ssZ}",
                token);
        }
    }
}
