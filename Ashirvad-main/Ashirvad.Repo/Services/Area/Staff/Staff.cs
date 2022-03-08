using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Staff
{
    public class Staff : ModelAccess, IStaffAPI
    {

        public async Task<long> CheckUser(string mobileno,long userID,string financialyear)
        {
            long result;
            bool isExists =(from u in this.context.BRANCH_STAFF
                            join t in this.context.TRANSACTION_MASTER on u.trans_id equals t.trans_id
                            where ((userID == 0 || u.staff_id != userID) && u.mobile_no == mobileno && u.row_sta_cd == 1 && t.financial_year== financialyear) select u).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> StaffMaintenance(StaffEntity staffInfo)
        {
            Model.BRANCH_STAFF branchStaff = new Model.BRANCH_STAFF();
            if (CheckUser(staffInfo.MobileNo,staffInfo.StaffID,staffInfo.Transaction.FinancialYear).Result != -1)
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

        public async Task<long> UpdateProfile(StaffEntity staffInfo)
        {
            Model.BRANCH_STAFF branchStaff = new Model.BRANCH_STAFF();
            if (CheckUser(staffInfo.MobileNo,staffInfo.StaffID,staffInfo.Transaction.FinancialYear).Result != -1)
            {
                var data = (from staff in this.context.BRANCH_STAFF
                            where staff.staff_id == staffInfo.StaffID
                            select staff).FirstOrDefault();
                branchStaff = data;
                staffInfo.Transaction.TransactionId = data.trans_id;
                branchStaff.name = staffInfo.Name;
                branchStaff.email_id = staffInfo.EmailID;
                branchStaff.mobile_no = staffInfo.MobileNo;
                this.context.BRANCH_STAFF.Add(branchStaff);
                this.context.Entry(branchStaff).State = System.Data.Entity.EntityState.Modified;
                return this.context.SaveChanges() > 0 ? branchStaff.staff_id : 0;
            }
            else
            {
                return -1;
            }
        }

        public async Task<List<StaffEntity>> GetAllStaff(long branchID,string financialyear)
        {
            long Type = branchID == 0 ? 0 : (long)Enums.UserType.Staff;
            var data = (from u in this.context.BRANCH_STAFF
                        .Include("BRANCH_MASTER")
                        join li in this.context.USER_DEF on u.staff_id equals li.staff_id into ps
                        from li in ps.DefaultIfEmpty() orderby u.staff_id descending
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1 && (Type == 0 || li.user_type == Type && u.TRANSACTION_MASTER.financial_year == financialyear)
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
                            User_Password = li.password,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<StaffEntity>> GetAllCustomStaff(DataTableAjaxPostModel model, long branchID, string financialyear)
        {
            var Result = new List<StaffEntity>();
            long Type = branchID == 0 ? 0 : (long)Enums.UserType.Staff;
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.BRANCH_STAFF
                        .Include("BRANCH_MASTER")
                          join li in this.context.USER_DEF on u.staff_id equals li.staff_id into ps
                          from li in ps.DefaultIfEmpty()
                          where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1 && (Type == 0 || li.user_type == Type && u.TRANSACTION_MASTER.financial_year == financialyear)                
                          select new { }).Count();
            var data = (from u in this.context.BRANCH_STAFF
                        .Include("BRANCH_MASTER")
                        join li in this.context.USER_DEF on u.staff_id equals li.staff_id into ps
                        from li in ps.DefaultIfEmpty() 
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1 && (Type == 0 || li.user_type == Type && u.TRANSACTION_MASTER.financial_year == financialyear)
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.name.ToLower().Contains(model.search.value)
                        || u.mobile_no.ToLower().Contains(model.search.value)
                        || u.email_id.ToLower().Contains(model.search.value)) orderby u.staff_id descending
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
                            Count = count,
                            Education = u.education,
                            EmailID = u.email_id,
                            GenderText = u.gender == 1 ? "Male" : u.gender == 2 ? "Female" : "Transgender",
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
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();

            return data;
        }

        public async Task<List<StaffEntity>> GetAllStaff(string financialyear)
        {
            var data = (from u in this.context.BRANCH_STAFF
                        where u.TRANSACTION_MASTER.financial_year == financialyear
                        orderby u.staff_id descending
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
                var data1 = (from u in context.USER_DEF where u.staff_id == StaffID select u).FirstOrDefault();
                if(data1 != null)
                {
                    data1.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data1.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data1.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                }
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
                            UserID = ud.user_id,
                            Userrole = (Enums.UserType)ud.user_type,
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
                            User_Password = ud.password,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }
    }

}
