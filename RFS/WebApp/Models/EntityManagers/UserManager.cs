using RFS.Models;
using RFS.Models.Entities;
using RoomManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManagement
{
    public class UserManager : IUserManager
    {
        private IRoomManagementDatabasseEntities _dbContext;
        public UserManager(IRoomManagementDatabasseEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public string AddNewUser(user user)
        {
            var __user =  _dbContext.users.FirstOrDefault();
            if (__user == null)
            {
                _dbContext.users.Add(user);
                _dbContext.SaveChanges();
                return "User added successfully.";
            }
            else
            {
                return "User with same name and country already exist.";
            }
        }

        public void DeleteUser(string id)
        {
            var user = _dbContext.users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                _dbContext.users.Remove(user);
                _dbContext.SaveChanges();
            }
          
        }

        public IList<user> GetAllUsers()
        {
            List<user> users = new List<user>();
            try
            {
                users = _dbContext.users.ToList();
            }catch(Exception ex)
            {
                var asdf = ex;
            }
            return users;
        }

        public user GetUserById(string id)
        {
            var user = _dbContext.users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        

        public user GetUserFromMailId(string email)
        {
            return _dbContext.users.FirstOrDefault(x => x.email == email);
        }

        public void SetUserProfilePic(string usermail, byte[] buffer, string filename)
        {
            var user = GetUserFromMailId(usermail);
            UserProfilePic userProfilePic= null;
            if (user.UserProfilePics == null || user.UserProfilePics.Count == 0)
            {
                userProfilePic = new UserProfilePic();
                userProfilePic.Id = Guid.NewGuid().ToString();
                _dbContext.UserProfilePicture.Add(userProfilePic);
            }
            else
            {
                userProfilePic = user.UserProfilePics.FirstOrDefault();
            }
        
            userProfilePic.data = buffer;
            userProfilePic.ext = filename.Split('.')[1];
            userProfilePic.UserId = user.Id;
            _dbContext.SaveChanges();
        }

        public void UpdateUserProperties(string id, user user)
        {
            var loc = _dbContext.users.FirstOrDefault(x => x.Id == id);
           
            _dbContext.SaveChanges();
        }
    }
}
