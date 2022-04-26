using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Page;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using Ashirvad.ServiceAPI.ServiceAPI.Area.UserRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/UserRights/v1")]
    [AshirvadAuthorization]
    public class UserRightsController : ApiController
    { // GET: UserRights
        private readonly IUserRightsService _UserRightService;
        private readonly IPageService _pageService;
        private readonly IUserService _userService;

        public UserRightsController(IUserRightsService UserRightService, IPageService pageService, IUserService userService)
        {
            _UserRightService = UserRightService;
            _pageService = pageService;
            _userService = userService;
        }

        [Route("UserRightsMaintenance")]
        [HttpPost]
        public OperationResult<UserWiseRightsEntity> UserRightsMaintenance(UserWiseRightsEntity userInfo)
        {
            var data = this._UserRightService.UserRightsMaintenance(userInfo);
            OperationResult<UserWiseRightsEntity> result = new OperationResult<UserWiseRightsEntity>();
            result.Completed = data.Result.Status;
            if (data.Result.Status && data.Result.Data != null)
            {
                result.Data = (UserWiseRightsEntity)data.Result.Data;
            }
            result.Message = data.Result.Message;
            return result;
        }

        [Route("GetAllUserRightsbyBranchId")]
        [HttpGet]
        public OperationResult<List<UserWiseRightsEntity>> GetAllUserRightsbyBranchId(long branchId)
        {
            var data = this._UserRightService.GetAllUserRightsbyBranchId(branchId);
            OperationResult<List<UserWiseRightsEntity>> result = new OperationResult<List<UserWiseRightsEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetUserRightsByUserID")]
        [HttpGet]
        public OperationResult<List<UserWiseRightsEntity>> GetUserRightsByUserID(long userId)
        {
            var data = this._UserRightService.GetUserRightsByUserID(userId);
            OperationResult<List<UserWiseRightsEntity>> result = new OperationResult<List<UserWiseRightsEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("RemoveUserRights")]
        [HttpPost]
        public OperationResult<bool> RemoveUserRights(long userRightsId, string lastupdatedby)
        {
            var data = this._UserRightService.RemoveUserRights(userRightsId, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }

        [Route("UserRightUniqueData")]
        [HttpGet]
        public OperationResult<List<UserWiseRightsEntity>> UserRightUniqueData(long roleid)
        {
            var data = this._UserRightService.GetAllUserRightsUniqData(roleid);
            OperationResult<List<UserWiseRightsEntity>> result = new OperationResult<List<UserWiseRightsEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllStaffUserbyBranch")]
        [HttpGet]
        public OperationResult<List<UserEntity>> GetAllStaffUserbyBranch(long branchid)
        {
            var data = this._userService.GetAllStaffUserbyBranch(branchid);
            OperationResult<List<UserEntity>> result = new OperationResult<List<UserEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

    }
}
