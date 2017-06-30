using HackAtHome.Entities;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HackAtHome.SAL
{
    public class ServiceCaller
    {
        String WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
        public async Task<ResultInfo> AutentificateAsync(string studentEmail, string studentPassword)
        {
            ResultInfo Result = null;

            string EventID = "xamarin30";
            string RequestUri = "api/evidence/Authenticate";

            UserInfo User = new UserInfo
            {
                Email = studentEmail,
                Password = studentPassword,
                EventID = EventID

            };

            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(WebAPIBaseAddress);
                Client.DefaultRequestHeaders.Accept.Clear();

                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var JSONUserInfo = JsonConvert.SerializeObject(User);
                    HttpResponseMessage Response = await Client.PostAsync(RequestUri, new StringContent(JSONUserInfo.ToString(), Encoding.UTF8, "application/json"));

                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();

                    Result = JsonConvert.DeserializeObject<ResultInfo>(ResultWebAPI);
                }
                catch (Exception)
                {
                    Result = null;
                }
            }
            return Result;
        }

        public async Task<List<Evidence>> GetEvidencesAsync(string Token)
        {
            List<Evidence> Evidences = null;
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidences?token={Token}";

            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(WebAPIBaseAddress);
                Client.DefaultRequestHeaders.Accept.Clear();

                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var Response = await Client.GetAsync(URI);
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                        Evidences = JsonConvert.DeserializeObject<List<Evidence>>(ResultWebAPI);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return Evidences;
        }

        public async Task<EvidenceDetail> GetEvidenceByIDAsync(string token, int evidenceID)
        {
            EvidenceDetail Result = null;

            string URI = $"{WebAPIBaseAddress}api/evidence/getevidencebyid?token={token}&&evidenceid={evidenceID}";
            return Result;

            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(WebAPIBaseAddress);
                Client.DefaultRequestHeaders.Accept.Clear();

                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var Response = await Client.GetAsync(URI);

                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Result = JsonConvert.DeserializeObject<EvidenceDetail>(ResultWebAPI);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Result;
        }
    }
}
