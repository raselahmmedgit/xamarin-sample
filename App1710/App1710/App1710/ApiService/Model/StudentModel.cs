using App1710.ApiHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1710.ApiService.Model
{
    public class StudentModel : ApiModel
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; }
    }
}
