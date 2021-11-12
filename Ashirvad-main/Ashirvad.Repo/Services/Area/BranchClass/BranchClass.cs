using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Ashirvad.Repo.Services.Area.Branch
{
    public class BranchClass : ModelAccess, IBranchClassAPI
    {

        public async Task<long> CheckClass(int ClassDetailID, int ClassID, int BranchID, int CourseDetailID)
        {
            long result;
            bool isExists = this.context.CLASS_DTL_MASTER.Where(s => (ClassDetailID == 0 || s.class_dtl_id != ClassDetailID)
            && s.class_id == ClassID && s.branch_id == BranchID && s.course_dtl_id == CourseDetailID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<long> ClassMaintenance(BranchClassEntity ClassInfo)
        {
            Model.CLASS_DTL_MASTER ClassMaster = new Model.CLASS_DTL_MASTER();
            if (CheckClass((int)ClassInfo.Class_dtl_id, (int)ClassInfo.Class.ClassID, (int)ClassInfo.branch.BranchID, (int)ClassInfo.BranchCourse.course_dtl_id).Result != -1)
            {
                bool isUpdate = true;
                var data = (from Class in this.context.CLASS_DTL_MASTER
                            where Class.class_dtl_id == ClassInfo.Class_dtl_id
                            select new
                            {
                                ClassMaster = Class
                            }).FirstOrDefault();
                if (data == null)
                {
                    ClassMaster = new Model.CLASS_DTL_MASTER();
                    isUpdate = false;
                }
                else
                {
                    ClassMaster = data.ClassMaster;
                    ClassInfo.Transaction.TransactionId = data.ClassMaster.trans_id;
                }

                ClassMaster.class_id = ClassInfo.Class.ClassID;
                ClassMaster.branch_id = ClassInfo.branch.BranchID;
                ClassMaster.row_sta_cd = (int)Enums.RowStatus.Active;
                ClassMaster.is_class = ClassInfo.isClass;
                ClassMaster.course_dtl_id = ClassInfo.BranchCourse.course_dtl_id;
                ClassMaster.trans_id = this.AddTransactionData(ClassInfo.Transaction);
                this.context.CLASS_DTL_MASTER.Add(ClassMaster);
                if (isUpdate)
                {
                    this.context.Entry(ClassMaster).State = System.Data.Entity.EntityState.Modified;
                }
                var result = this.context.SaveChanges();
                if (result > 0)
                {
                    ClassInfo.Class_dtl_id = ClassMaster.class_dtl_id;
                    //var result2 = ClassDetailMaintenance(ClassInfo).Result;
                    return result > 0 ? ClassInfo.Class_dtl_id : 0;
                }
                return this.context.SaveChanges() > 0 ? 1 : 0;
            }
            return -1;
        }

        public async Task<List<BranchClassEntity>> GetAllClass(long BranchID, long ClassID = 0)
        {
            var data = (from u in this.context.CLASS_DTL_MASTER
                        .Include("CLASS_MASTER")
                        .Include("BRANCH_MASTER")
                        where (BranchID == 0 || u.branch_id == BranchID) && (ClassID == 0 || u.course_dtl_id == ClassID) && u.row_sta_cd == 1 && u.is_class == true
                        select new BranchClassEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Class = new ClassEntity()
                            {
                                ClassID = u.CLASS_MASTER.class_id,
                                ClassName = u.CLASS_MASTER.class_name
                            },
                            branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            isClass = u.is_class == true ? true : false,
                            Class_dtl_id = u.class_dtl_id,
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id,

                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            if (data.Count > 0)
            {
                data[0].BranchClassData = (from u in this.context.CLASS_DTL_MASTER
                              .Include("CLASS_MASTER")
                              .Include("BRANCH_MASTER")
                                           where (BranchID == 0 || u.branch_id == BranchID) && (ClassID == 0 || u.course_dtl_id == ClassID) && u.row_sta_cd == 1
                                           select new BranchClassEntity()
                                           {
                                               branch = new BranchEntity()
                                               {
                                                   BranchID = u.BRANCH_MASTER.branch_id,
                                                   BranchName = u.BRANCH_MASTER.branch_name
                                               },

                                               BranchCourse = new BranchCourseEntity()
                                               {
                                                   course_dtl_id = u.course_dtl_id,
                                                   course = new CourseEntity()
                                                   {
                                                       CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                                   }
                                               },
                                           }).Distinct().ToList();
            }
            else
            {
                BranchClassEntity entity = new BranchClassEntity();
                entity.BranchClassData = new List<BranchClassEntity>();
                data.Add(entity);
            }
            return data;

        }


        public async Task<List<BranchClassEntity>> GetClassByClassID(long ClassID, long BranchID)
        {
            var data = (from u in this.context.CLASS_DTL_MASTER
                       .Include("CLASS_MASTER")
                        where u.row_sta_cd == 1 && u.course_dtl_id == ClassID && u.branch_id == BranchID
                        select new BranchClassEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Class = new ClassEntity()
                            {
                                ClassID = u.CLASS_MASTER.class_id,
                                ClassName = u.CLASS_MASTER.class_name
                            },
                            Class_dtl_id = u.class_dtl_id,
                            isClass = u.is_class == true ? true : false,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            if (data.Count > 0)
            {
                data[0].branchClass = (from u in this.context.CLASS_DTL_MASTER
                              .Include("CLASS_MASTER")
                              .Include("BRANCH_MASTER")
                                       where u.row_sta_cd == 1 && u.course_dtl_id == ClassID && u.branch_id == BranchID
                                       select new BranchClassEntity()
                                       {
                                           BranchCourse = new BranchCourseEntity()
                                           {
                                               course_dtl_id = u.course_dtl_id,
                                               course = new CourseEntity()
                                               {
                                                   CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                               }
                                           },

                                       }).FirstOrDefault();
            }
            return data;
        }
        public async Task<BranchClassEntity> GetClassbyID(long ClassID)
        {
            var data = (from u in this.context.CLASS_DTL_MASTER
                       .Include("Class_MASTER")
                        where u.row_sta_cd == 1 && u.branch_id == ClassID
                        select new BranchClassEntity()
                        {
                            Class_dtl_id = u.class_dtl_id,
                            Class = new ClassEntity()
                            {
                                ClassID = u.CLASS_MASTER.class_id,
                                ClassName = u.CLASS_MASTER.class_name
                            },

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).FirstOrDefault();
            return data;
        }

        public bool RemoveClass(long ClassID, long BranchID, string lastupdatedby)
        {
            var data = (from u in this.context.CLASS_DTL_MASTER
                        where u.branch_id == BranchID && u.course_dtl_id == ClassID
                        select u).ToList();
            if (data != null)
            {
                foreach (var item in data)
                {
                    item.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    item.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = item.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                }

                return true;
            }

            return false;
        }



    }
}
