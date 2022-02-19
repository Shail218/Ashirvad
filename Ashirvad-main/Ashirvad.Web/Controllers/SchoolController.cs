using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.School;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class SchoolController : BaseController
    {
        private readonly ISchoolService _schoolService;
        public ResponseModel res = new ResponseModel();
        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        // GET: School
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SchoolMaintenance(long branchID)
        {
            long schoolID = branchID;
            SchoolMaintainanceModel branch = new SchoolMaintainanceModel();
            if (schoolID > 0)
            {
                var result = await _schoolService.GetSchoolsByID(schoolID);
                branch.SchoolInfo = result;
            }
            branch.SchoolData = new List<SchoolEntity>();

            return View("Index", branch);
        }

        public async Task<ActionResult> EditSchool(long schoolID, long branchID)
        {
            SchoolMaintainanceModel branch = new SchoolMaintainanceModel();
            if (schoolID > 0)
            {
                var result = await _schoolService.GetSchoolsByID(schoolID);
                branch.SchoolInfo = result;
            }

            if (branchID > 0)
            {
                var result = await _schoolService.GetAllSchools(branchID);
                branch.SchoolData = result;
            }

            var branchData = await _schoolService.GetAllSchools();
            branch.SchoolData = branchData;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveSchool(SchoolEntity branch)
        {
            branch.Transaction = GetTransactionData(branch.SchoolID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            branch.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _schoolService.SchoolMaintenance(branch);
            res.Status = data.SchoolID > 0 ? true : false;
            res.Message = data.SchoolID == -1 ? "School Already exists!!" : data.SchoolID == 0 ? "School failed to insert!!":"School Inserted Successfully!!";
            return Json(res);
        }

        [HttpPost]
        public JsonResult RemoveSchool(long branchID)
        {
            var result = _schoolService.RemoveSchool(branchID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> SchoolData(long branchID)
        {
            var branchData = await _schoolService.GetAllSchools(branchID);
            return Json(branchData);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("SchoolName");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _schoolService.GetAllCustomSchools(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData[0].Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData
            });

        }

        [HttpPost]
        public async Task<ActionResult> GetExportData(string Search)       
        {
            
            var branchData = await _schoolService.GetAllExportSchools(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
           
            return View("~/Views/School/_Export_School.cshtml", branchData);

        }

    }
}