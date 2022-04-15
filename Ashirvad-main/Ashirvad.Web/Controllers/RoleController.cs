using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _RoleService;
        public ResponseModel res = new ResponseModel();

        public RoleController(IRoleService RoleService)
        {
            _RoleService = RoleService;
        }

        // GET: Role
        public ActionResult Index()
        {
            RoleMaintenanceModel branch = new RoleMaintenanceModel();
            return View(branch);
        }
        public async Task<ActionResult> RoleMaintenance(long branchID)
        {
            long RoleID = branchID;
            RoleMaintenanceModel branch = new RoleMaintenanceModel();
            branch.RoleInfo = new RoleEntity();
            if (RoleID > 0)
            {
                var result = await _RoleService.GetRoleByID(RoleID);
                branch.RoleInfo = result;
            }

            //var branchData = await _RoleService.GetAllRoles(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.RoleData = new List<RoleEntity>();

            return View("Index", branch);
        }

        public async Task<ActionResult> EditRole(long RoleID, long branchID)
        {
            RoleMaintenanceModel branch = new RoleMaintenanceModel();
            if (RoleID > 0)
            {
                var result = await _RoleService.GetRoleByID(RoleID);
                branch.RoleInfo = result;
            }

            if (branchID > 0)
            {
                var result = await _RoleService.GetAllRoles(branchID);
                branch.RoleData = result;
            }

            var branchData = await _RoleService.GetAllRoles();
            branch.RoleData = branchData;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveRole(RoleEntity branch)
        {
            branch.Transaction = GetTransactionData(branch.RoleID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            branch.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            branch.BranchInfo = new BranchEntity();
            branch.BranchInfo = SessionContext.Instance.LoginUser.BranchInfo;
            res = await _RoleService.RoleMaintenance(branch);
            return Json(res);
        }

        [HttpPost]
        public JsonResult RemoveRole(long RoleID)
        {
            var result = _RoleService.RemoveRole(RoleID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> RoleData()
        {
            var branchData = await _RoleService.GetAllRoles();
            return Json(branchData);
        }

        public async Task<JsonResult> RoleDataByBranch()
        {
            var branchData = await _RoleService.GetAllRoles(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            return Json(branchData);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            var branchData = await _RoleService.GetAllCustomRole(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData.Count;
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