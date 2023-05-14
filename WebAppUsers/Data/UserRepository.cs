using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebAppUsers.Data
{
    public class UserRepository : DataAccessRepository
    {
        public IEnumerable<User> GetAll()
        {
            string query = "dbo.GetUsers_Se";
            DataTable dataTable = SpGetDataFromBD(query);

            return dataTable.AsEnumerable()
                            .Select(x => new User
                            {
                                UserId = x.Field<int>("UserId"),
                                LoginName = x.Field<string>("LoginName"),
                                FullName = x.Field<string>("FullName"),
                                Email = x.Field<string>("Email"),
                                Password = x.Field<string>("Password"),
                                Avatar = x.Field<string>("Avatar"),
                                ProfileId = x.Field<int>("ProfileId"),
                                Profile = x.Field<string>("Profile"),
                            })
                            .ToList();
        }

        public User GetById(int userId)
        {
            return GetAll().First(x => x.UserId == userId);
        }

        public string Save(User user)
        {
            string query = "dbo.AddUser_In";
            int result = SpSaveDataToBD(query,
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("@loginName", user.LoginName),
                    new KeyValuePair<string, string>("@fullName", user.FullName),
                    new KeyValuePair<string, string>("@email", user.Email),
                    new KeyValuePair<string, string>("@password", user.Password),
                    new KeyValuePair<string, string>("@avatar", user.Avatar),
                    new KeyValuePair<string, string>("@profileId", user.ProfileId.ToString()),
                }                               
                );

            return ResponeDB(result);
        }

        public string Update(User user)
        {
            string query = "dbo.ChangeDataUser_Up";
            int result = SpSaveDataToBD(query,
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("@userId", user.UserId.ToString()),
                    new KeyValuePair<string, string>("@loginName", user.LoginName),
                    new KeyValuePair<string, string>("@fullName", user.FullName),
                    new KeyValuePair<string, string>("@email", user.Email),
                    new KeyValuePair<string, string>("@avatar", user.Avatar),
                    new KeyValuePair<string, string>("@profileId", user.ProfileId.ToString()),
                }
                );

            return ResponeDB(result);
        }
        public string ChangePassword(User user)
        {
            string query = "dbo.ChangePasswordUser_Up";
            int result = SpSaveDataToBD(query,
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("@userId", user.UserId.ToString()),
                    new KeyValuePair<string, string>("@password", user.Password),
                 }
                );

            return ResponeDB(result);
        }

        public string Remove(int userId)
        {
            string query = "dbo.RemoveUser_De";
            int result = SpSaveDataToBD(query,
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("@userId", userId.ToString()),
                 }
                );

            return ResponeDB(result);
        }
    }
}