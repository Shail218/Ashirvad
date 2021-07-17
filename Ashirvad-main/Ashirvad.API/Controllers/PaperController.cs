using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Paper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/paper/v1")]
    [AshirvadAuthorization]
    public class PaperController : ApiController
    {
        private readonly IPaperService _paperService = null;
        public PaperController(IPaperService paperService)
        {
            _paperService = paperService;
        }

        [Route("PaperMaintenance")]
        [HttpPost]
        public OperationResult<PaperEntity> PaperMaintenance(PaperEntity paperInfo)
        {
            OperationResult<PaperEntity> result = new OperationResult<PaperEntity>();

            var data = this._paperService.PaperMaintenance(paperInfo);
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllPaper")]
        [HttpGet]
        public OperationResult<List<PaperEntity>> GetAllPaper()
        {
            var data = this._paperService.GetAllPaper();
            OperationResult<List<PaperEntity>> result = new OperationResult<List<PaperEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllPaperByBranch")]
        [HttpGet]
        public OperationResult<List<PaperEntity>> GetAllPaper(long branchID)
        {
            var data = this._paperService.GetAllPaper(branchID);
            OperationResult<List<PaperEntity>> result = new OperationResult<List<PaperEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }


        [Route("GetPracticePaperSubject")]
        [HttpGet]
        public OperationResult<List<SubjectEntity>> GetPracticePaperSubject(long branchID, long stdID)
        {
            var data = this._paperService.GetPracticePaperSubject(branchID, stdID);
            return data.Result;
        }

        [Route("GetPracticePapersByStandardSubjectAndBranch")]
        [HttpGet]
        public OperationResult<List<PaperEntity>> GetPracticePapersByStandardSubjectAndBranch(long branchID, long stdID, long subID, int batchTypeID)
        {
            var data = this._paperService.GetPracticePapersByStandardSubjectAndBranch(branchID, stdID, subID, batchTypeID);
            return data.Result;
        }

        [Route("GetPracticePapersSubjectByStandardBatchAndBranch")]
        [HttpGet]
        public OperationResult<List<SubjectEntity>> GetPracticePapersSubjectByStandardBatchAndBranch(long branchID, long stdID, int batchTypeID)
        {
            var data = this._paperService.GetPracticePapersSubjectByStandardBatchAndBranch(branchID, stdID, batchTypeID);
            return data.Result;
        }

        [Route("GetPaperByPaperID")]
        [HttpGet]
        public OperationResult<PaperEntity> GetPaperByPaperID(long paperID)
        {
            var data = this._paperService.GetPaperByPaperID(paperID);
            OperationResult<PaperEntity> result = new OperationResult<PaperEntity>();
            result = data.Result;
            return result;
        }


        [Route("RemovePaper")]
        [HttpPost]
        public OperationResult<bool> RemovePaper(long paperID, string lastupdatedby)
        {
            var data = this._paperService.RemovePaper(paperID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }
    }
}
