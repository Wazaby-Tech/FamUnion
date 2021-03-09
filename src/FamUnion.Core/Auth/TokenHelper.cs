using FamUnion.Auth;
using Newtonsoft.Json;
using RestSharp;

namespace FamUnion.Core.Auth
{
    public class TokenHelper
    {
        public static Auth0Token GetAuth0Token(AuthConfig authConfig)
        {
            var client = new RestClient($"https://{authConfig.Domain}/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json",
                JsonConvert.SerializeObject(
                    new
                    {
                        client_id = authConfig.ClientId,
                        client_secret = authConfig.ClientSecret,
                        audience = authConfig.Audience,
                        grant_type = "client_credentials"
                    })
                , ParameterType.RequestBody
                );
            var response = client.Execute(request);
            var token = JsonConvert.DeserializeObject<Auth0Token>(response.Content);
            return token;
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
