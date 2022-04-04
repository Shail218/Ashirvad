using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.User
{
    public interface IUserAPI
    {
        Task<ResponseModel> UserMaintenance(UserEntity userInfo);
        Task<ResponseModel> ProfileMaintenance(UserEntity userInfo);
        Task<UserEntity> ValidateUser(string userName, string password);
        Task<bool> CheckAgreement(long branchID);

        Task<UserEntity> ValidateStudent(string userName, string password);
        List<UserEntity> GetAllUsers(long branchID, List<int> userType);
        ResponseModel RemoveUser(long userID, string lastupdatedby);
        bool AddUserRoles(UserEntity user);
        Task<ResponseModel> ChangePassword(long userID, string password, string oldPassword);
        List<RolesEntity> GetRolesByUser(long userID);
        Task<UserEntity> GetUserByUserID(long userID);
        List<UserEntity> GetAllUsers(string userName, string contactNo);

        List<UserEntity> GetAllUsersddl(long branchID);
        Task<ResponseModel> UpdatefcmToken(UserEntity userentity, string fcm_token);
        Task<ResponseModel> StudentUserMaintenance(UserEntity userInfo, bool IsDeleted = false);

        Task<UserEntity> ValidateStudentData(string userName, string password);
    }
}
