using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebAppUsers.Data
{
    public class ProfileRepository : DataAccessRepository
    {
        public IEnumerable<Profile> GetAll()
        {
            string query = "dbo.GetProfiles_Se";
            DataTable dataTable = SpGetDataFromBD(query);

            return dataTable.AsEnumerable()
                            .Select(x => new Profile
                            {
                                Id = x.Field<int>("Id"),
                                Name = x.Field<string>("Name"),
                            })
                            .ToList();
        }
    }
}