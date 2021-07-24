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

            var staffData = await _staffService.GetAllStaff(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            userData.StaffData = staffData;

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

    }
}