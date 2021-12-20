using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Class
{
    public class Class : ModelAccess, IClassAPI
    {
        private readonly IBranchClassAPI _BranchClass;

        public Class(IBranchClassAPI BranchClass)
        {
            _BranchClass = BranchClass;
        }

        public async Task<long> CheckClass(string name, long Id)
        {
            long result;
            bool isExists = this.context.CLASS_MASTER.Where(s => (Id == 0 || s.class_id != Id) && s.class_name == name && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> ClassMaintenance(ClassEntity classEntity)
        {
            Model.CLASS_MASTER classMaster = new Model.CLASS_MASTER();
            if (CheckClass(classEntity.ClassName, classEntity.ClassID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from cl in this.context.CLASS_MASTER
                            where cl.class_id == classEntity.ClassID
                            select new
                            {
                                classMaster = cl
                            }).FirstOrDefault();
                if (data == null)
                {
                    classMaster = new Model.CLASS_MASTER();
                    isUpdate = false;
                }
                else
                {
                    classMaster = data.classMaster;
                    classEntity.Transaction.TransactionId = data.classMaster.trans_id;
                }
                classMaster.class_name = classEntity.ClassName;
                classMaster.row_sta_cd = classEntity.RowStatus.RowStatusId;
                classMaster.trans_id = this.AddTransactionData(classEntity.Transaction);
                this.context.CLASS_MASTER.Add(classMaster);
                if (isUpdate)
                {
                    this.context.Entry(classMaster).State = System.Data.Entity.EntityState.Modified;
                }
                var Result = this.context.SaveChanges();
                if (Result > 0)
                {
                    classEntity.ClassID = classMaster.class_id;
                    classEntity.Transaction.TransactionId = classEntity.Transaction.TransactionId;
                    ClassMasterMaintenance(classEntity);
                }
                return Result > 0 ? classEntity.ClassID : 0;
            }
            else
            {
                return -1;
            }
        }

        public async Task<ClassEntity> GetClassByClassID(long classID)
        {
            var data = (from u in this.context.CLASS_MASTER
                        where u.class_id == classID
                        select new ClassEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            ClassID = u.class_id,
                            ClassName = u.class_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }

        public async Task<List<ClassEntity>> GetAllClass()
        {
            var data = (from u in this.context.CLASS_MASTER
                        orderby u.class_name descending
                        where u.row_sta_cd == 1
                        select new ClassEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            ClassID = u.class_id,
                            ClassName = u.class_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },

                        }).ToList();

            return data;
        }

        public bool RemoveClass(long classID, string lastupdatedby)
        {
            var data = (from u in this.context.CLASS_MASTER
                        where u.class_id == classID
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

        public async Task<long> ClassMasterMaintenance(ClassEntity ClassEntity)
        {
            try
            {
                long result = 0;
                var data = (from Class in this.context.CLASS_DTL_MASTER
                            select new BranchClassEntity
                            {
                                branch=new BranchEntity()
                                {
                                    BranchID = Class.branch_id
                                },
                                Class= new ClassEntity()
                                {
                                    ClassID= Class.class_id
                                },
                                BranchCourse= new BranchCourseEntity()
                                {
                                    course_dtl_id= Class.course_dtl_id

                                }
                            }).Distinct().ToList();

                BranchClassEntity branchClass = new BranchClassEntity();
                branchClass.Class = new ClassEntity()
                {
                    ClassID = ClassEntity.ClassID,
                    ClassName = ClassEntity.ClassName
                };
                branchClass.Transaction = new TransactionEntity();
                branchClass.Transaction = ClassEntity.Transaction;
                branchClass.isClass = false;
                branchClass.RowStatus = new RowStatusEntity()
                {
                    RowStatus = Enums.RowStatus.Active
                };
                foreach (var item in data)
                {
                    branchClass.branch = new BranchEntity()
                    {
                        BranchID = item.branch.BranchID,

                    };
                    branchClass.BranchCourse = new BranchCourseEntity()
                    {
                        course_dtl_id = item.BranchCourse.course_dtl_id,

                    };
                    branchClass.Class_dtl_id = 0;
                    result = _BranchClass.ClassMaintenance(branchClass).Result;
                }


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
