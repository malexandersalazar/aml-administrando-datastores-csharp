using AML.Administrando.Datastores.CSharp.Responses;
using System.Text.Json;

namespace AML.Administrando.Datastores.CSharp.Services
{
    internal class IdentityService
    {
        private static HttpClient _httpClient = new HttpClient();

        public async Task<string> GetBearerTokenAsync(string tenantId, string clientId, string clientSecret)
        {
            var formContent = new Dictionary<string, string>();
            formContent.Add("grant_type", "client_credentials");
            formContent.Add("resource", "https://management.azure.com/");
            formContent.Add("client_id", clientId);
            formContent.Add("client_secret", clientSecret);

            var httpResponseMessage = await _httpClient.PostAsync($"https://login.microsoftonline.com/{tenantId}/oauth2/token", new FormUrlEncodedContent(formContent));
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                Console.WriteLine("Token:");
                Console.WriteLine(responseContent);
                Console.WriteLine();

                var token = JsonSerializer.Deserialize<TokenResponse>(responseContent);
                return token.access_token;
            }
            else
                throw new(httpResponseMessage.ReasonPhrase);
        }
    }
}