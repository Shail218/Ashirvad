using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Staff
{
    public class Staff : ModelAccess, IStaffAPI
    {
  
        public async Task<long> CheckUser(string emailid, string mobileno, long branch, long userID)
        {
            long result;
            bool isExists = this.context.BRANCH_STAFF.Where(s => (userID == 0 || s.staff_id != userID) && (s.email_id == emailid || s.mobile_no == mobileno) && s.branch_id == branch && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> StaffMaintenance(StaffEntity staffInfo)
        {
            Model.BRANCH_STAFF branchStaff = new Model.BRANCH_STAFF();
            if (CheckUser(staffInfo.EmailID, staffInfo.MobileNo, staffInfo.BranchInfo.BranchID,staffInfo.StaffID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from staff in this.context.BRANCH_STAFF
                            where staff.staff_id == staffInfo.StaffID
                            select staff).FirstOrDefault();
                if (data == null)
                {
                    branchStaff = new Model.BRANCH_STAFF();
                    isUpdate = false;
                }
                else
                {
                    branchStaff = data;
                    staffInfo.Transaction.TransactionId = data.trans_id;
                }

                branchStaff.name = staffInfo.Name;
                branchStaff.education = staffInfo.Education;
                branchStaff.dob = staffInfo.DOB;
                branchStaff.gender = (int)staffInfo.Gender;
                branchStaff.address = staffInfo.Address;
                branchStaff.appt_dt = staffInfo.ApptDT;
                branchStaff.join_dt = staffInfo.JoinDT;
                branchStaff.leaving_dt = staffInfo.LeavingDT;
                branchStaff.email_id = staffInfo.EmailID;
                branchStaff.branch_id = staffInfo.BranchInfo.BranchID;
                branchStaff.mobile_no = staffInfo.MobileNo;
                branchStaff.row_sta_cd = staffInfo.RowStatus.RowStatusId;
                branchStaff.trans_id = this.AddTransactionData(staffInfo.Transaction);
                this.context.BRANCH_STAFF.Add(branchStaff);
                if (isUpdate)
                {
                    this.context.Entry(branchStaff).State = System.Data.Entity.EntityState.Modified;
                }
                return this.context.SaveChanges() > 0 ? branchStaff.staff_id : 0;
            }
            else
            {
                return -1;
            }
        }

        public async Task<List<StaffEntity>> GetAllStaff(long branchID)
        {
            long Type = branchID == 0 ? 0 : (long)Enums.UserType.Staff;
            var data = (from u in this.context.BRANCH_STAFF
                        .Include("BRANCH_MASTER")
                        join li in this.context.USER_DEF on u.staff_id equals li.staff_id into ps
                        from li in ps.DefaultIfEmpty()
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1 && (Type == 0 || li.user_type == Type)
                        select new StaffEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Address = u.address,
                            ApptDT = u.appt_dt,
                            DOB = u.dob,
                            Education = u.education,
                            EmailID = u.email_id,
                            Gender = u.gender == 1 ? Enums.Gender.Male : u.gender == 2 ? Enums.Gender.Female : Enums.Gender.Transgender,
                            JoinDT = u.join_dt,
                            LeavingDT = u.leaving_dt,
                            MobileNo = u.mobile_no,
                            Name = u.name,
                            UserID = li.user_id,
                            StaffID = u.staff_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<StaffEntity>> GetAllStaff()
        {
            var data = (from u in this.context.BRANCH_STAFF
                        select new StaffEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Address = u.address,
                            ApptDT = u.appt_dt,
                            DOB = u.dob,
                            Education = u.education,
                            EmailID = u.email_id,
                            Gender = u.gender == 1 ? Enums.Gender.Male : u.gender == 2 ? Enums.Gender.Female : Enums.Gender.Transgender,
                            JoinDT = u.join_dt,
                            LeavingDT = u.leaving_dt,
                            MobileNo = u.mobile_no,
                            Name = u.name,
                            StaffID = u.staff_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public bool RemoveStaff(long StaffID, string lastupdatedby)
        {
            var data = (from u in this.context.BRANCH_STAFF
                        where u.staff_id == StaffID
                        select u).FirstOrDefault();
            if (data != null)
            {
                data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                this.context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<StaffEntity> GetStaffByID(long userID)
        {
            var data = (from u in this.context.BRANCH_STAFF
                        join ud in this.context.USER_DEF on u.staff_id equals ud.staff_id
                        where u.staff_id == userID
                        select new StaffEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            UserID= ud.user_id,
                            Address = u.address,
                            ApptDT = u.appt_dt,
                            DOB = u.dob,
                            Education = u.education,
                            EmailID = u.email_id,
                            Gender = u.gender == 1 ? Enums.Gender.Male : u.gender == 2 ? Enums.Gender.Female : Enums.Gender.Transgender,
                            JoinDT = u.join_dt,
                            LeavingDT = u.leaving_dt,
                            MobileNo = u.mobile_no,
                            Name = u.name,
                            StaffID = u.staff_id,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }
    }

}
