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

        public async Task<UserEntity> UserMaintenance(UserEntity UserInfo)
        {
            UserEntity user = new UserEntity();
            try
            {
                long userID = await this._userContext.UserMaintenance(UserInfo);
                if (userID > 0)
                {
                    //Add User
                    //Get Branch
                    user.UserID = userID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return user;
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

        public async Task<bool> ChangePassword(long userID, string password, string oldPassword)
        {
            try
            {
                return await this._userContext.ChangePassword(userID, password, oldPassword);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public bool RemoveUser(long userID, string lastupdatedby)
        {
            try
            {
                return this._userContext.RemoveUser(userID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}