using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppUsers.Data;
using WebAppUsers.Models;

namespace WebAppUsers.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ProfileRepository _profileRepository;

        public ProfileService()
        {
            _profileRepository = new ProfileRepository();
        }

        public IEnumerable<ProfileDTO> GetAll()
        {
            return _profileRepository.GetAll()
                                    .Select(x=> new ProfileDTO
                                    {
                                        Id = x.Id,
                                        Name = x.Name
                                    })
                                    .ToList();
        }
    }
}