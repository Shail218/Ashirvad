using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.School
{
    public class School : ModelAccess, ISchoolAPI
    {
        public async Task<long> Checkschool(string name, long branch, long Id)
        {
            long result;
            bool isExists = this.context.SCHOOL_MASTER.Where(s => (Id == 0 || s.school_id != Id) && s.school_name == name && s.branch_id == branch && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> SchoolMaintenance(SchoolEntity schoolInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.SCHOOL_MASTER schoolMaster = new Model.SCHOOL_MASTER();
                if (Checkschool(schoolInfo.SchoolName, schoolInfo.BranchInfo.BranchID, schoolInfo.SchoolID).Result != -1)
                {
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
                    var da = this.context.SaveChanges() > 0 ? schoolMaster.school_id : 0;
                    if (da > 0)
                    {
                        schoolInfo.SchoolID = da;
                        responseModel.Data = schoolInfo;
                        responseModel.Message = isUpdate == true ? "School Updated Successfully." : "School Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "School Not Updated." : "School Not Inserted.";
                        responseModel.Status = false;
                    }
                    //return this.context.SaveChanges() > 0 ? schoolMaster.school_id : 0;
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "School Already Exists.";
                    //return -1;
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
        }

        public async Task<List<SchoolEntity>> GetAllSchools(long branchID)
        {
            var data = (from u in this.context.SCHOOL_MASTER
                        orderby u.school_id descending
                        where branchID == 0 || u.branch_id == branchID && u.row_sta_cd == 1
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

        public async Task<List<SchoolEntity>> GetAllCustomSchools(DataTableAjaxPostModel model, long branchID)
        {
            var Result = new List<SchoolEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = this.context.SCHOOL_MASTER.Where(s => s.row_sta_cd == 1 && s.branch_id == branchID).Distinct().Count();
            var data = (from u in this.context.SCHOOL_MASTER
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1
&& (model.search.value == null
|| model.search.value == ""
|| u.school_name.ToLower().Contains(model.search.value))
                        orderby u.school_id descending
                        select new SchoolEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            SchoolName = u.school_name,
                            Count = count,
                            SchoolID = u.school_id,
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

        public async Task<List<SchoolEntity>> GetAllSchools()
        {
            var data = (from u in this.context.SCHOOL_MASTER
                        orderby u.school_id descending
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

        public ResponseModel RemoveSchool(long SchoolID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
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
                    responseModel.Message = "School Removed Successfully.";
                    responseModel.Status = true;
                    //return true;
                }
                else
                {
                    responseModel.Message = "School Not Found";
                    responseModel.Status = false;
                }
            }
            catch(Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
             return responseModel;
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



        public async Task<List<SchoolEntity>> GetAllExportSchools(long branchID)
        {
            var data = (from u in this.context.SCHOOL_MASTER
                        orderby u.school_id descending
                        where (branchID == 0 || u.branch_id == branchID)
                        && u.row_sta_cd == 1
                        select new SchoolEntity()
                        {
                            SchoolName = u.school_name,


                        }).ToList();

            return data;
        }
    }
}
