using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class ManageStudentController : Controller
    {
        private readonly IStudentService _studentService;
        public ManageStudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: ManageStudent
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ManageStudentMaintenance(long branchID)
        {
            StudentMaintenanceModel branch = new StudentMaintenanceModel();
            //if (branchID > 0)
            //{
            //    var result = await _studentService.GetAllStudentWithoutContent(branchID);
            //    branch.StudentData = result;
            //}
            branch.StudentData = new List<StudentEntity>();
            return View("Index", branch);
        }

        [HttpPost]
        public async Task<ActionResult> GetAllActiveStudent(long branchID)
        {
            var data = await this._studentService.GetAllStudent(branchID, (int)Enums.RowStatus.Active);
            return View("~/Views/ManageStudent/Manage.cshtml", data);
        }

        [HttpPost]
        public async Task<ActionResult> GetAllInActiveStudent(long branchID)
        {
            var data = await this._studentService.GetAllStudent(branchID, (int)Enums.RowStatus.Inactive);
            return View("~/Views/ManageStudent/Manage.cshtml", data);
        }

        [HttpPost]
        public JsonResult Removestudent(long studentID)
        {
            var result = _studentService.RemoveStudent(studentID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }
      
        public async Task<JsonResult> GetStudentByStd(long Std,long BatchTime)
        {
            var result = _studentService.GetStudentByStd(Std, SessionContext.Instance.LoginUser.BranchInfo.BranchID, BatchTime).Result;
            return Json(result);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model,string status)
        {
            // action inside a standard controller

            int statusid = 0;
            List<string> columns = new List<string>();
            columns.Add("");
            columns.Add("Name");
            columns.Add("AdmissionDate");
            columns.Add("");
            columns.Add("");
            columns.Add("ContactNo");
            if (model.order != null)
            {
                foreach (var item in model.order)
                {
                    item.name = columns[item.column];
                }
            }
            model.length = model.length == 0 ? 10 : model.length;
            model.draw = model.draw == 0 ? 1 : model.draw;
            
            if (!string.IsNullOrEmpty(status))   
            {
                statusid = Convert.ToInt32(status);
            }
            var branchData = await _studentService.GetAllCustomStudent(model,SessionContext.Instance.LoginUser.BranchInfo.BranchID, statusid);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData[0].Count;
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData
            });

        }

        [HttpPost]
        public async Task<ActionResult> GetExportData(string Search)
        {
            var branchData = await _studentService.GetAllStudent(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            return View("~/Views/ManageStudent/_Export_Student.cshtml", branchData);

        }
    }
}