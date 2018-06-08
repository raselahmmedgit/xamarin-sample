using App.Web1710.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web1710.Models
{
    public class StudentModel
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; }
    }

    public class StudentResponse : ApiResponse<StudentModel>
    {
    }
}