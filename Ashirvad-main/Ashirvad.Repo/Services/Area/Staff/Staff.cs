﻿using Ashirvad.Common;
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
        ResponseModel res = new ResponseModel();

        public async Task<bool> CheckUser(string emailid, string mobileno ,long userID)
        {
            bool isExists = this.context.BRANCH_STAFF.Where(s => (userID == 0 || s.staff_id != userID) && s.email_id == emailid && s.mobile_no == mobileno && s.row_sta_cd == 1).FirstOrDefault() != null;
            return isExists;
        }

        public async Task<ResponseModel> StaffMaintenance(StaffEntity staffInfo)
        {
            try
            {
                Model.BRANCH_STAFF branchStaff = new Model.BRANCH_STAFF();
                if (!CheckUser(staffInfo.EmailID, staffInfo.MobileNo, staffInfo.StaffID).Result)
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
                    res.Status =  this.context.SaveChanges() > 0 ? true : false;
                    res.Message =  res.Status == true ? "user details saved!!!" : "fail to insert!!!";
                }
                else
                {
                    res.Message = "EmailId and Mobile no Already Exists!!!";
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return res;
        }

        public async Task<List<StaffEntity>> GetAllStaff(long branchID)
        {
            var data = (from u in this.context.BRANCH_STAFF
                        where branchID == 0 || u.branch_id == branchID
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
                        where u.staff_id == userID
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
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }
    }

}
