using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/user/v1")]
    [AshirvadAuthorization]
    public class UserController : ApiController
    {
        private readonly IUserService _userService = null;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [Route("ValidateUser")]
        [HttpGet]
        public OperationResult<UserEntity> ValidateUser(string userName, string password)
        {
            var data = this._userService.ValidateUser(userName, password);
            OperationResult<UserEntity> result = new OperationResult<UserEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("UserMaintenance")]
        [HttpPost]
        public OperationResult<UserEntity> UserMaintenance(UserEntity userinfo)
        {
            var data = this._userService.UserMaintenance(userinfo);
            OperationResult<UserEntity> result = new OperationResult<UserEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("RemoveUser")]
        [HttpPost]
        public OperationResult<bool> RemoveUser(long userID, string lastupdatedby)
        {
            var data = this._userService.RemoveUser(userID,lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllUsers")]
        [HttpPost]
        public OperationResult<List<UserEntity>> GetAllUsers(long branchID)
        {
            var data = this._userService.GetAllUsers(branchID);
            OperationResult<List<UserEntity>> result = new OperationResult<List<UserEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllUsersByBranchAndType")]
        [HttpPost]
        public OperationResult<List<UserEntity>> GetAllUsers(long branchID, List<int> userType)
        {
            var data = this._userService.GetAllUsers(branchID, userType);
            OperationResult<List<UserEntity>> result = new OperationResult<List<UserEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllUsersByUsernameAndContact")]
        [HttpPost]
        public OperationResult<List<UserEntity>> GetAllUsers(string userName, string contactNo)
        {
            var data = this._userService.GetAllUsers(userName, contactNo);
            OperationResult<List<UserEntity>> result = new OperationResult<List<UserEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetUserByID")]
        [HttpPost]
        public OperationResult<UserEntity> GetAllUserByID(long userID)
        {
            var data = this._userService.GetUserByID(userID);
            OperationResult<UserEntity> result = new OperationResult<UserEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("ChangePassword")]
        [HttpPost]
        public OperationResult<bool> ChangePassword(long userID, string password, string oldPassword)
        {
            var data = this._userService.ChangePassword(userID, password, oldPassword);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetUserRoleList")]
        [HttpPost]
        public OperationResult<Dictionary<string, int>> GetUserRoleList()
        {
            OperationResult<Dictionary<string, int>> result = new OperationResult<Dictionary<string, int>>();
            result.Completed = true;
            result.Data = Common.Common.GetRoles();
            return result;
        }

        [Route("GetUserRole")]
        [HttpPost]
        public OperationResult<List<RolesEntity>> GetUserRoleListByUserID(long userID)
        {
            OperationResult<List<RolesEntity>> result = new OperationResult<List<RolesEntity>>();
            var data = _userService.GetRolesByUser(userID);
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("UserRoleManagement")]
        [HttpPost]
        public OperationResult<bool> UserRoleManagement(UserEntity user)
        {
            OperationResult<bool> result = new OperationResult<bool>();
            var data = _userService.AddUserRoles(user);
            result.Completed = true;
            result.Data = data;
            return result;
        }
    }
}
