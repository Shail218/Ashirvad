using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.Services.Area.UPI;
using Ashirvad.Repo.Services.Area.User;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.UPI;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IBranchRightsService _BranchRightService;
        public ResponseModel res = new ResponseModel();
        public UserController(IUserService userService, IBranchRightsService branchRightsService)
        {
            this._userService = userService;
            this._BranchRightService = branchRightsService;
        }

        [Route("ValidateUser")]
        [HttpGet]
        public OperationResult<UserEntity> ValidateUser(string userName, string password,string fcmtoken)
        {
            var data = this._userService.ValidateUser(userName, password);            
            OperationResult<UserEntity> result = new OperationResult<UserEntity>();
            if (data.Result == null)
            {
                result.Completed = false;
                result.Data = null;
                result.Message = "Invalid Username Or Password !!";

            }
            else
            {
                var da = this._userService.UpdatefcmToken(data.Result, fcmtoken);
                var isAggrement = this._userService.CheckAgreement(data.Result.BranchInfo.BranchID);
                if (isAggrement.Result)
                {
                    result.Completed = true;
                    result.Data = data.Result;
                    result.Message = "Login Successfully!!";
                }
                else
                {
                    result.Completed = false;
                    result.Data = data.Result;
                    result.Message = "Your agreement was expired!!!";
                }

            }
            return result;
        }

        [Route("GetUserPermission")]
        [HttpGet]
        public OperationResult<UserEntity> GetUserPermission(long BranchID)
        {
            var data = this._BranchRightService.GetBranchRightsByBranchID(BranchID);
            OperationResult<UserEntity> result = new OperationResult<UserEntity>();
            if (data.Result.Count > 0)
            {
                result.Data = new UserEntity();
                result.Completed = true;
                result.Data.Permission = data.Result;
                result.Message = "Success";
            }
            else
            {
                result.Data = new UserEntity();
                result.Completed = false;
                result.Data.Permission = data.Result;
                result.Message = "You have no permission!!!";
            }
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
            var data = this._userService.RemoveUser(userID, lastupdatedby);
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


        [Route("ValidateStudent")]
        [HttpGet]
        public OperationResult<UserEntity> ValidateStudent(string userName, string password,string fcmtoken)
        {
            var data = this._userService.ValidateStudent(userName, password);
            OperationResult<UserEntity> result = new OperationResult<UserEntity>();
            if (data.Result.UserID == 0)
            {
                result.Completed = false;
                result.Data = null;
                result.Message = "Invalid Username Or Password !!";

            }
            else
            {
                var da = this._userService.UpdatefcmToken(data.Result, fcmtoken);
                var isAggrement = this._userService.CheckAgreement(data.Result.BranchInfo.BranchID);
                if (isAggrement.Result)
                {
                    result.Completed = true;
                    result.Data = data.Result;
                    result.Message = "Login Successfully!!";
                }
                else
                {
                    result.Completed = false;
                    result.Data = data.Result;
                    result.Message = "Your agreement has expired!!!";
                }
            }
            return result;
        }

        [Route("GetAllUsersddl")]
        [HttpGet]
        public OperationResult<List<UserEntity>> GetAllUsersddl(long branchID)
        {
            var data = this._userService.GetAllUsersddl(branchID);
            OperationResult<List<UserEntity>> result = new OperationResult<List<UserEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("CheckUserName")]
        [HttpPost]
        public OperationResult<ResponseModel> CheckUserName(UserEntity userinfo)
        {
            User model = new User();
            ResponseModel entity = new ResponseModel();
            var data = model.Check_UserName(userinfo.Username).Result;
            if (data != null)
            {
                string contactNo = data.Username;
                string message = "Dear%20" + "User" + "%20your%20Password%20is:%20" + data.Password + "%20Thank%20you" + "%20MSMIND";
                //string message = "testing msg oasissoftwares";
                string requestUrl = string.Format("http://sms.oasissoftwares.online/sms-panel/api/http/index.php?username=MSMlND&apikey=7F7A1-06464&apirequest=Text&sender=MSMlND&mobile=" + contactNo + "&message=" + message + "&route=TRANS&TemplateID=1507164378545227889&format=JSON");
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                var dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                const string accessToken = "status\":\"";
                int clientIndex = responseFromServer.IndexOf(accessToken, StringComparison.Ordinal);

                int accessTokenIndex = clientIndex + accessToken.Length;
                string access_token = responseFromServer.Substring(accessTokenIndex, (responseFromServer.Length - accessTokenIndex - 2));
                int clientIndex1 = access_token.IndexOf("\",\"", StringComparison.Ordinal);
                string access_token2 = access_token.Substring(0, clientIndex1);
                if (access_token2 == "success")
                {
                    entity.Status = true;
                    entity.Message = "SMS Send to Your Register Mobile Number.";
                }
                else
                {
                    entity.Status = false;
                    entity.Message = "Please try again!!!";
                }
            }
            else
            {
                entity.Status = false;
                entity.Message = "Please Enter Register Mobile Number!!";
            }
            OperationResult<ResponseModel> result = new OperationResult<ResponseModel>();
            result.Completed = true;
            result.Data = entity;
            return result;
        }

        [Route("GetAllUPIs")]
        [HttpGet]
        public OperationResult<List<UPIEntity>> GetAllUPIs(long branchid)
        {
            UPI upi = new UPI();
            var data = upi.GetAllUPIs(branchid);
            OperationResult<List<UPIEntity>> result = new OperationResult<List<UPIEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("UPIMaintenance")]
        [HttpPost]
        public OperationResult<long> UPIMaintenance(UPIEntity uPIEntity)
        {
            UPI upi = new UPI();
            var data = upi.UPIMaintenance(uPIEntity).Result;
            OperationResult<long> result = new OperationResult<long>();
            result.Completed = false;
            result.Data = 0;
            if(data > 0)
            {
                result.Completed = true;
                result.Data = data;
            }
            return result;
        }

        [Route("RemoveUPI")]
        [HttpPost]
        public OperationResult<bool> RemoveUPI(long UPIID, string lastupdatedby)
        {
            UPI upi = new UPI();
            var data = upi.RemoveUPI(UPIID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("UpdateFcMToken")]
        [HttpPost]
        public OperationResult<bool> UpdateFcMToken(long userID,string fcmtoken)
        {
            UserEntity user = new UserEntity();
            user.UserID = userID;
            var da = this._userService.UpdatefcmToken(user, fcmtoken);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = da.Result;
            return result;
        }
    }
}
