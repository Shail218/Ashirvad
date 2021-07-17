
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class UserPermissionController : BaseController
    {
        private readonly IUserService _userService = null;
        public UserPermissionController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: UserPermission
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserPermissionMaintenance()
        {
            UserPermissionMaintenanceModel branch = new UserPermissionMaintenanceModel();
            branch.RolesData = Common.Common.GetRoles();
            return View("Index", branch);
        }

        public JsonResult GetAllUsers(long branchID)
        {
            var branchData = _userService.GetAllUsers(branchID);
            return Json(branchData);
        }

        [HttpPost]
        public JsonResult UserRoleManagement(UserEntity user)
        {
            return Json(_userService.AddUserRoles(user));
        }
    }
}