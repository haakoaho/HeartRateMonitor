using HeartRateApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HeartRateApp.Backend
{
    class APIModule
    {
        private const string url = "https://192.168.10.115:5001";
        private static HttpClient httpClient = new HttpClient(GetInsecureHandler());
        internal static async Task<long> GetUserIdAsync(string username)
        {
            HttpResponseMessage msg = await httpClient.GetAsync(url+"/User?username="+username,HttpCompletionOption.ResponseContentRead);
            return long.Parse(await msg.Content.ReadAsStringAsync());
        }

        internal static async Task<List<HeartRateModel>> GetRecords(long userId)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url + "/HeartRate?userId=" + userId , HttpCompletionOption.ResponseContentRead);
            string msg = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<HeartRateModel>>(msg);
        }

        internal static async Task CreateUserAsync(string username)
        {
            await httpClient.PostAsync(url + "/User?username=" + username, null);
        }

        internal static async Task DeleteRecord(long recordId)
        {
            await httpClient.DeleteAsync(url + "/HeartRate?recordId=" + recordId);
        }

        /// <summary>
        /// Use this as we are using self signed certificate
        /// </summary>
        /// <returns></returns>
        private static HttpClientHandler GetInsecureHandler()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        internal static async Task CreateRecord(CreateHeartRateRequestModel createHeartRateRequestModel)
        {
            HttpContent content = new StringContent( JsonConvert.SerializeObject(createHeartRateRequestModel), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(url + "/HeartRate", content);
        }
    }
}
