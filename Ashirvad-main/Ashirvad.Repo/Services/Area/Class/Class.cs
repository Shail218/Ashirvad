using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Class;
using Ashirvad.Repo.DataAcceessAPI.Area.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Class
{
    public class Class : ModelAccess, IClassAPI
    {
        private readonly IBranchClassAPI _BranchClass;
        private readonly IStandardAPI _standard;

        public Class(IBranchClassAPI BranchClass, IStandardAPI standard)
        {
            _BranchClass = BranchClass;
            _standard = standard;
        }

        public async Task<long> CheckClass(string name, long courseid, long Id)
        {
            long result;
            bool isExists = this.context.CLASS_MASTER.Where(s => (Id == 0 || s.class_id != Id) && s.course_id == courseid && s.class_name == name && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> ClassMaintenance(ClassEntity classEntity)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.CLASS_MASTER classMaster = new Model.CLASS_MASTER();
                if (CheckClass(classEntity.ClassName, classEntity.courseEntity.CourseID, classEntity.ClassID).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from cl in this.context.CLASS_MASTER
                                where cl.class_id == classEntity.ClassID
                                select cl).FirstOrDefault();
                    if (data == null)
                    {
                        classMaster = new Model.CLASS_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        classMaster = data;
                        classEntity.Transaction.TransactionId = classMaster.trans_id;
                    }
                    classMaster.class_name = classEntity.ClassName;
                    classMaster.row_sta_cd = classEntity.RowStatus.RowStatusId;
                    classMaster.trans_id = this.AddTransactionData(classEntity.Transaction);
                    classMaster.course_id = classEntity.courseEntity.CourseID;
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
                        if ((int)classEntity.UserType == 5)
                        {
                            SuperadminClassMasterMaintenance(classEntity);
                        }
                        else
                        {
                            ClassMasterMaintenance(classEntity);
                        }

                        // UpdateStandard(classEntity);
                    }
                    var re = Result > 0 ? classEntity.ClassID : 0;
                    if (re > 0)
                    {
                        classEntity.ClassID = classMaster.class_id;
                        responseModel.Data = classEntity;
                        responseModel.Status = true;
                        responseModel.Message = isUpdate == true ? "Class Updated Successfully." : "Class Inserted Successfully.";
                    }
                    else
                    {
                        responseModel.Status = false;
                        responseModel.Message = isUpdate==true?"Class Not Updated.":"Class Not Inserted.";
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Class Already Exist.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
         
        }

        public async Task<ClassEntity> GetClassByClassID(long classID)
        {
            var data = (from u in this.context.CLASS_MASTER
                        .Include("COURSE_MASTER")
                        where u.class_id == classID && u.row_sta_cd == 1
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
                            courseEntity = new CourseEntity()
                            {
                                CourseID = u.COURSE_MASTER.course_id,
                                CourseName = u.COURSE_MASTER.course_name
                            },

                            OldStandard = u.class_name,
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

        public async Task<List<ClassEntity>> GetAllClassByCourse(long courseid, bool Isupdate = false)
        {
            if (Isupdate)
            {

                var data = (from u in this.context.CLASS_DTL_MASTER
                            orderby u.CLASS_MASTER.class_name descending
                            where u.row_sta_cd == 1 && u.course_dtl_id == courseid
                            select new ClassEntity()
                            {
                                RowStatus = new RowStatusEntity()
                                {
                                    RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                    RowStatusId = (int)u.row_sta_cd,
                                    RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                                },
                                ClassID = u.class_id,
                                ClassName = u.CLASS_MASTER.class_name,
                                Transaction = new TransactionEntity() { TransactionId = u.trans_id },

                            }).OrderByDescending(a => a.ClassID).ToList();

                return data;


            }
            else
            {
                long CourseID = 0;
                var coursedata = this.context.COURSE_DTL_MASTER.Where(s => s.course_dtl_id == courseid && s.row_sta_cd == 1).FirstOrDefault();
                CourseID = coursedata == null ? CourseID : coursedata.course_id;
                var data = (from u in this.context.CLASS_MASTER
                            orderby u.class_name descending
                            where u.row_sta_cd == 1 && u.course_id == CourseID
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

                            }).OrderByDescending(a => a.ClassID).ToList();

                return data;

            }

        }

        public async Task<List<ClassEntity>> GetAllClassDDL(long BranchID, long ClassID = 0)
        {

            var data = (from u in this.context.CLASS_MASTER
                        where u.course_id == ClassID && u.row_sta_cd == 1
                        select new ClassEntity()
                        {
                            ClassID = u.class_id,
                            ClassName = u.class_name
                        }).ToList();
            return data;
        }

        public async Task<List<ClassEntity>> GetAllCustomClass(DataTableAjaxPostModel model)
        {
            var Count = (from u in this.context.CLASS_MASTER
                        .Include("COURSE_MASTER")
                         orderby u.class_name descending
                         where u.row_sta_cd == 1
                         && (
                         model.search.value == null || model.search.value == ""
                         || u.COURSE_MASTER.course_name.ToLower().Contains(model.search.value)
                         || u.class_name.ToLower().Contains(model.search.value)
                         )
                         select new ClassEntity()
                         {
                             RowStatus = new RowStatusEntity()
                             {
                                 RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                 RowStatusId = (int)u.row_sta_cd,
                                 RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                             },
                             courseEntity = new CourseEntity()
                             {
                                 CourseName = u.COURSE_MASTER.course_name
                             },
                             ClassID = u.class_id,
                             ClassName = u.class_name,
                             course_id = u.course_id,
                             Transaction = new TransactionEntity() { TransactionId = u.trans_id },

                         }).Count();
            var data = (from u in this.context.CLASS_MASTER
                        .Include("COURSE_MASTER")
                        orderby u.class_id descending
                        where u.row_sta_cd == 1
                        && (
                         model.search.value == null || model.search.value == ""
                         || u.COURSE_MASTER.course_name.ToLower().Contains(model.search.value)
                         || u.class_name.ToLower().Contains(model.search.value)
                        )
                        select new ClassEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            courseEntity = new CourseEntity()
                            {
                                CourseName = u.COURSE_MASTER.course_name
                            },
                            ClassID = u.class_id,
                            ClassName = u.class_name,
                            course_id = u.course_id,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            Count = Count
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();

            return data;
        }

        public bool CheckHistory(long classID)
        {
            bool Issuccess = true;
            Issuccess = this.context.CLASS_DTL_MASTER.Where(s => s.class_id == classID && s.is_class == true && s.row_sta_cd == 1).FirstOrDefault() != null;
            if (Issuccess)
            {
                return false;
            }
            return true;
        }

        public ResponseModel RemoveClass(long classID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            Check_Delete check = new Check_Delete();
            string message = "";
            try
            {
                var data = (from u in this.context.CLASS_MASTER
                            where u.class_id == classID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    var data1 = (from a in this.context.CLASS_DTL_MASTER
                                 where a.class_id == data.class_id && a.is_class == true && a.row_sta_cd == 1
                                 select a).ToList();
                    foreach (var item in data1)
                    {
                        var data_course = check.check_remove_class_superadmin(item.class_dtl_id).Result;
                        if (data_course.Status)
                        {
                            data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                            data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                            item.row_sta_cd = (int)Enums.RowStatus.Inactive;
                            item.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = item.trans_id, LastUpdateBy = lastupdatedby });
                            this.context.SaveChanges();
                        }
                        else
                        {
                            message = message + data_course.Message;
                            break;
                        }
                    }
                    responseModel.Status = message == "" ? true : false;
                    responseModel.Message = message == "" ? "Class Removed Successfully!!" : message;
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
        }

        public async Task<ResponseModel> ClassMasterMaintenance(ClassEntity ClassEntity)
        {
            ResponseModel result = new ResponseModel();
            try
            {
             
                var List = GetClassdetails(ClassEntity.courseEntity.CourseID).Result;
                BranchClassEntity branchClass = new BranchClassEntity();
                branchClass.Class = new ClassEntity()
                {
                    ClassID = ClassEntity.ClassID,
                    ClassName = ClassEntity.ClassName
                };
                branchClass.UserType = ClassEntity.UserType;
                branchClass.Transaction = new TransactionEntity();
                branchClass.Transaction = ClassEntity.Transaction;
                branchClass.isClass = false;
                branchClass.RowStatus = new RowStatusEntity()
                {
                    RowStatus = Enums.RowStatus.Active
                };
                foreach (var item in List)
                {
                    branchClass.BranchCourse = new BranchCourseEntity()
                    {
                        course_dtl_id = item.BranchCourse.course_dtl_id
                    };
                    branchClass.branch = item.branch;
                    result = _BranchClass.ClassMaintenance(branchClass).Result;
                }
                //if ((int)ClassEntity.UserType == 5)
                //{
                //    StandardEntity standard = new StandardEntity();
                //    standard.BranchInfo = new BranchEntity()
                //    {
                //        BranchID = ClassEntity.branchEntity.BranchID,

                //    };
                //    standard.Branchclass = new BranchClassEntity()
                //    {
                //        Class_dtl_id = 0,

                //    };
                //    standard.Standard = ClassEntity.ClassName;
                //    standard.Transaction = new TransactionEntity()
                //    {
                //        TransactionId = 1
                //    };
                //    standard.RowStatus = new RowStatusEntity()
                //    {
                //        RowStatus = Enums.RowStatus.Active
                //    };
                //    if (ClassEntity.ClassID > 0)
                //    {
                //        Model.STD_MASTER sTD_ = new Model.STD_MASTER();
                //        var std = (from cl in this.context.STD_MASTER
                //                   where cl.standard == ClassEntity.OldStandard
                //                   && cl.row_sta_cd == 1
                //                   && cl.branch_id == ClassEntity.branchEntity.BranchID
                //                   select new
                //                   {
                //                       sTD_ = cl
                //                   }).FirstOrDefault();

                //        if (std != null)
                //        {
                //            standard.StandardID = std.sTD_.std_id;
                //        }
                //    }

                //    result = _standard.StandardMaintenance(standard).Result;
                //}
            
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message.ToString();
            }
            return result;
        }

        public async Task<ResponseModel> SuperadminClassMasterMaintenance(ClassEntity ClassEntity)
        {
            ResponseModel result = new ResponseModel();
            try
            {
               
                var List = (from u in context.COURSE_DTL_MASTER
                            where u.branch_id == ClassEntity.branchEntity.BranchID 
                            && u.row_sta_cd == 1
                            && u.course_id == ClassEntity.courseEntity.CourseID
                            select new BranchCourseEntity
                            {
                                course_dtl_id = u.course_dtl_id,
                                branch = new BranchEntity()
                                {
                                    BranchID = u.branch_id
                                }
                            }).ToList();
                BranchClassEntity branchClass = new BranchClassEntity();
                branchClass.Class = new ClassEntity()
                {
                    ClassID = ClassEntity.ClassID,
                    ClassName = ClassEntity.ClassName
                };
                branchClass.UserType = ClassEntity.UserType;
                branchClass.Transaction = new TransactionEntity();
                branchClass.Transaction = ClassEntity.Transaction;
                branchClass.isClass = true;
                branchClass.RowStatus = new RowStatusEntity()
                {
                    RowStatus = Enums.RowStatus.Active
                };
                foreach (var item in List)
                {
                    branchClass.BranchCourse = new BranchCourseEntity()
                    {
                        course_dtl_id = item.course_dtl_id
                    };
                    branchClass.branch = item.branch;
                    result = _BranchClass.ClassMaintenance(branchClass).Result;
                }                
               
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message.ToString();
            }
             return result;
        }

        public async Task<long> UpdateStandard(ClassEntity ClassEntity)
        {
            try
            {
                long result = 0;
                var data = (from std in this.context.STD_MASTER
                            .Include("CLASS_DTL_MASTER")
                            where std.CLASS_DTL_MASTER.class_id == ClassEntity.ClassID
                            && std.CLASS_DTL_MASTER.is_class == true
                            && std.row_sta_cd == 1
                            select new StandardEntity
                            {
                                StandardID = std.std_id
                            }).Distinct().ToList();
                if (data?.Count > 0)
                {
                    foreach (var item in data)
                    {
                        Model.STD_MASTER _MASTER = new Model.STD_MASTER();

                        var master = (from cl in this.context.STD_MASTER
                                      where cl.std_id == item.StandardID
                                      select new
                                      {
                                          _MASTER = cl
                                      }).FirstOrDefault();
                        if (master != null)
                        {
                            _MASTER = master._MASTER;
                            _MASTER.standard = ClassEntity.ClassName;
                            this.context.STD_MASTER.Add(_MASTER);
                            this.context.Entry(_MASTER).State = System.Data.Entity.EntityState.Modified;
                            var Result = this.context.SaveChanges();

                        }


                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<CourseEntity>> GetAllCourse()
        {
            var data = (from u in this.context.COURSE_MASTER
                        where u.row_sta_cd == 1
                        select new CourseEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            CourseID = u.course_id,
                            CourseName = u.course_name,

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }

                        }).ToList();

            return data;
        }

        public async Task<List<BranchClassEntity>> GetClassdetails(long CourseID)
        {
            var data = (from u in this.context.CLASS_DTL_MASTER
                        where u.row_sta_cd == 1 && u.COURSE_DTL_MASTER.course_id == CourseID
                        select new BranchClassEntity()
                        {

                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id
                            },
                            branch = new BranchEntity()
                            {
                                BranchID = u.branch_id
                            }

                        }).Distinct().ToList();

            return data;
        }


        public async Task<List<BranchClassEntity>> GetAllClassByCourseddl(long courseid, bool Isupdate = false)
        {
            var data = (from u in this.context.CLASS_DTL_MASTER
                        orderby u.CLASS_MASTER.class_name descending
                        where u.row_sta_cd == 1 && u.course_dtl_id == courseid && u.CLASS_MASTER.row_sta_cd == 1
                        select new BranchClassEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            Class = new ClassEntity()
                            {
                                ClassID = u.class_id,
                                ClassName = u.CLASS_MASTER.class_name,
                            },
                            isClass = u.is_class.HasValue ? u.is_class.Value : false,
                            Class_dtl_id = u.class_dtl_id,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).OrderByDescending(a => a.Class.ClassID).ToList();


            if (data?.Count == 0)
            {
                long CourseID = 0;
                var coursedata = this.context.COURSE_DTL_MASTER.Where(s => s.course_dtl_id == courseid && s.row_sta_cd == 1).FirstOrDefault();
                CourseID = coursedata == null ? CourseID : coursedata.course_id;
                var data1 = (from u in this.context.CLASS_MASTER
                             orderby u.class_name descending
                             where u.row_sta_cd == 1 && u.course_id == CourseID
                             select new BranchClassEntity()
                             {
                                 RowStatus = new RowStatusEntity()
                                 {
                                     RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                     RowStatusId = (int)u.row_sta_cd,
                                     RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                                 },
                                 Class = new ClassEntity()
                                 {
                                     ClassID = u.class_id,
                                     ClassName = u.class_name,
                                 },
                                 Transaction = new TransactionEntity() { TransactionId = u.trans_id },

                             }).OrderByDescending(a => a.Class.ClassID).ToList();

                return data1;
            }
            else
            {
                return data;
            }



        }

    }
}
