using Ashirvad.Repo.DataAcceessAPI.Area.User;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly IUserService _userServices;

        public ChangePasswordController(IUserService staffService)
        {
            _userServices = staffService;
        }
        // GET: ChangePassword
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult changepassword(string password, string oldPassword)
        {
            var result = _userServices.ChangePassword(SessionContext.Instance.LoginUser.UserID, password,oldPassword).Result;
            return Json(result);
        }
    }
}