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
        Task<long> UserMaintenance(UserEntity userInfo);
        Task<UserEntity> ValidateUser(string userName, string password);

        Task<UserEntity> ValidateStudent(string userName, string password);
        List<UserEntity> GetAllUsers(long branchID, List<int> userType);
        bool RemoveUser(long userID, string lastupdatedby);
        bool AddUserRoles(UserEntity user);
        Task<bool> ChangePassword(long userID, string password, string oldPassword);
        List<RolesEntity> GetRolesByUser(long userID);
        Task<UserEntity> GetUserByUserID(long userID);
        List<UserEntity> GetAllUsers(string userName, string contactNo);
    }
}
