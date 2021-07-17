using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.User
{
    public interface IUserService
    {
        Task<UserEntity> UserMaintenance(UserEntity UserInfo);
        Task<UserEntity> ValidateUser(string userName, string password);
        List<UserEntity> GetAllUsers(long branchID, List<int> userType = null);
        bool RemoveUser(long userID, string lastupdatedby);
        bool AddUserRoles(UserEntity user);
        List<RolesEntity> GetRolesByUser(long userID);
        Task<bool> ChangePassword(long userID, string password, string oldPassword);
        Task<UserEntity> GetUserByID(long userID);
        List<UserEntity> GetAllUsers(string userName, string contactNo);
        
        }
}
