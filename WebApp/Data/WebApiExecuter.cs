using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Data
{
    public class WebApiExecuter : IWebApiExecuter
    {
        private readonly IHttpClientFactory clientFactory;
        private const string apiName = "ShirtsApi";
        private const string authApiName = "AuthorityApi";

        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public WebApiExecuter(IHttpClientFactory clientFactory,IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            this.clientFactory = clientFactory;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var client = clientFactory.CreateClient(apiName);
           await AddJwtToken(client);
          //  return await client.GetFromJsonAsync<T>(relativeUrl);
          var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await client.SendAsync(request);
            await HandlePotentialError(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl,T obj)
        {
            var client = clientFactory.CreateClient(apiName);
            await AddJwtToken(client);
            var response =await client.PostAsJsonAsync<T>(relativeUrl, obj);
            await HandlePotentialError(response);
            
            return await response.Content.ReadFromJsonAsync<T>();
        }
        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var client = clientFactory.CreateClient(apiName);
            await AddJwtToken(client);
            var response = await client.PutAsJsonAsync<T>(relativeUrl, obj);
            await HandlePotentialError(response);
        }
        public async Task InvokeDelete(string relativeUrl)
        {
            var client = clientFactory.CreateClient(apiName);
            await AddJwtToken(client);
            var response = await client.DeleteAsync(relativeUrl);
            await HandlePotentialError(response);
        }
        private async Task HandlePotentialError(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorJson = await httpResponse.Content.ReadAsStringAsync();
                throw new WebApiException(errorJson);
            }
        }
        private async Task AddJwtToken( HttpClient httpClient)
        {

            //Check if token is already in session
            JwtToken? token = null;
            var tokenString = httpContextAccessor.HttpContext?.Session.GetString("acces_token");
            if (!string.IsNullOrWhiteSpace(tokenString))
            {
                token = JsonConvert.DeserializeObject<JwtToken>(tokenString);
            }
            if (token == null|| token.ExpiresAt<DateTime.UtcNow)
            {
                var clientId = configuration.GetValue<string>("ClientId");
                var secret = configuration.GetValue<string>("Secret");
                //Authenticate
                var authClient = clientFactory.CreateClient(authApiName);
                var response = await authClient.PostAsJsonAsync("auth", new AppCredentials
                {
                    ClientId = clientId,
                    Secret = secret
                });
                response.EnsureSuccessStatusCode();
                //Get Token
                var strToken = await response.Content.ReadAsStringAsync();
                 token = JsonConvert.DeserializeObject<JwtToken>(strToken);

                httpContextAccessor.HttpContext?.Session.SetString("acces_token", strToken);
            }
            
            //Add Token to Header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);

        }
    }
} 
