using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService = null;
        private readonly IBranchRightsService _BranchRightService;
        ResponseModel response = new ResponseModel();
        public LoginController(IUserService userService, IBranchRightsService branchRightsService)
        {
            _userService = userService;
            _BranchRightService = branchRightsService;
        }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ValidateUser(UserEntity user)
        {
            bool success = false;
            var userInfo = await _userService.ValidateUser(user.Username, user.Password);
            if(userInfo != null)
            {
                success = true;
                if(userInfo.UserType== Enums.UserType.SuperAdmin)
                {
                    List<BranchWiseRightEntity> branchWises = new List<BranchWiseRightEntity>();
                    SessionContext.Instance.userRightsList= JsonConvert.SerializeObject(branchWises); 
                }
                else
                {
                    var Get = await GetBranchRights(userInfo.BranchInfo.BranchID);
                }
               
                if (SessionContext.Instance.userRightsList != null)
                {
                    response.Message = "Login Successfully!!";
                    response.Status = true;
                    SessionContext.Instance.LoginUser = userInfo;

                }
                else
                {
                    SessionContext.Instance.LoginUser = null;
                    response.Message = "You have no permission of any module!!";
                    response.Status = false;
                }
                
            }
            else
            {
                response.Message = "Invalid username and password!!";
                response.Status = false;
            }
            return Json(response);
            //return Json(userInfo);
        }
        public async Task<string> GetBranchRights(long PackageRightID)
        {
            var BranchRightData = await _BranchRightService.GetBranchRightsByBranchID(PackageRightID);
            if (BranchRightData.Count > 0)
            {
                SessionContext.Instance.userRightsList = JsonConvert.SerializeObject(BranchRightData);
            }
            else
            {
                SessionContext.Instance.userRightsList = null;
            }
            return SessionContext.Instance.userRightsList;

        }
    }
}