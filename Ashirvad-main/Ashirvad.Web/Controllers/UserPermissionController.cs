
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Enums;

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
            RolesEntity rl = new RolesEntity();
            var roles = JsonConvert.DeserializeObject<List<RolesEntity>>(user.JSONData);
            foreach(var item in roles)
            {
                Roles prm;
                if(Enum.TryParse(item.Permission.ToString(),out prm))
                {
                    item.PermissionValue = (int)prm;
                }
            }
            user.Transaction = GetTransactionData(user.UserID > 0 ? Common.Enums.TransactionType.Insert : Common.Enums.TransactionType.Update);
            user.Roles = roles;
            var data = _userService.AddUserRoles(user);
            if (data)
            {
                return Json(data);
            }
            else
            {
                return Json(false);
            }
        }
    }
}