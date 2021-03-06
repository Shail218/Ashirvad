using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using Ashirvad.Repo.DataAcceessAPI.Area.Staff;
using Ashirvad.Repo.DataAcceessAPI.Area.User;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Staff
{
    public class StaffService : IStaffService
    {
        private readonly IUserAPI _userContext;
        private readonly IStaffAPI _staffContext;
        private readonly IBranchAPI _branchContext;

        public StaffService(IStaffAPI staffContext, IUserAPI userContext, IBranchAPI branchContext)
        {
            this._staffContext = staffContext;
            this._userContext = userContext;
            this._branchContext = branchContext;
        }

        public async Task<ResponseModel> StaffMaintenance(StaffEntity staffInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            StaffEntity staff = new StaffEntity();
            try
            {
                bool isUpdate = staffInfo.StaffID > 0;
                responseModel = await _staffContext.StaffMaintenance(staffInfo);
                if (responseModel.Status)
                {
                    var da = (StaffEntity)responseModel.Data;
                    long staffID = da.StaffID;
                    staff.StaffID = staffID;
                    if (staffID > 0)
                    {
                        var user = await _userContext.UserMaintenance(await this.GetUserData(staffInfo, staffID));
                    }
                }
                             
                //if (staffID > 0)
                //{
                //    staff.StaffID = staffID;
                //    var user = await _userContext.UserMaintenance(await this.GetUserData(staffInfo, staffID));
                //}
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<ResponseModel> UpdateProfile(StaffEntity staffInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            StaffEntity staff = new StaffEntity();
            try
            {
                bool isUpdate = staffInfo.StaffID > 0;
                responseModel = await _staffContext.UpdateProfile(staffInfo);
                if (responseModel.Status)
                {
                    var da = (StaffEntity)responseModel.Data;
                    long staffID = da.StaffID;
                    staff.StaffID = staffID;
                    var user = await _userContext.ProfileMaintenance(new UserEntity()
                    {
                        Username = staffInfo.userNameNew,
                        mobileNo = staffInfo.MobileNo,
                        Transaction = staffInfo.Transaction,
                        UserID = staffInfo.UserID
                    });
                }
                //staff.StaffID = staffID;
                //var user = await _userContext.ProfileMaintenance(new UserEntity()
                //{
                //    Username = staffInfo.MobileNo,
                //    Transaction = staffInfo.Transaction,
                //    UserID = staffInfo.UserID
                //});
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Severity.Error, ex);
            }

            return responseModel;
        }

        private async Task<UserEntity> GetUserData(StaffEntity staffInfo, long studentID)
        {
            var result = await _branchContext.GetAllBranch();
            UserEntity user = new UserEntity()
            {
                BranchInfo = result.Where(x => x.BranchID == staffInfo.BranchInfo.BranchID).FirstOrDefault(),
                ClientSecret = "TESTGUID",
                Password = staffInfo.User_Password,
                RowStatus = staffInfo.RowStatus,
                StaffID = studentID,
                Transaction = staffInfo.Transaction,
                Username = staffInfo.userNameNew,
                mobileNo = staffInfo.MobileNo,
                //UserType = Enums.UserType.Staff
                UserType = staffInfo.Userrole,
                UserID = staffInfo.UserID
            };

            return user;
        }

        public async Task<List<StaffEntity>> GetAllStaff(long branchID)
        {
            try
            {
                return await this._staffContext.GetAllStaff(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveStaff(long StaffID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._staffContext.RemoveStaff(StaffID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<List<StaffEntity>> GetAllCustomStaff(DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._staffContext.GetAllCustomStaff(model,branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StaffEntity>> GetAllStaff()
        {
            try
            {
                return await this._staffContext.GetAllStaff();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<StaffEntity> GetStaffByID(long userID)
        {
            try
            {
                return await this._staffContext.GetStaffByID(userID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
