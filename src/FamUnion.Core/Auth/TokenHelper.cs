using FamUnion.Auth;
using Newtonsoft.Json;
using RestSharp;

namespace FamUnion.Core.Auth
{
    public class TokenHelper
    {
        public static Auth0Token GetAuth0Token(AuthConfig authConfig)
        {
            var options = new RestClientOptions($"https://{authConfig.Domain}");
            using var client = new RestClient(options);
            var request = new RestRequest("/oauth/token", Method.Post);
            request.AddJsonBody(new
            {
                client_id = authConfig.ClientId,
                client_secret = authConfig.ClientSecret,
                audience = authConfig.Audience,
                grant_type = "client_credentials"
            });
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<Auth0Token>(response.Content);
        }
    }

    public class Auth0Token
    {
        public string scope { get; set; }
        public long expires_in { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
}
