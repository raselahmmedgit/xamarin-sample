using App1710.ApiHelper.Response;
using App1710.ApiService.Model;
using System.Collections.Generic;

namespace App1710.ApiService.Response
{
    public class StudentResponse : ApiResponse<StudentModel>
    {
    }

    public class StudentListResponse : ApiListResponse<List<StudentModel>>
    {
    }
}
