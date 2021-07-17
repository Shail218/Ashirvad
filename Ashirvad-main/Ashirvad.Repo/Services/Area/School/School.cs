using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.School
{
    public class School : ModelAccess, ISchoolAPI
    {
        public async Task<long> SchoolMaintenance(SchoolEntity schoolInfo)
        {
            Model.SCHOOL_MASTER schoolMaster = new Model.SCHOOL_MASTER();

            bool isUpdate = true;
            var data = (from school in this.context.SCHOOL_MASTER
                        where school.school_id == schoolInfo.SchoolID
                        select school).FirstOrDefault();
            if (data == null)
            {
                schoolMaster = new Model.SCHOOL_MASTER();
                isUpdate = false;
            }

            else
            {
                schoolMaster = data;
                schoolInfo.Transaction.TransactionId = schoolMaster.trans_id;
            }
            schoolMaster.school_name = schoolInfo.SchoolName;
            schoolMaster.branch_id = schoolInfo.BranchInfo.BranchID;
            schoolMaster.row_sta_cd = schoolInfo.RowStatus.RowStatusId;
            schoolMaster.trans_id = this.AddTransactionData(schoolInfo.Transaction);
            this.context.SCHOOL_MASTER.Add(schoolMaster);
            if (isUpdate)
            {
                this.context.Entry(schoolMaster).State = System.Data.Entity.EntityState.Modified;
            }
            return this.context.SaveChanges() > 0 ? schoolMaster.school_id : 0;
        }

        public async Task<List<SchoolEntity>> GetAllSchools(long branchID)
        {
            var data = (from u in this.context.SCHOOL_MASTER
                        where branchID == 0 || u.branch_id == branchID
                        select new SchoolEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },

                            SchoolName = u.school_name,
                            SchoolID = u.school_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<SchoolEntity>> GetAllSchools()
        {
            var data = (from u in this.context.SCHOOL_MASTER
                        select new SchoolEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },

                            SchoolName = u.school_name,
                            SchoolID = u.school_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public bool RemoveSchool(long SchoolID, string lastupdatedby)
        {
            var data = (from u in this.context.SCHOOL_MASTER
                        where u.school_id == SchoolID
                        select u).FirstOrDefault();
            if (data != null)
            {
                data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                data.trans_id = this.AddTransactionData(new TransactionEntity()
                {
                    TransactionId = data.trans_id,
                    LastUpdateBy = lastupdatedby
                });
                this.context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<SchoolEntity> GetSchoolsByID(long branchID)
        {
            var data = (from u in this.context.SCHOOL_MASTER
                        where u.school_id == branchID
                        select new SchoolEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },

                            SchoolName = u.school_name,
                            SchoolID = u.school_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }
    }
}
