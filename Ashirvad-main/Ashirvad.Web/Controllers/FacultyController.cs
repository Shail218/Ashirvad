using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Faculty;
using Newtonsoft.Json;

namespace Ashirvad.Web.Controllers
{
    public class FacultyController : BaseController
    {
        private readonly IFacultyService _facultyService;
        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }
        // GET: Faculty
        public ActionResult Index()
        {
           
            return View();
        }

        public async Task<ActionResult> FacultyMaintenance(long facultyID)
        {
            FacultyMaintenanceModel faculty = new FacultyMaintenanceModel();
            if (facultyID > 0)
            {
                var result = await _facultyService.GetFacultyByFacultyID(facultyID);
                faculty.FacultyInfo = result.Data;
            }
            else
            {
                faculty.FacultyInfo = new FacultyEntity();
            }

            var branchData = await _facultyService.GetAllFaculty(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            faculty.FacultyData = branchData;

            return View("Index", faculty);
        }

        [HttpPost]
        public async Task<JsonResult> SaveFaculty(FacultyEntity facultyEntity)
        {
            if (facultyEntity.FileInfo != null)
            {
                string _FileName = Path.GetFileName(facultyEntity.FileInfo.FileName);
                string extension = System.IO.Path.GetExtension(facultyEntity.FileInfo.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/FacultyImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/FacultyImage"), randomfilename + extension);
                facultyEntity.FileInfo.SaveAs(_path);
                facultyEntity.FacultyContentFileName = _FileName;
                facultyEntity.FilePath = _Filepath;
            }

            facultyEntity.Transaction = GetTransactionData(facultyEntity.FacultyID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            facultyEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _facultyService.FacultyMaintenance(facultyEntity);
            if (data != null)
            {
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveFaculty(long facultyID)
        {
            var result = _facultyService.RemoveFaculty(facultyID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

    }
}