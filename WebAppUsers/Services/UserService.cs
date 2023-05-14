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
                                    Avatar = x.Avatar,
                                    Profile = x.Profile,
                                })
                                .ToList();
        }

        public UserDTO GetById(int userId)
        {
            User user = _userRepository.GetById(userId);

            return user != null
                ? new UserDTO
                {
                    UserId = user.UserId,
                    LoginName = user.LoginName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    Profile = user.Profile,
                }
                : null;
        }

        public bool IsAvailableLoginName(string loginName)
        {
            return _userRepository.IsAvailableLoginName(loginName);
        }


        public void Save(UserSaveDTO user)
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


        private void OperationComplete(string result)
        {
            if (!string.IsNullOrEmpty(result))
            {
                throw new Exception(result);
            }
        }
    }
}