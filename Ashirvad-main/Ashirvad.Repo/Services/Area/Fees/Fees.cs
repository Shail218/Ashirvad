using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area
{
    public class Fees : ModelAccess, IFeesAPI
    {

        public async Task<long> CheckFees(long BranchID, long StdID, long feesid,long courseid)
        {
            long result;
            bool isExists = this.context.FEE_STRUCTURE_MASTER.Where(s => (feesid == 0 || s.fee_struct_mst_id != feesid) && s.branch_id == BranchID && s.class_dtl_id == StdID && s.course_dtl_id==courseid && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<long> FeesMaintenance(FeesEntity FeesInfo)
        {
            Model.FEE_STRUCTURE_MASTER FeesMaster = new Model.FEE_STRUCTURE_MASTER();
            if (CheckFees(FeesInfo.BranchInfo.BranchID, FeesInfo.BranchClass.Class_dtl_id, FeesInfo.FeesID, FeesInfo.BranchCourse.course_dtl_id).Result != -1)
            {
                bool isUpdate = true;
                var data = (from Fees in this.context.FEE_STRUCTURE_MASTER
                            where Fees.fee_struct_mst_id == FeesInfo.FeesID
                            select new
                            {
                                FeesMaster = Fees
                            }).FirstOrDefault();
                if (data == null)
                {
                    FeesMaster = new Model.FEE_STRUCTURE_MASTER();
                    isUpdate = false;
                }
                else
                {
                    FeesMaster = data.FeesMaster;
                    FeesInfo.Transaction.TransactionId = data.FeesMaster.trans_id;
                }

                FeesMaster.branch_id = FeesInfo.BranchInfo.BranchID;
                FeesMaster.class_dtl_id = FeesInfo.BranchClass.Class_dtl_id;
                FeesMaster.course_dtl_id = FeesInfo.BranchCourse.course_dtl_id;
                FeesMaster.remarks = FeesInfo.Remark;
                FeesMaster.row_sta_cd = FeesInfo.RowStatus.RowStatusId;
                FeesMaster.trans_id = this.AddTransactionData(FeesInfo.Transaction);
                this.context.FEE_STRUCTURE_MASTER.Add(FeesMaster);
                if (isUpdate)
                {
                    this.context.Entry(FeesMaster).State = System.Data.Entity.EntityState.Modified;
                }
                var result = this.context.SaveChanges();
                if (result > 0)
                {
                    FeesInfo.FeesID = FeesMaster.fee_struct_mst_id;
                    var result2 = FeesDetailMaintenance(FeesInfo).Result;
                    return result2 > 0 ? FeesInfo.FeesID : 0;
                }
                return this.context.SaveChanges() > 0 ? FeesMaster.fee_struct_mst_id : 0;
            }
            return -1;
        }

        public async Task<long> FeesDetailMaintenance(FeesEntity FeesInfo)
        {
            Model.FEE_STRUCTURE_DTL FeesMaster = new Model.FEE_STRUCTURE_DTL();
            bool isUpdate = true;
            var data = (from Fees in this.context.FEE_STRUCTURE_DTL
                        where Fees.fee_struct_dtl_id == FeesInfo.FeesDetailID
                        select new
                        {
                            FeesMaster = Fees
                        }).FirstOrDefault();
            if (data == null)
            {
                FeesMaster = new Model.FEE_STRUCTURE_DTL();
                isUpdate = false;
            }
            else
            {
                FeesMaster = data.FeesMaster;
                FeesInfo.Transaction.TransactionId = data.FeesMaster.trans_id;
            }

            FeesMaster.fee_content = FeesInfo.Fees_Content;
            FeesMaster.file_name = FeesInfo.FileName;
            FeesMaster.fee_struct_mst_id = FeesInfo.FeesID;
            FeesMaster.file_path = FeesInfo.FilePath;
            FeesMaster.row_sta_cd = FeesInfo.RowStatus.RowStatusId;
            FeesMaster.trans_id = this.AddTransactionData(FeesInfo.Transaction);
            this.context.FEE_STRUCTURE_DTL.Add(FeesMaster);
            if (isUpdate)
            {
                this.context.Entry(FeesMaster).State = System.Data.Entity.EntityState.Modified;
            }

            return this.context.SaveChanges() > 0 ? FeesMaster.fee_struct_dtl_id : 0;
        }

        public async Task<List<FeesEntity>> GetAllFees(long BranchID)
        {
            var data = (from u in this.context.FEE_STRUCTURE_MASTER
                        join b in this.context.FEE_STRUCTURE_DTL on u.fee_struct_mst_id equals b.fee_struct_mst_id orderby u.fee_struct_mst_id descending
                        where u.row_sta_cd == 1 && u.branch_id == BranchID
                        select new FeesEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            FeesID = u.fee_struct_mst_id,
                            FeesDetailID = b.fee_struct_dtl_id,
                            Remark = u.remarks,
                            FilePath = "https://mastermind.org.in" + b.file_path,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchClass= new BranchClassEntity()
                            {
                                Class_dtl_id= u.class_dtl_id.HasValue? u.class_dtl_id.Value:0,
                                Class= new ClassEntity()
                                {
                                    ClassID= u.CLASS_DTL_MASTER.class_id,
                                    ClassName= u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id }
                        }).ToList();

            return data;
        }

        public async Task<List<FeesEntity>> GetAllCustomFees(DataTableAjaxPostModel model, long branchID)
        {
            var Result = new List<FeesEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.FEE_STRUCTURE_MASTER
                          join b in this.context.FEE_STRUCTURE_DTL on u.fee_struct_mst_id equals b.fee_struct_mst_id
                          orderby u.fee_struct_mst_id descending
                          where u.row_sta_cd == 1 && u.branch_id == branchID
                          select new FeesEntity() {
                              FeesID = u.fee_struct_mst_id
                          }).Distinct().Count();
            var data = (from u in this.context.FEE_STRUCTURE_MASTER.Include("CLASS_DTL_MASTER")
                        join b in this.context.FEE_STRUCTURE_DTL on u.fee_struct_mst_id equals b.fee_struct_mst_id
                        orderby u.fee_struct_mst_id descending
                        where u.row_sta_cd == 1 && u.branch_id == branchID
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.CLASS_DTL_MASTER.CLASS_MASTER.class_name.ToLower().Contains(model.search.value)
                        || u.remarks.ToLower().Contains(model.search.value))
                        orderby u.fee_struct_mst_id descending
                        select new FeesEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            FeesID = u.fee_struct_mst_id,
                            FeesDetailID = b.fee_struct_dtl_id,
                            Remark = u.remarks,
                            Count = count,
                            FilePath = "https://mastermind.org.in" + b.file_path,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id }
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }

        public async Task<List<FeesEntity>> GetAllFeesByBranchID(long BranchID,long courseid, long StdID)
        {
            var data = (from u in this.context.FEE_STRUCTURE_MASTER
                        join b in this.context.FEE_STRUCTURE_DTL on u.fee_struct_mst_id equals b.fee_struct_mst_id
                        orderby u.fee_struct_mst_id descending
                        where u.row_sta_cd == 1 && u.branch_id == BranchID && (u.class_dtl_id == StdID || StdID == 0)
                        && (u.course_dtl_id == courseid || courseid == 0)
                        select new FeesEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            FeesID = u.fee_struct_mst_id,
                            FeesDetailID = b.fee_struct_dtl_id,
                            Remark = u.remarks,
                            FilePath = "https://mastermind.org.in" + b.file_path,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id }
                        }).ToList();

            return data;
        }

        public Task<List<FeesEntity>> GetAllFeesWithoutImage()
        {
            throw new NotImplementedException();
        }

        public async Task<FeesEntity> GetFeesByFeesID(long FeesID)
        {
            var data = (from u in this.context.FEE_STRUCTURE_MASTER
                        join b in this.context.FEE_STRUCTURE_DTL on u.fee_struct_mst_id equals b.fee_struct_mst_id
                        where u.fee_struct_mst_id == FeesID
                        select new FeesEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            FeesID = u.fee_struct_mst_id,
                            Remark = u.remarks,
                            FeesDetailID = b.fee_struct_dtl_id,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            FilePath = b.file_path,
                            FileName = b.file_name,
                            Fees_Content = b.fee_content
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveFees(long FeesID, string lastupdatedby)
        {
            var data = (from u in this.context.FEE_STRUCTURE_MASTER
                        where u.fee_struct_mst_id == FeesID
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
    }
}
