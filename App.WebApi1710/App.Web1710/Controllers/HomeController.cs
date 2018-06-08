using App.Web1710.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace App.Web1710.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentClient studentClient;

        public HomeController()
        {
            var apiClient = new ApiClient();
            studentClient = new StudentClient(apiClient);
        }

        public HomeController(IStudentClient studentClient)
        {
            this.studentClient = studentClient;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetStudent(int id)
        {
            //var student = await studentClient.GetStudent(id);
            //var student = await GetStudentAsync(id);
            return View(student);
        }

        public async Task<ActionResult> GetStudents()
        {
            var student = await GetStudentListAsync();
            return View(student);
        }

        public async Task<StudentModel> GetStudentAsync(int id)
        {
            StudentModel student = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string url = "http://localhost:14180/api/student?id=" + id;

                //Using https to satisfy iOS ATS requirements
                var response = await client.GetAsync(new Uri(url));

                //response.EnsureSuccessStatusCode(); //I was playing around with this to see if it makes any difference
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    student = JsonConvert.DeserializeObject<StudentModel>(content);
                }
            }
            return student;
        }

        public async Task<List<StudentModel>> GetStudentListAsync()
        {
            List<StudentModel> studentList = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string url = "http://localhost:14180/api/student";

                //Using https to satisfy iOS ATS requirements
                var response = await client.GetAsync(new Uri(url));

                //response.EnsureSuccessStatusCode(); //I was playing around with this to see if it makes any difference
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    studentList = JsonConvert.DeserializeObject<List<StudentModel>>(content);
                }
            }
            return studentList;
        }


    }
}