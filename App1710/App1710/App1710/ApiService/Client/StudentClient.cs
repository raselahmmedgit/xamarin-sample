using App1710.ApiHelper;
using App1710.ApiHelper.Client;
using App1710.ApiHelper.Response;
using App1710.ApiService.Model;
using App1710.ApiService.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace App1710.ApiService.Client
{
    public class StudentClient : ClientBase, IStudentClient
    {
        private ITokenContainer _iTokenContainer;
        private const string StudentUri = "api/student";
        private const string StudentListUri = "api/student";
        private const string IdKey = "id";

        public StudentClient(IApiClient iApiClient) : base(iApiClient)
        {
        }

        public async Task<CreateStudentResponse> CreateStudent(StudentModel student)
        {
            var apiModel = new StudentModel
            {
                StudentName = student.StudentName
            };
            var studentResponse = await PostEncodedContentWithSimpleResponse<CreateStudentResponse, StudentModel>(StudentListUri, apiModel);
            return studentResponse;
        }

        public async Task<StudentResponse> GetStudent(int studentId)
        {
            var idPair = IdKey.AsPair(studentId.ToString());
            return await GetJsonDecodedContent<StudentResponse, StudentModel>(StudentUri, idPair);
        }

        public async Task<StudentListResponse> GetStudentList()
        {
            return await GetJsonListDecodedContent<StudentListResponse, List<StudentModel>>(StudentUri);
        }

        //public async Task<StudentListResponse> GetStudentList()
        //{
        //    StudentListResponse studentListResponse;
        //    try
        //    {
        //        List<StudentModel> studentModelList = null;
        //        using (var httpClient = HttpClientInstance.Instance)
        //        {
        //            //Add Token
        //            _iTokenContainer = new TokenContainer();
        //            if (_iTokenContainer.IsApiCurrentToken())
        //            {
        //                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _iTokenContainer.ApiCurrentToken.ToString());
        //            }

        //            var response = await httpClient.GetAsync(StudentListUri);

        //            //response.EnsureSuccessStatusCode(); //I was playing around with this to see if it makes any difference
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var content = response.Content.ReadAsStringAsync().Result;
        //                studentModelList = JsonConvert.DeserializeObject<List<StudentModel>>(content);
        //            }

        //            studentListResponse = new StudentListResponse
        //            {
        //                StatusIsSuccessful = response.IsSuccessStatusCode,
        //                ResponseCode = response.StatusCode,
        //                DataList = studentModelList
        //            };
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        studentListResponse = new StudentListResponse
        //        {
        //            StatusIsSuccessful = false,
        //            ErrorState = new ErrorStateResponse() { Message = "Unauthorized." },
        //            ResponseCode = System.Net.HttpStatusCode.Unauthorized,
        //            DataList = null
        //        };
        //    }

        //    return studentListResponse;
        //}
    }

    public interface IStudentClient
    {
        Task<CreateStudentResponse> CreateStudent(StudentModel student);
        Task<StudentResponse> GetStudent(int id);
        Task<StudentListResponse> GetStudentList();
    }
}
