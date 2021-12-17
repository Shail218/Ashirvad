using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.Services.Area.Faculty;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Faculty;
using Ashirvad.ServiceAPI.Services.Area.Faculty;

namespace Ashirvad.Web.Controllers
{
    public class FacultyController : ApiController
    {
        private readonly IFacultyService _facultyService;
        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        public FacultyController()
        {
            _facultyService = new FacultyService(new Faculty());
        }

        [Route("FacultyMaintenance/{facultyID}/{StaffID}/{Subject_dtl_id}/{course_dtl_id}/{Class_dtl_id}/{BranchID}/{Descripation}/{CreateId}/{CreateBy}/{TransactionId}/{FileName}/{Extension}/{HasFile}")]
        [HttpPost]
        public async Task<OperationResult<FacultyEntity>> FacultyMaintenance(long facultyID, long StaffID, long Subject_dtl_id, long course_dtl_id, long Class_dtl_id, long BranchID, string Descripation, long CreateId, string CreateBy, long TransactionId, string FileName, string Extension, bool HasFile)
        {
            var httpRequest = HttpContext.Current.Request;
            string[] filename = FileName.Split(',');
            string FilePath = "";
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
                            // for live server
                            string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                            // for local server
                            //string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/FacultyImage/" + randomfilename + extension;
                            string _Filepath1 = "FacultyImage/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/FacultyImage/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            FileName = fileName;
                            FilePath = _Filepath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            else
            {
                FileName = filename[0];
                FilePath = "/FeesImage/" + filename[1] + "." + Extension;
            }
            var facultyEntity = new FacultyEntity()
            {
                FacultyContentFileName = FileName,
                FilePath = FilePath,
                FacultyID = facultyID,
                BranchInfo = new BranchEntity()
                {
                    BranchID = BranchID
                },
                staff = new StaffEntity()
                {
                    StaffID = StaffID
                },
                branchSubject = new BranchSubjectEntity()
                {
                    Subject_dtl_id = Subject_dtl_id
                },
                BranchCourse = new BranchCourseEntity()
                {
                    course_dtl_id = course_dtl_id
                },
                BranchClass = new BranchClassEntity()
                {
                    Class_dtl_id = Class_dtl_id
                },
                Descripation = Descripation,
                RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active
                },
                Transaction = new TransactionEntity()
                {
                    TransactionId = TransactionId,
                    LastUpdateBy = CreateBy,
                    LastUpdateId = CreateId,
                    CreatedBy = CreateBy,
                    CreatedId = CreateId,
                }
            };
            var data = await _facultyService.FacultyMaintenance(facultyEntity);
            OperationResult<FacultyEntity> response = new OperationResult<FacultyEntity>();
            response.Completed = true;
            response.Data = data;
            return response;
        }

        [Route("GetFacultyByBranchID")]
        [HttpGet]
        public OperationResult<List<FacultyEntity>> GetFacultyByBranchID(long BranchID)
        {
            var data = _facultyService.GetAllFaculty(BranchID);
            OperationResult<List<FacultyEntity>> result = new OperationResult<List<FacultyEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("RemoveFaculty")]
        [HttpPost]
        public OperationResult<bool> RemoveFaculty(long facultyID, string lastupdatedby)
        {
            var result = _facultyService.RemoveFaculty(facultyID, lastupdatedby);
            OperationResult<bool> response = new OperationResult<bool>();
            response.Completed = true;
            response.Data = result;
            return response;
        }
    }
}