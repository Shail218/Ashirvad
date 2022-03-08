using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.Services.Area.User;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
            if (userInfo != null)
            {
                userInfo.FinancialYear = user.FinancialYear;
                success = true;
                var Get = await GetBranchRights(userInfo.BranchInfo.BranchID);
                if (userInfo.UserType == Enums.UserType.SuperAdmin)
                {
                    List<BranchWiseRightEntity> branchWises = new List<BranchWiseRightEntity>();
                    SessionContext.Instance.userRightsList= JsonConvert.SerializeObject(branchWises);
                    response.Message = "Login Successfully!!";
                    response.Status = true;
                    response.URL = "Home/Dashboard";
                    
                    SessionContext.Instance.LoginUser = userInfo;
                    //if (SessionContext.Instance.userRightsList != null)
                    //{
                    //    response.Message = "Login Successfully!!";
                    //    response.Status = true;
                    //    response.URL = "Home/Dashboard";
                    //    SessionContext.Instance.LoginUser = userInfo;
                    //}
                    //else
                    //{
                    //    SessionContext.Instance.LoginUser = null;
                    //    response.Message = "You have no permission of any module!!";
                    //    response.Status = false;
                    //}
                }
                else
                {

                    var isAggrement = this._userService.CheckAgreement(userInfo.BranchInfo.BranchID);
                    if (isAggrement.Result)
                    {
                        response.Message = "Login Successfully!!";
                        response.Status = true;
                        response.URL = "Home/ADashboard";

                        SessionContext.Instance.LoginUser = userInfo;
                    }
                    else
                    {
                        response.Message = "Your agreement was expired!!!";
                        response.Status = false;
                        SessionContext.Instance.LoginUser = null;
                    }
                }
            }
            else
            {
                response.Message = "Invalid username and password!!";
                response.Status = false;
            }
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> CheckUserName(UserEntity user)
        {
            User model = new User();
            ResponseModel entity = new ResponseModel();
            smsmodel sms = new smsmodel();
            try
            {
                var data = model.Check_UserName(user.Username).Result;
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
                    entity.Message = "Please Enter Registered Mobile Number!!";
                }

            }
            catch (Exception ex)
            {
                entity.Status = false;
                entity.Message = ex.Message;
            }

            return Json(entity);

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

    public class smsmodel
    {
        public string status { get; set; }
        public string message { get; set; }
    }
}