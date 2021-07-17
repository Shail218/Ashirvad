using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/student/v1")]
    [AshirvadAuthorization]
    public class StudentController : ApiController
    {
        private readonly IStudentService _studentService = null;
        public StudentController(IStudentService studentService)
        {
            this._studentService = studentService;
        }

        [Route("StudentMaintenance")]
        [HttpPost]
        public OperationResult<StudentEntity> StudentMaintenance(StudentEntity studentInfo)
        {
            var data = this._studentService.StudentMaintenance(studentInfo);
            OperationResult<StudentEntity> result = new OperationResult<StudentEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }
        [Route("GetAllStudent")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllStudent(long branchID)
        {
            var data = await this._studentService.GetAllStudent(branchID);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllActiveStudent")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllActiveStudent(long branchID)
        {
            var data = await this._studentService.GetAllStudent(branchID, (int)Enums.RowStatus.Active);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllInActiveStudent")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllInActiveStudent(long branchID)
        {
            var data = await this._studentService.GetAllStudent(branchID, (int)Enums.RowStatus.Inactive);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllStudentByNameAndContact")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllStudent(string studName, string contactNo)
        {
            var data = await this._studentService.GetAllStudent(studName, contactNo);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("RemoveStudent")]
        [HttpPost]
        public OperationResult<bool> RemoveStudent(long StudentID, string lastupdatedby)
        {
            var data = this._studentService.RemoveStudent(StudentID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

    }

}
