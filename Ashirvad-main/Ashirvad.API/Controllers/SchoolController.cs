using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/school/v1")]
    [AshirvadAuthorization]
    public class SchoolController : ApiController
    {
        private readonly ISchoolService _schoolService = null;
        public SchoolController(ISchoolService schoolService)
        {
            this._schoolService = schoolService;
        }

        [Route("SchoolMaintenance")]
        [HttpPost]
        public OperationResult<SchoolEntity> SchoolMaintenance(SchoolEntity schoolInfo)
        {
            var data = this._schoolService.SchoolMaintenance(schoolInfo);
            OperationResult<SchoolEntity> result = new OperationResult<SchoolEntity>();
            result.Completed = data.Result.Status;
            result.Message = data.Result.Message;
            if (data.Result.Status)
            {
                result.Data = (SchoolEntity)data.Result.Data;
            }
            return result;
        }

        [Route("GetAllSchools")]
        [HttpPost]
        public OperationResult<List<SchoolEntity>> GetAllSchools(long branchID)
        {
            var data = this._schoolService.GetAllSchools(branchID);
            OperationResult<List<SchoolEntity>> result = new OperationResult<List<SchoolEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }


        [Route("RemoveSchool")]
        [HttpPost]
        public OperationResult<bool> RemoveSchool(long SchoolID, string lastupdatedby)
        {
            var data = this._schoolService.RemoveSchool(SchoolID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }

    }
}
