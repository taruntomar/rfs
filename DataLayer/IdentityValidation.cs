using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class IdentityValidation
    {
        private string _dbConnectionString;
        public IdentityValidation(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }
        public bool ValidateUserCredential(UserCredential c)
        {
            bool loginsuccessful = false;
            SqlConnection sqlConnection = new SqlConnection(_dbConnectionString);
            SqlCommand command;
            string sql = "select * from users where email like '" + c.username + "'";
            SqlDataReader dataReader;
            try
            {
                sqlConnection.Open();
                command = new SqlCommand(sql, sqlConnection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    string id = dataReader.GetValue(0).ToString();
                    string email = dataReader.GetValue(1).ToString();
                    string salt = dataReader.GetValue(3).ToString();
                    string password = dataReader.GetValue(2).ToString();

                    if (password == GetHashedPassword(c.password, salt))
                    {
                        // password matched.
                        loginsuccessful = true;
                    }
                    break;

                }

                dataReader.Close();
                command.Dispose();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                // db connection string
                

            }
            return loginsuccessful;
        }

        public bool CheckLoginCode(string username, string logincode)
        {
            bool loginsuccessful = false;
            SqlConnection sqlConnection = new SqlConnection(_dbConnectionString);
            SqlCommand command;
            string sql = "select * from users where email like '" + username + "'";
            SqlDataReader dataReader;
            try
            {
                sqlConnection.Open();
                command = new SqlCommand(sql, sqlConnection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    string logincode2 = dataReader.GetValue(4).ToString();
                    

                    if (logincode == logincode2)
                    {
                        // password matched.
                        loginsuccessful = true;
                    }
                    break;

                }

                dataReader.Close();
                command.Dispose();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {

            }
            return loginsuccessful;
        }

        public void UpdatePassword(string email, string password)
        {

            
            SqlConnection sqlConnection = new SqlConnection(_dbConnectionString);
            SqlCommand command = null;
            try
            {
                sqlConnection.Open();
                string sql = "select * from users where email like '" + email + "'";

                command = new SqlCommand(sql, sqlConnection);
                SqlDataReader dataReader;
                dataReader = command.ExecuteReader();
                string salt = "";
                while (dataReader.Read())
                {
                   salt = dataReader["salt"].ToString();
                }
                dataReader.Close();
                string hashedPassword = GetHashedPassword((string)password, salt);
                sql = "update users set password = '" + hashedPassword + "' where email like '" + email + "'";

                command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }

            if (command != null)
                command.Dispose();
            sqlConnection.Close();

        }

        public bool CheckPasswordResetCode(string email, string code)
        {
             bool loginsuccessful = false;
            SqlConnection sqlConnection = new SqlConnection(_dbConnectionString);
            SqlCommand command;
            string sql = "select * from users where email like '" + email + "'";
            SqlDataReader dataReader;
            try
            {
                sqlConnection.Open();
                command = new SqlCommand(sql, sqlConnection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    string passwordResetCode = dataReader["passResetCode"].ToString();


                    if (passwordResetCode == code)
                    {
                        // password matched.
                        loginsuccessful = true;
                    }
                    break;

                }

                dataReader.Close();
                command.Dispose();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {

            }
            return loginsuccessful;
        }

        static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public void SetLoginCode(string username, string loginCode)
        {
            string sql = "update users set logincode = '"+ loginCode + "' where email like '"+username+"'";

            SqlConnection sqlConnection = new SqlConnection(_dbConnectionString);
            SqlCommand command = null;
            try
            {
                sqlConnection.Open();
                command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }

            if (command != null)
                command.Dispose();
            sqlConnection.Close();

        }

        public void StorePasswordResetCode(string email, string passResetCode)
        {
            string sql = "update users set passResetCode = '" + passResetCode + "' where email like '" + email + "'";

            SqlConnection sqlConnection = new SqlConnection(_dbConnectionString);
            SqlCommand command = null;
            try
            {
                sqlConnection.Open();
                command = new SqlCommand(sql, sqlConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }

            if (command != null)
                command.Dispose();
            sqlConnection.Close();
        }

        public void RegisterUser(dynamic c,string code)
        {
            // generate salt 
            string salt = Guid.NewGuid().ToString();
            // has password
            string hashedPassword = GetHashedPassword((string)c.password, salt);

            string sql = "INSERT into users (Id,email,password,salt,logincode,Name,location,phone,IsActivated,isAdmin,IsVerified,VerificationCode) VALUES (@Id,@email,@password,@salt,@logincode,@Name,@location,@phone,@IsActivated,@isAdmin,@IsVerified,@VerificationCode)";

            SqlConnection sqlConnection = new SqlConnection(_dbConnectionString);
            SqlCommand command;
            SqlDataReader dataReader;
            try
            {
                sqlConnection.Open();
                command = new SqlCommand(sql, sqlConnection);
                command.Parameters.Add("@Id", SqlDbType.NVarChar, 50).Value = Guid.NewGuid().ToString();
                command.Parameters.Add("@email", SqlDbType.NVarChar, 150).Value =(string) c.email;
                command.Parameters.Add("@password", SqlDbType.Text).Value = hashedPassword;
                command.Parameters.Add("@salt", SqlDbType.NVarChar, 50).Value = salt;
                command.Parameters.Add("@logincode", SqlDbType.NVarChar, 50).Value = "";
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = (string)c.name;
                command.Parameters.Add("@location", SqlDbType.NVarChar, 50).Value = "Bangalore";
                command.Parameters.Add("@phone", SqlDbType.NVarChar, 50).Value = (string)c.phone;
                command.Parameters.Add("@IsActivated", SqlDbType.NVarChar, 50).Value = false;
                command.Parameters.Add("@isAdmin", SqlDbType.NVarChar, 50).Value = false;
                command.Parameters.Add("@IsVerified", SqlDbType.NVarChar, 50).Value = false;
                command.Parameters.Add("@VerificationCode", SqlDbType.NVarChar, 50).Value = code;
                command.ExecuteNonQuery();
                command.Dispose();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {

            }

        }

        private static string GetHashedPassword(string password, string salt)
        {
            byte[] passwordHashByte = GenerateSaltedHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));
            string hashedPassword = Convert.ToBase64String(passwordHashByte);
            return hashedPassword;
        }

        public bool UserExist(UserCredential c)
        {
            bool userrExist = false;
            SqlConnection sqlConnection = new SqlConnection(_dbConnectionString);
            SqlCommand command;
            string sql = "select * from users where email like '" + c.username + "'";
            SqlDataReader dataReader;
            try
            {
                sqlConnection.Open();
                command = new SqlCommand(sql, sqlConnection);
                dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    userrExist = true;
                }

                dataReader.Close();
                command.Dispose();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {

            }
            return userrExist;
        }
    }
}
