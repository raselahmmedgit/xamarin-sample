using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace App.Web1710.Models
{
    public interface IStudentClient
    {
        Task<StudentResponse> GetStudent(int studentId);
    }

    public class StudentClient : ClientBase, IStudentClient
    {
        private const string StudentUri = "api/student";

        public StudentClient(IApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<StudentResponse> CreateProduct(StudentModel student)
        {
            var apiModel = new StudentModel
            {
                StudentName = student.StudentName
            };
            var createStudentResponse = await PostEncodedContentWithSimpleResponse<StudentResponse, StudentModel>(StudentUri, apiModel);
            return createStudentResponse;
        }

        public async Task<StudentResponse> GetStudent(int studentId)
        {
            var idPair = new KeyValuePair<string, string>("id", studentId.ToString());
            return await GetJsonDecodedContent<StudentResponse, StudentModel>(StudentUri, idPair);
        }
    }

}