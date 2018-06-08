using App1710.ApiHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1710.ApiService.Model
{
    public class RegisterModel : ApiModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
