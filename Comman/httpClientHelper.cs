using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using System.Text;

namespace EmployeeManagementSystem.Comman
{
    public class httpClientHelper
    {
        public static async Task<string> MakePostRequest(string baseUrl, string endPoint, string apiRequestData)
        {
            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 10

            };

            using (var httpClient = new HttpClient(socketsHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                httpClient.BaseAddress = new Uri(baseUrl);
                StringContent content = new StringContent(apiRequestData, Encoding.UTF8, "application/json");

                var httpResponse = httpClient.PostAsync(endPoint, content).Result;
                var httpStringResponse = httpResponse.Content.ReadAsStringAsync().Result;

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpStringResponse);
                }
                return httpStringResponse;


            }
        }

        public static async Task<string> MakeGetRequest(string baseUrl, string endPoint)
        {
            var socketHandler = new SocketsHttpHandler
            {
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(10),
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 10
            };

            using (var httpClient = new HttpClient(socketHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                httpClient.BaseAddress = new Uri(baseUrl);

                var response = await httpClient.GetAsync(endPoint);
                var responseString = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(responseString);
                }
                return responseString;
            };

        }

        internal static async Task<string> MakeGetRequest(string managerUrl, object getAllManagerEndPoint)
        {
            throw new NotImplementedException();
        }
    }
}
