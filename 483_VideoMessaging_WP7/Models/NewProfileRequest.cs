using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _483_VideoMessaging_WP7.Models
{
    public class NewProfileRequest
    {
        public string EmailAddress { get; set;}
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public NewProfileRequest(string email, string username, string password, string confirmPassword)
        {
            EmailAddress = email;
            Username = username;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
