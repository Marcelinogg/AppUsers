﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppUsers.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public int ProfileId { get; set; }
        public string Profile { get; set; }
    }
}