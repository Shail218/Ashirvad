using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Staff;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IStaffService _staffService;
        private readonly IUserService _userService;
        public ResponseModel res = new ResponseModel();
        public ProfileController(IStaffService staffService, IUserService userService)
        {
            _staffService = staffService;
            _userService = userService;
        }

        // GET: Profile
        public async Task<ActionResult> Index()
        {
            if (SessionContext.Instance.LoginUser.UserType.Equals(Enums.UserType.Admin))
            {
                var result = await _staffService.GetStaffByID((long)SessionContext.Instance.LoginUser.StaffID);
                return View(result);
            }
            else
            {
                StaffEntity staff = new StaffEntity()
                {
                    Userrole = Enums.UserType.SuperAdmin,
                    Transaction = SessionContext.Instance.LoginUser.Transaction,
                    StaffID = 0,
                    UserID = SessionContext.Instance.LoginUser.UserID,
                    MobileNo = SessionContext.Instance.LoginUser.Username
                };
                return View(staff);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveUser(StaffEntity branch)
        {
            if (SessionContext.Instance.LoginUser.UserType.Equals(Enums.UserType.Admin))
            {
                branch.Transaction = GetTransactionData(branch.StaffID > 0 ? Enums.TransactionType.Update : Enums.TransactionType.Insert);
                branch.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active
                };
                branch.BranchInfo = new BranchEntity();
                branch.BranchInfo.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
                var data = await _staffService.UpdateProfile(branch);
                res.Status = data.StaffID > 0 ? true : false;
                if (branch.MobileNo == SessionContext.Instance.LoginUser.Username)
                {
                    res.Message = data.StaffID == 0 ? "Profile failed to Update!!" : "Profile Update Successfully!!";
                    res.IsEdit = false;
                }
                else
                {
                    res.Message = data.StaffID == 0 ? "Profile failed to Update!!" : "Your Mobile Number has Changed!! Please Login Again!!";
                    res.IsEdit = true;
                }
                return Json(res);
            }
            else
            {
                branch.Transaction = GetTransactionData(branch.StaffID > 0 ? Enums.TransactionType.Update : Enums.TransactionType.Insert);
                var data = await _userService.ProfileMaintenance(new UserEntity()
                {
                    Transaction = branch.Transaction,
                    Username = branch.MobileNo,
                    UserID = branch.UserID
                });
                res.Status = data > 0 ? true : false;
                if (branch.MobileNo == SessionContext.Instance.LoginUser.Username)
                {
                    res.Message = data == 0 ? "Profile failed to Update!!" : "Profile Update Successfully!!";
                    res.IsEdit = false;
                }
                else
                {
                    res.Message = data == 0 ? "Profile failed to Update!!" : "Your Mobile Number has Changed!! Please Login Again!!";
                    res.IsEdit = true;
                }
                return Json(res);
            }
        }
    }
}