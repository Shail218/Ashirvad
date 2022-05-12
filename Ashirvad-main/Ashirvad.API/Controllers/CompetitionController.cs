using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/competition/v1")]
    [AshirvadAuthorization]
    public class CompetitionController : ApiController
    {
        private readonly ICompetitonService _competitionService;
        public CompetitionController(ICompetitonService competitionService)
        {
            _competitionService = competitionService;
        }

        [Route("GetAllCompetition")]
        [HttpGet]
        public OperationResult<List<CompetitionEntity>> GetAllCompetition()
        {
            var data = _competitionService.GetAllCompetiton();
            OperationResult<List<CompetitionEntity>> result = new OperationResult<List<CompetitionEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetStudentRank")]
        [HttpGet]
        public OperationResult<CompetitionRankEntity> GetStudentRank(long CompetitionID,long StudentID)
        {
            var data = _competitionService.GetStudentRank(CompetitionID, StudentID);
            OperationResult<CompetitionRankEntity> result = new OperationResult<CompetitionRankEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetCompetitionWinnerList")]
        [HttpGet]
        public OperationResult<List<CompetitionWinnerEntity>> GetCompetitionWinnerList()
        {
            var data = _competitionService.GetCompetitionWinnerListbyCompetitionId();
            OperationResult<List<CompetitionWinnerEntity>> result = new OperationResult<List<CompetitionWinnerEntity>>();
            result.Completed = data.Result.Status;
            result.Data = data.Result.Data;
            return result;
        }

        [Route("CompetitionAnswerSheetMaintenance/{CompetitionID}/{BranchID}/{StudentID}/{Status}/{SubmitDate}/{CreateId}/{CreateBy}")]
        [HttpPost]
        public OperationResult<CompetitionAnswerSheetEntity> CompetitionAnswerSheetMaintenance(long CompetitionID, long BranchID, long StudentID,
            int Status, DateTime SubmitDate, long CreateId, string CreateBy)
        {
            OperationResult<CompetitionAnswerSheetEntity> result = new OperationResult<CompetitionAnswerSheetEntity>();

            CompetitionAnswerSheetEntity CompetitionDetail = new CompetitionAnswerSheetEntity();
            CompetitionAnswerSheetEntity Response = new CompetitionAnswerSheetEntity();
            ResponseModel ResponseM = new ResponseModel();

            CompetitionDetail.competitionInfo = new CompetitionEntity();
            CompetitionDetail.branchInfo = new BranchEntity();
            CompetitionDetail.studentInfo = new StudentEntity();
            var httpRequest = HttpContext.Current.Request;
            CompetitionDetail.competitionInfo.CompetitionID = CompetitionID;
            CompetitionDetail.branchInfo.BranchID = BranchID;
            CompetitionDetail.studentInfo.StudentID = StudentID;
            CompetitionDetail.Remarks = "";
            CompetitionDetail.Status = Status;
            CompetitionDetail.SubmitDate = SubmitDate;
            CompetitionDetail.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            CompetitionDetail.Transaction = new TransactionEntity()
            {
                CreatedBy = CreateBy,
                CreatedId = CreateId,
                CreatedDate = DateTime.Now,
            };
            var data1 = this._competitionService.RemoveCompetitionAnswerSheetdetail(CompetitionID, StudentID);
            if (httpRequest.Files.Count > 0)
            {
                try
                {
                    for (int file = 0; file < httpRequest.Files.Count; file++)
                    {
                        string fileName;
                        string extension;
                        string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                        // for live server
                        //string UpdatedPath = currentDir.Replace("mastermindapi", "mastermind");
                        // for local server
                        string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                        var postedFile = httpRequest.Files[file];
                        string randomfilename = Common.Common.RandomString(20);
                        extension = Path.GetExtension(postedFile.FileName);
                        fileName = Path.GetFileName(postedFile.FileName);
                        string _Filepath = "/CompetitionImage/" + randomfilename + extension;
                        string _Filepath1 = "CompetitionImage/" + randomfilename + extension;
                        var filePath = HttpContext.Current.Server.MapPath("~/CompetitionImage/" + randomfilename + extension);
                        string _path = UpdatedPath + _Filepath1;
                        postedFile.SaveAs(_path);
                        CompetitionDetail.CompetitionSheetName = fileName;
                        CompetitionDetail.CompetitionFilepath = _Filepath;
                        var data = this._competitionService.CompetitionSheetMaintenance(CompetitionDetail);
                        ResponseM = data.Result;
                    }
                    result.Completed = ResponseM.Status;
                    if (ResponseM.Status)
                    {
                        result.Data = (CompetitionAnswerSheetEntity)ResponseM.Data;
                    }
                    result.Message = ResponseM.Message;

                }
                catch (Exception ex)
                {
                    result.Data = Response;
                    result.Completed = false;
                    result.Message = ex.Message.ToString();
                }

            }
            else
            {
                var data = this._competitionService.CompetitionSheetMaintenance(CompetitionDetail);
                // Response = data.Result;

                result.Completed = data.Result.Status;
                if (data.Result.Status && data.Result.Data != null)
                {
                    result.Data = (CompetitionAnswerSheetEntity)data.Result.Data;
                }
                result.Message = data.Result.Message;
            }
            return result;


        }

    }
}
