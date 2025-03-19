using DomainModel.Models.Request;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RIAYATIEPrescriptionAPI.Services
{
    public class ErxDHAWebRequestService
    {
        public async Task<HttpResponseMessage> Post(ApiRequestModel model)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var soapAction = $"{model.ApiUrl}/{model.Method}";
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, model.EndPoint);
                request.Content = new StringContent(model.StringContent, Encoding.UTF8, "text/xml");
                request.Headers.Add("SOAPAction", soapAction);
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    responseMessage.Content = new StringContent(responseContent);
                    responseMessage.StatusCode = System.Net.HttpStatusCode.OK;
                    responseMessage.ReasonPhrase = response.ReasonPhrase;
                }
                else
                {
                    responseMessage = response;
                }
            }

            return responseMessage;

        }
    }
}