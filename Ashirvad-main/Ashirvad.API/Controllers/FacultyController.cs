using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.Services.Area.Faculty;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Faculty;
using Ashirvad.ServiceAPI.Services.Area.Faculty;
using Newtonsoft.Json;

namespace Ashirvad.Web.Controllers
{
    [RoutePrefix("api/faculty/v1")]
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

        [Route("FacultyMaintenance")]
        [HttpPost]
        public async Task<OperationResult<FacultyEntity>> FacultyMaintenance(string model, bool HasFile)
        {
            var httpRequest = HttpContext.Current.Request;
            OperationResult<FacultyEntity> response = new OperationResult<FacultyEntity>();
            try
            {
                FacultyEntity entity = new FacultyEntity();
                var facultyentity = JsonConvert.DeserializeObject<FacultyEntity>(model);
                entity.BranchInfo = new BranchEntity()
                {
                    BranchID = facultyentity.BranchInfo.BranchID
                };
                entity.staff = new StaffEntity()
                {
                    StaffID = facultyentity.staff.StaffID
                };
                entity.branchSubject = new BranchSubjectEntity()
                {
                    Subject_dtl_id = facultyentity.branchSubject.Subject_dtl_id
                };
                entity.BranchCourse = new BranchCourseEntity()
                {
                    course_dtl_id = facultyentity.BranchCourse.course_dtl_id
                };
                entity.BranchClass = new BranchClassEntity()
                {
                    Class_dtl_id = facultyentity.BranchClass.Class_dtl_id
                };
                entity.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active
                };
                entity.Descripation = facultyentity.Descripation;
                entity.FacultyID = facultyentity.FacultyID;
                entity.Transaction = new TransactionEntity()
                {
                    TransactionId = facultyentity.Transaction.TransactionId,
                    LastUpdateBy = facultyentity.Transaction.LastUpdateBy,
                    LastUpdateId = facultyentity.Transaction.LastUpdateId,
                    CreatedBy = facultyentity.Transaction.CreatedBy,
                    CreatedId = facultyentity.Transaction.CreatedId
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
                                string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                                var postedFile = httpRequest.Files[file];
                                string randomfilename = Common.Common.RandomString(20);
                                extension = Path.GetExtension(postedFile.FileName);
                                fileName = Path.GetFileName(postedFile.FileName);
                                string _Filepath = "/FacultyImage/" + randomfilename + extension;
                                string _Filepath1 = "FacultyImage/" + randomfilename + extension;
                                var filePath = HttpContext.Current.Server.MapPath("~/FacultyImage/" + randomfilename + extension);
                                string _path = UpdatedPath + _Filepath1;
                                postedFile.SaveAs(_path);
                                entity.FacultyContentFileName = fileName;
                                entity.FilePath = _Filepath;
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
                    entity.FacultyContentFileName = facultyentity.FacultyContentFileName;
                    entity.FilePath = facultyentity.FilePath;
                }
                var data = await _facultyService.FacultyMaintenance(entity);
                response.Completed = false;
                response.Data = null;
                if (data.FacultyID > 0)
                {
                    response.Completed = true;
                    response.Data = data;
                    if (entity.FacultyID > 0)
                    {
                        response.Message = "Faculty Updated Successfully.";
                    }
                    else
                    {
                        response.Message = "Faculty Created Successfully.";
                    }
                }
                else
                {
                    response.Message = "Faculty Already Exists!!";
                }
            }
            catch(Exception e)
            {
                response.Completed = false;
                response.Message = e.ToString();
            }
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

        public static string Decode(string Path)
        {
            byte[] mybyte = Convert.FromBase64String(Path);
            string returntext = Encoding.UTF8.GetString(mybyte);
            return returntext;
        }
    }
}