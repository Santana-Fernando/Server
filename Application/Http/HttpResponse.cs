using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Application.Http
{
    public class HttpResponse
    {
        public HttpResponseMessage Response(HttpStatusCode status, StringContent? messages, string reasonPhrase)
        {
            return new HttpResponseMessage(status)
            {
                Content = new StringContent(JsonSerializer.Serialize(messages)),
                ReasonPhrase = reasonPhrase
            };
        }
    }
}
