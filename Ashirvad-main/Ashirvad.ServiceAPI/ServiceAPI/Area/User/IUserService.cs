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
        Task<ResponseModel> UserMaintenance(UserEntity UserInfo);
        Task<ResponseModel> ProfileMaintenance(UserEntity userInfo);
        Task<UserEntity> ValidateUser(string userName, string password);
        Task<bool> CheckAgreement(long branchID);
        Task<UserEntity> ValidateStudent(string userName, string password);
        List<UserEntity> GetAllUsers(long branchID, List<int> userType = null);
        List<UserEntity> GetAllUsersddl(long branchID);
        ResponseModel RemoveUser(long userID, string lastupdatedby);
        bool AddUserRoles(UserEntity user);
        List<RolesEntity> GetRolesByUser(long userID);
        Task<bool> ChangePassword(long userID, string password, string oldPassword);
        Task<UserEntity> GetUserByID(long userID);
        List<UserEntity> GetAllUsers(string userName, string contactNo);
        Task<bool> UpdatefcmToken(UserEntity userentity, string fcm_token);
        Task<UserEntity> ValidateStudentData(string userName, string password);
    }
}
