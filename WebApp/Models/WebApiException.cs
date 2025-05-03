using System.Text.Json;

namespace WebApp.Models
{
    public class WebApiException:Exception
    {
        public ErrorResponse? Response { get;}
        public WebApiException(string errorJson)
        {
                Response = JsonSerializer.Deserialize<ErrorResponse>(errorJson);
        }
    }
}
