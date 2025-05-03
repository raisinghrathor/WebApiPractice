using WebApp.Models;

namespace WebApp.Data
{
    public class WebApiExecuter : IWebApiExecuter
    {
        private readonly IHttpClientFactory clientFactory;
        private const string apiName = "ShirtsApi";
        public WebApiExecuter(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }
        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var client = clientFactory.CreateClient(apiName);
          //  return await client.GetFromJsonAsync<T>(relativeUrl);
          var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await client.SendAsync(request);
            await HandlePotentialError(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl,T obj)
        {
            var client = clientFactory.CreateClient(apiName);
           var response =await client.PostAsJsonAsync<T>(relativeUrl, obj);
            await HandlePotentialError(response);
            
            return await response.Content.ReadFromJsonAsync<T>();
        }
        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var client = clientFactory.CreateClient(apiName);
            var response = await client.PutAsJsonAsync<T>(relativeUrl, obj);
            await HandlePotentialError(response);
        }
        public async Task InvokeDelete(string relativeUrl)
        {
            var client = clientFactory.CreateClient(apiName);
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
    }
} 
