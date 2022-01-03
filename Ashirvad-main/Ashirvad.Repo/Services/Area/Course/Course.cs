using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Course
{
    public class Course : ModelAccess, ICourseAPI
    {
        private readonly IBranchCourseAPI _BranchCourse;

        public Course(IBranchCourseAPI branchCourse)
        {
            this._BranchCourse = branchCourse;
        }
        public async Task<long> CheckCourse(string name, long Id)
        {
            long result;
            bool isExists = this.context.COURSE_MASTER.Where(s => (Id == 0 || s.course_id != Id) && s.course_name == name && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> CourseMaintenance(CourseEntity courseEntity)
        {
            Model.COURSE_MASTER courseMaster = new Model.COURSE_MASTER();
            if (CheckCourse(courseEntity.CourseName, courseEntity.CourseID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from course in this.context.COURSE_MASTER
                            where course.course_id == courseEntity.CourseID
                            select new
                            {
                                courseMaster = course
                            }).FirstOrDefault();
                if (data == null)
                {
                    courseMaster = new Model.COURSE_MASTER();
                    isUpdate = false;
                }
                else
                {
                    courseMaster = data.courseMaster;
                    courseEntity.Transaction.TransactionId = data.courseMaster.trans_id;
                }
                courseMaster.course_name = courseEntity.CourseName;
                courseMaster.row_sta_cd = courseEntity.RowStatus.RowStatusId;
                courseMaster.file_name = courseEntity.filename;
                courseMaster.file_path = courseEntity.filepath;
                courseMaster.trans_id = this.AddTransactionData(courseEntity.Transaction);
                this.context.COURSE_MASTER.Add(courseMaster);
                if (isUpdate)
                {
                    this.context.Entry(courseMaster).State = System.Data.Entity.EntityState.Modified;
                }
                var Result = this.context.SaveChanges();
                if (Result > 0)
                {
                    courseEntity.CourseID = courseMaster.course_id;
                    courseEntity.Transaction.TransactionId = courseMaster.trans_id;
                    CourseMasterMaintenance(courseEntity);
                }
                return Result > 0 ? courseEntity.CourseID : 0;
            }
            else
            {
                return -1;
            }
        }

        public async Task<CourseEntity> GetCourseByCourseID(long courseID)
        {
            var data = (from u in this.context.COURSE_MASTER
                        where u.course_id == courseID
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
                            filename = u.file_name,
                            filepath = u.file_path,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }

        public async Task<List<CourseEntity>> GetAllCourse()
        {
            var data = (from u in this.context.COURSE_MASTER
                        orderby u.course_id descending
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
                            filepath = "https://mastermind.org.in" + u.file_path,
                            filename = u.file_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },

                        }).ToList();

            return data;
        }

        public async Task<List<CourseEntity>> GetAllCustomCourse(DataTableAjaxPostModel model)
        {
            var Count = context.COURSE_MASTER.Where(a => a.row_sta_cd == 1).Count();
            var data = (from u in this.context.COURSE_MASTER
                        orderby u.course_id descending
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
                            filepath = "https://mastermind.org.in" + u.file_path,
                            filename = u.file_name,
                            Count = Count,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }

        public bool CheckHistory(long courseID)
        {
            bool Issuccess = true;
            Issuccess = this.context.COURSE_DTL_MASTER.Where(s => s.course_id == courseID && s.is_course == true && s.row_sta_cd == 1).FirstOrDefault() != null;
            if (Issuccess)
            {
                return false;
            }
            return true;
        }

        public bool RemoveCourse(long courseID, string lastupdatedby)
        {
            bool Isvalid = CheckHistory(courseID);
            if(Isvalid)
            {
                var data = (from u in this.context.COURSE_MASTER
                            where u.course_id == courseID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    return true;
                }
            }
          
            return false;
        }

        public async Task<long> CourseMasterMaintenance(CourseEntity courseEntity)
        {
            try
            {
                long result = 0;
                var data = (from course in this.context.COURSE_DTL_MASTER
                            select new BranchEntity
                            {
                                BranchID = course.branch_id
                            }).Distinct().ToList();

                BranchCourseEntity branchCourse = new BranchCourseEntity();
                branchCourse.course = new CourseEntity()
                {
                    CourseID = courseEntity.CourseID,
                    CourseName = courseEntity.CourseName
                };
                branchCourse.Transaction = new TransactionEntity();
                branchCourse.Transaction = courseEntity.Transaction;
                branchCourse.iscourse = false;
                branchCourse.RowStatus = new RowStatusEntity()
                {
                    RowStatus = Enums.RowStatus.Active
                };
                foreach (var item in data)
                {
                    branchCourse.branch = new BranchEntity()
                    {
                        BranchID = item.BranchID,

                    };
                    branchCourse.course_dtl_id = 0;
                    result = _BranchCourse.CourseMaintenance(branchCourse).Result;
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
