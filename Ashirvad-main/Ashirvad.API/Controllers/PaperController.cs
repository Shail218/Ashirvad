using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Paper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
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

        [Route("GetAllPaperWithoutContent")]
        [HttpGet]
        public OperationResult<List<PaperEntity>> GetAllPaperWithoutContent(long branchID)
        {
            var data = this._paperService.GetAllPaperWithoutContent(branchID);
            return data.Result;
        }


        [Route("GetPracticePaperSubject")]
        [HttpGet]
        public OperationResult<List<SubjectEntity>> GetPracticePaperSubject(long branchID, long courseid,long stdID,int batch_time)
        {
            var data = this._paperService.GetPracticePaperSubject(branchID,courseid, stdID,batch_time);
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

        [Route("PaperMaintenance")]
        [HttpPost]
        public OperationResult<PaperEntity> PaperMaintenance(string model,bool HasFile)
        {
            OperationResult<PaperEntity> result = new OperationResult<PaperEntity>();
            var httpRequest = HttpContext.Current.Request;
            PaperEntity paperEntity = new PaperEntity();
            paperEntity.Branch = new BranchEntity();
            paperEntity.BranchCourse = new BranchCourseEntity();
            paperEntity.BranchClass = new BranchClassEntity();
            paperEntity.BranchSubject = new BranchSubjectEntity();
            paperEntity.PaperData = new PaperData();
            var entity = JsonConvert.DeserializeObject<PaperEntity>(model);
            paperEntity.PaperID = entity.PaperID;
            paperEntity.PaperData.UniqueID = entity.PaperData.UniqueID;
            paperEntity.Branch.BranchID = entity.Branch.BranchID;
            paperEntity.BranchCourse.course_dtl_id = entity.BranchCourse.course_dtl_id;
            paperEntity.BranchClass.Class_dtl_id = entity.BranchClass.Class_dtl_id;
            paperEntity.BranchSubject.Subject_dtl_id = entity.BranchSubject.Subject_dtl_id;
            paperEntity.BatchTypeID = entity.BatchTypeID;
            paperEntity.Remarks = entity.Remarks;      
            paperEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            paperEntity.Transaction = new TransactionEntity()
            {
                TransactionId = entity.Transaction.TransactionId,
                LastUpdateBy = entity.Transaction.LastUpdateBy,
                LastUpdateId = entity.Transaction.LastUpdateId,
                CreatedBy = entity.Transaction.CreatedBy,
                CreatedId = entity.Transaction.CreatedId
            };
            if (HasFile)
            {
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        foreach (string file in httpRequest.Files)
                        {
                            string fileName;
                            string extension;
                            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                            string UpdatedPath = currentDir.Replace("WebAPI", "wwwroot");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/PaperDocument/" + randomfilename + extension;
                            string _Filepath1 = "PaperDocument/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/PaperDocument/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            paperEntity.PaperData.PaperPath = fileName;
                            paperEntity.PaperData.FilePath = _Filepath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Completed = false;
                    result.Data = null;
                    result.Message = ex.ToString();
                }
            }
            else
            {
                paperEntity.PaperData.PaperPath = entity.PaperData.PaperPath;
                paperEntity.PaperData.FilePath = entity.PaperData.FilePath;
            }
            var data = this._paperService.PaperMaintenance(paperEntity).Result;
            result.Completed = false;
            result.Data = null;
            if (data.PaperID > 0 || data.PaperData.UniqueID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (entity.PaperID > 0)
                {
                    result.Message = "Practice Paper Updated Successfully.";
                }
                else
                {
                    result.Message = "Practice Paper Created Successfully.";
                }
            }
            return result;
        }

        public static string Decode(string Path)
        {
            byte[] mybyte = Convert.FromBase64String(Path);
            string returntext = Encoding.UTF8.GetString(mybyte);
            return returntext;
        }

    }
}
