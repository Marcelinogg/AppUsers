using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppUsers.Data;
using WebAppUsers.Models;

namespace WebAppUsers.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }


        public IEnumerable<UserDTO> GetAll()
        {
            return _userRepository.GetAll()
                                .Select(x => new UserDTO
                                {
                                    UserId = x.UserId,
                                    LoginName = x.LoginName,
                                    FullName = x.FullName,
                                    Email = x.Email,
                                    Password = x.Password,
                                    Avatar = x.Avatar,
                                    ProfileId = x.ProfileId,
                                    Profile = x.Profile,
                                })
                                .ToList();
        }

        public UserDTO GetById(int userId)
        {
            User user = _userRepository.GetById(userId);

            return user != null                             // Checks the object because could by null and when is null send a 404 status
                ? new UserDTO
                {
                    UserId = user.UserId,
                    LoginName = user.LoginName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Password = user.Password,
                    Avatar = user.Avatar,
                    ProfileId = user.ProfileId,
                    Profile = user.Profile,
                }
                : null;
        }

        // Is used to check in the database if the new login is already occupied
        public bool IsAvailableLoginName(string loginName)
        {
            return _userRepository.IsAvailableLoginName(loginName);
        }


        public int Save(UserSaveDTO user)
        {
            OperationComplete(
                _userRepository.Save(
                new User
                {
                    LoginName = user.LoginName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Password = user.Password,
                    Avatar = user.Avatar,
                    ProfileId = user.ProfileId,
                }
                )
            );

            return _userRepository.GetUserIdByLoginName(user.LoginName);
        }

        public void Update(UserSaveDTO user)
        {
            OperationComplete(
                _userRepository.Update(
                new User
                {
                    UserId = user.UserId,
                    LoginName = user.LoginName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    ProfileId = user.ProfileId,
                }
                )
            );
        }

        public void ChangePassword(UserSaveDTO user)
        {
            OperationComplete(
                _userRepository.ChangePassword(
                new User
                {
                    UserId = user.UserId,
                    Password = user.Password,
                }
                )
            );
        }

        public void Remove(int userId)
        {
            OperationComplete(
                _userRepository.Remove(userId)
            );
        }

        // When the operation had affected the database it is good but when does not throw an exception to notify the log
        private void OperationComplete(string result)
        {
            if (!string.IsNullOrEmpty(result))
            {
                throw new Exception(result);
            }
        }
    }
}