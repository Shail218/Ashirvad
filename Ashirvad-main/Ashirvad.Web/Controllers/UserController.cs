using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IStaffService _staffService;
        public ResponseModel res = new ResponseModel();
        public UserController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> UserMaintenance(long branchID)
        {
            long staffID = branchID;

            UserMaintenanceModel userData = new UserMaintenanceModel();

            if (staffID > 0)
            {
                var result = await _staffService.GetStaffByID(staffID);
                userData.StaffInfo = result;
            }

            //var staffData = await _staffService.GetAllStaff(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            userData.StaffData = new List<StaffEntity>();

            return View("Index", userData);
        }

        public async Task<ActionResult> EditUser(long subjectID, long branchID)
        {
            UserMaintenanceModel branch = new UserMaintenanceModel();
            if (subjectID > 0)
            {
                var result = await _staffService.GetStaffByID(subjectID);
                branch.StaffInfo = result;
            }

            if (branchID > 0)
            {
                var result = await _staffService.GetAllStaff(branchID);
                branch.StaffData = result;
            }

            var branchData = await _staffService.GetAllStaff();
            branch.StaffData = branchData;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveUser(StaffEntity branch)
        {
            branch.Transaction = GetTransactionData(branch.StaffID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            branch.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            if(SessionContext.Instance.LoginUser.UserType == Ashirvad.Common.Enums.UserType.Admin)
            {
                branch.Userrole = Ashirvad.Common.Enums.UserType.Staff;
                branch.BranchInfo = new BranchEntity();
                branch.BranchInfo.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;

            }
            var data = await _staffService.StaffMaintenance(branch);
            res.Status = data.StaffID > 0 ? true : false;
            res.Message = data.StaffID == -1 ? "User Already exists!!" : data.StaffID == 0 ? "User failed to insert!!" : "User Inserted Successfully!!";
            return Json(res);
        }

        [HttpPost]
        public JsonResult RemoveUser(long userID)
        {
            var result = _staffService.RemoveStaff(userID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("Name");
            columns.Add("MobileNo");
            columns.Add("EmailID");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _staffService.GetAllCustomStaff(model, SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
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

    }
}