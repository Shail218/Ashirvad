using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.User;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.User
{
    public class UserService : IUserService
    {
        private readonly IUserAPI _userContext;
        public UserService(IUserAPI userContext)
        {
            this._userContext = userContext;
        }

        public async Task<ResponseModel> ProfileMaintenance(UserEntity userInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            UserEntity user = new UserEntity();
            try
            {
                responseModel = await this._userContext.ProfileMaintenance(userInfo);
                //long userID = await this._userContext.ProfileMaintenance(userInfo);
                //if (userID > 0)
                //{
                //    user.UserID = userID;
                //}
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<ResponseModel> UserMaintenance(UserEntity UserInfo)
        {
             ResponseModel responseModel = new ResponseModel();
            UserEntity user = new UserEntity();
            try
            {
                responseModel = await this._userContext.UserMaintenance(UserInfo);
                //long userID = await this._userContext.UserMaintenance(UserInfo);
                //if (userID > 0)
                //{
                //    //Add User
                //    //Get Branch
                //    user.UserID = userID;
                //}
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<UserEntity> ValidateUser(string userName, string password)
        {
            try
            {
                return await this._userContext.ValidateUser(userName, password);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<bool> CheckAgreement(long branchID)
        {
            try
            {
                return await this._userContext.CheckAgreement(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return false;
        }

        public async Task<UserEntity> ValidateStudent(string userName, string password)
        {
            try
            {
                return await this._userContext.ValidateStudent(userName, password);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public List<UserEntity> GetAllUsers(long branchID, List<int> userType = null)
        {
            try
            {
                if (userType == null)
                {
                    userType = new List<int>();
                }
                return this._userContext.GetAllUsers(branchID, userType);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public List<UserEntity> GetAllUsersddl(long branchID)
        {
            try
            {
                
                return this._userContext.GetAllUsersddl(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public List<UserEntity> GetAllUsers(string userName, string contactNo)
        {
            try
            {
                return this._userContext.GetAllUsers(userName, contactNo);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool AddUserRoles(UserEntity user)
        {
            try
            {
                return this._userContext.AddUserRoles(user);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public List<RolesEntity> GetRolesByUser(long userID)
        {
            try
            {
                return this._userContext.GetRolesByUser(userID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return new List<RolesEntity>();
        }

        public async Task<UserEntity> GetUserByID(long userID)
        {
            try
            {
                return await this._userContext.GetUserByUserID(userID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return new UserEntity();
        }

        public async Task<ResponseModel> ChangePassword(long userID, string password, string oldPassword)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                return await this._userContext.ChangePassword(userID, password, oldPassword);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return model;
        }

        public ResponseModel RemoveUser(long userID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._userContext.RemoveUser(userID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
        public async Task<ResponseModel> UpdatefcmToken(UserEntity userentity, string fcm_token)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                return await this._userContext.UpdatefcmToken(userentity, fcm_token);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return model;
        }

        public async Task<UserEntity> ValidateStudentData(string userName, string password)
        {
            try
            {
                return await this._userContext.ValidateStudentData(userName, password);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}