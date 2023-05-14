using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppUsers.Models;

namespace WebAppUsers.Services
{
    interface IProfileService
    {
        IEnumerable<ProfileDTO> GetAll();
    }
}
