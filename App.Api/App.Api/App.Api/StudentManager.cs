using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App.Api
{
    public class StudentManager
    {
        public async Task<List<Student>> GetStudentListAsync()
        {
            List<Student> studentList = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string url = "http://localhost:47641/api/default";

                //Using https to satisfy iOS ATS requirements
                var response = await client.GetAsync(new Uri(url));

                //response.EnsureSuccessStatusCode(); //I was playing around with this to see if it makes any difference
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    studentList = JsonConvert.DeserializeObject<List<Student>>(content);
                }
            }
            return studentList;
        }

        //public List<Student> GetStudentList()
        //{
        //    List<Student> studentList = null;
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        string url = "http://localhost:47641/api/default";

        //        //Using https to satisfy iOS ATS requirements
        //        var response = await client.GetAsync(new Uri(url));

        //        //response.EnsureSuccessStatusCode(); //I was playing around with this to see if it makes any difference
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync().Result;
        //            studentList = JsonConvert.DeserializeObject<List<Student>>(content);
        //        }
        //    }
        //    return studentList;
        //}

    }
}
