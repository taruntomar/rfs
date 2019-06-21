using DataLayer.Models;
using System.Net.Http;

namespace TAuthNIdentity
{
    public interface ITAuth
    {
        // below method returns the current logged in username, this method can be used to get username and can also used to check whether session is valid or invalid. If result is null or empty string then session is invalid. 
        string GetLoggedInUsername(System.Web.HttpRequestBase request);
        void Logout(System.Web.HttpRequestBase request, System.Web.HttpResponseBase response, System.Web.HttpSessionStateBase session);
        HttpResponseMessage Login(UserCredential c, System.Net.Http.HttpRequestMessage request);
    }
}