using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace RFS.Models.EmailComm
{
    public class TemplateManager
    {

        public async Task<string> GetResetPasswordTemplate(string host)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress= new Uri(host);
            var result = await httpClient.GetAsync("/content/templates/reset-password.htm");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await result.Content.ReadAsStringAsync();
            }
            else
                return "";

        }

        internal async Task<string> GetAccountVerificatitonTemplateAsync(string host)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(host);
            var result = await httpClient.GetAsync("/content/templates/verification.htm");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await result.Content.ReadAsStringAsync();
            }
            else
                return "";

        }
    }

    
}