using System.Collections.Generic;
using WebAPI.Models.Entities;
using RoomManagement.Entities;

namespace RoomManagement
{
    public interface IUserManager
    {
        IList<user> GetAllUsers();
        user GetUserById(string id);
        string AddNewUser(user roomName);
        void UpdateUserProperties(string id, user room);
        void DeleteUser(string id);
        user GetUserFromMailId(string username);
        void SetUserProfilePic(string useremail,byte[] buffer, string filename);
        void SetUserActivation(user userId, bool flag);
    }
}