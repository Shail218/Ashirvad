using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Page;
using Ashirvad.ServiceAPI.ServiceAPI.Area.UserRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class UserRightsController : BaseController
    {
        // GET: UserRights
        private readonly IUserRightsService _UserRightService;
        private readonly IPageService _pageService;

        public UserRightsController(IUserRightsService UserRightService, IPageService pageService)
        {

            _UserRightService = UserRightService;
            _pageService = pageService;
        }

        // GET: UserRight
        public ActionResult Index()
        {

            return View();
        }

        public async Task<ActionResult> UserRightMaintenance(long UserRightID)
        {
            UserWiseRightMaintenanceModel UserRight = new UserWiseRightMaintenanceModel();
            UserRight.UserWiseRightInfo = new UserWiseRightsEntity();
            //var UserRightData = await _UserRightService.GetAllUserRightss();
            UserRight.UserWiseRightData = new List<UserWiseRightsEntity>();

            if (UserRightID > 0)
            {
                var result = await _UserRightService.GetUserRightsByID(UserRightID);
                var UserRightData1 = await _UserRightService.GetAllUserRightsUniqData(result.Roleinfo.RoleID);
                UserRight.UserWiseRightInfo = result;
                UserRight.UserWiseRightInfo.list = UserRightData1;

            }

            var UserData = await _pageService.GetAllPages(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            UserRight.UserWiseRightInfo.PageList = UserData;
            return View("Index", UserRight);
        }

        [HttpPost]
        public async Task<JsonResult> SaveUserRight(UserWiseRightsEntity UserRight)
        {

            UserWiseRightsEntity UserRightEntity = new UserWiseRightsEntity();
            ResponseModel responseModel = new ResponseModel();
            UserRight.Transaction = GetTransactionData(UserRight.UserWiseRightsID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            UserRight.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            if (UserRight.UserID == 0)
            {
                UserRight.UserID = UserRight.userinfo.UserID;
            }
            responseModel = await _UserRightService.UserRightsMaintenance(UserRight);


            return Json(responseModel);
        }

        [HttpPost]
        public JsonResult RemoveUserRight(long UserRightID)
        {
            var result = _UserRightService.RemoveUserRights(UserRightID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> UserRightData()
        {
            var UserRightData = await _UserRightService.GetAllUserRightss();

            return Json(UserRightData);
        }

        public async Task<ActionResult> UserRightUniqueData(long RoleRightID)
        {
            var UserRightData = await _UserRightService.GetAllUserRightsUniqData(RoleRightID);

            return View("~/Views/UserRights/RightsDataTable.cshtml", UserRightData);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            var UserData = await _UserRightService.GetAllCustomRights(model,SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            long total = 0;
            if (UserData.Count > 0)
            {
                total = UserData[0].Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = UserData
            });

        }
    }
}