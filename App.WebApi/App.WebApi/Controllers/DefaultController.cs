using App.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.WebApi.Controllers
{
    public class DefaultController : ApiController
    {
        List<Student> studentList = new List<Student>
        {
            new Student { Id = 1, Name = "Rasel" },
            new Student { Id = 2, Name = "Sohel" },
            new Student { Id = 3, Name = "Nahid" }
        };

        public IEnumerable<Student> GetAllStudents()
        {
            return studentList.ToArray();
        }

        public IHttpActionResult GetStudent(int id)
        {
            var student = studentList.FirstOrDefault((s) => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }
    }
}
