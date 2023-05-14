using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppUsers.Models;

namespace WebAppUsers.Services
{
    interface IUserService
    {
        IEnumerable<UserDTO> GetAll();
        UserDTO GetById(int userId);

        void Save(UserSaveDTO user);
        void Update(UserSaveDTO user);
        void ChangePassword(UserSaveDTO user);
        void Remove(int userId);
    }
}
