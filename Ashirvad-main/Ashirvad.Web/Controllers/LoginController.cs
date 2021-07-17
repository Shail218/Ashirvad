using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
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
        public LoginController(IUserService userService)
        {
            _userService = userService;
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
                SessionContext.Instance.LoginUser = userInfo;
            }
            return Json(success);
            //return Json(userInfo);
        }
    }
}