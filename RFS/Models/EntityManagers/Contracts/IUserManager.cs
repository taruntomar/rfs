using System.Collections.Generic;
using RFS.Models.Entities;
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
    }
}