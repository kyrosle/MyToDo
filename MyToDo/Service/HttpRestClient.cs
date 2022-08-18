using MyToDo.Share;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class HttpRestClient
    {
        private readonly string apiUrl;
        protected readonly RestClient restClient;

        public HttpRestClient(string apiUrl)
        {
            this.apiUrl = apiUrl;
            restClient = new RestClient();
        }

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if (baseRequest.Parameter is not null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            restClient.BaseUrl = new Uri(apiUrl + baseRequest.Route);

            IRestResponse response = await restClient.ExecuteAsync(request);

            return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
        }
        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if (baseRequest.Parameter is not null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            restClient.BaseUrl = new Uri(apiUrl + baseRequest.Route);

            IRestResponse response = await restClient.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
        }
    }
}
