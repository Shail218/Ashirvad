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
    public class BranchCourse : ModelAccess, IBranchCourseAPI
    {

        public async Task<long> CheckCourse(int CourseDetailID, int CourseID,int BranchID)
        {
            long result;
            bool isExists = this.context.COURSE_DTL_MASTER.Where(s => (CourseDetailID == 0 || s.course_dtl_id != CourseDetailID) 
            && s.course_id == CourseID && s.branch_id == BranchID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<long> CourseMaintenance(BranchCourseEntity CourseInfo)
        {
            Model.COURSE_DTL_MASTER CourseMaster = new Model.COURSE_DTL_MASTER();
            if (CheckCourse((int)CourseInfo.course_dtl_id, (int)CourseInfo.course.CourseID, (int)CourseInfo.branch.BranchID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from course in this.context.COURSE_DTL_MASTER
                            where course.course_dtl_id == CourseInfo.course_dtl_id
                            select new
                            {
                                CourseMaster = course
                            }).FirstOrDefault();
                if (data == null)
                {
                    CourseMaster = new Model.COURSE_DTL_MASTER();
                    isUpdate = false;
                }
                else
                {
                    CourseMaster = data.CourseMaster;
                    CourseInfo.Transaction.TransactionId = data.CourseMaster.trans_id;
                }

                CourseMaster.course_id = CourseInfo.course.CourseID;                
                CourseMaster.branch_id = CourseInfo.branch.BranchID;
                CourseMaster.row_sta_cd = (int)Enums.RowStatus.Active;
                CourseMaster.is_course = CourseInfo.iscourse;               
                CourseMaster.trans_id = this.AddTransactionData(CourseInfo.Transaction);
                this.context.COURSE_DTL_MASTER.Add(CourseMaster);
                if (isUpdate)
                {
                    this.context.Entry(CourseMaster).State = System.Data.Entity.EntityState.Modified;
                }
                var result = this.context.SaveChanges();
                if (result > 0)
                {
                    CourseInfo.course_dtl_id = CourseMaster.course_dtl_id;
                    //var result2 = courseDetailMaintenance(courseInfo).Result;
                    return result > 0 ? CourseInfo.course_dtl_id : 0;
                }
                return this.context.SaveChanges() > 0 ? 1 : 0;
            }
            return -1;
        }

        public async Task<List<BranchCourseEntity>> GetAllCourse(long BranchID)
        {
            var data = (from u in this.context.COURSE_DTL_MASTER
                        .Include("COURSE_MASTER")
                        .Include("BRANCH_MASTER")
                        where (BranchID==0|| u.branch_id== BranchID) && u.row_sta_cd == 1 && u.is_course==true
                        select new BranchCourseEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            course = new CourseEntity()
                            {
                                CourseID = u.COURSE_MASTER.course_id,
                                CourseName = u.COURSE_MASTER.course_name
                            },
                            branch=new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            iscourse = u.is_course==true?true:false,
                           course_dtl_id=u.course_dtl_id,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            if (data.Count > 0)
            {
                data[0].BranchCourseData = (from u in this.context.COURSE_DTL_MASTER
                              .Include("COURSE_MASTER")                               
                              .Include("BRANCH_MASTER")                               
                                where (BranchID == 0 || u.branch_id == BranchID) && u.row_sta_cd == 1
                                select new BranchCourseEntity()
                                {
                                    branch = new BranchEntity()
                                    {
                                        BranchID = u.BRANCH_MASTER.branch_id,
                                        BranchName = u.BRANCH_MASTER.branch_name
                                    },
                                    
                                }).Distinct().ToList();
            }
            else
            {
                BranchCourseEntity entity = new BranchCourseEntity();
                entity.BranchCourseData = new List<BranchCourseEntity>();
                data.Add(entity);
            }
            return data;

        }


        public async Task<List<BranchCourseEntity>> GetCourseByCourseID(long CourseID)
        {
            var data = (from u in this.context.COURSE_DTL_MASTER
                       .Include("COURSE_MASTER")
                       
                        where u.row_sta_cd == 1 && u.branch_id == CourseID
                        select new BranchCourseEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            course = new CourseEntity()
                            {
                                CourseID = u.COURSE_MASTER.course_id,
                                CourseName = u.COURSE_MASTER.course_name
                            },
                            course_dtl_id=u.course_dtl_id,
                            iscourse = u.is_course==true?true:false,                            
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            return data;
        }
        public async Task<BranchCourseEntity> GetCoursebyID(long CourseID)
        {
            var data = (from u in this.context.COURSE_DTL_MASTER
                       .Include("course_MASTER")                       
                        where u.row_sta_cd == 1 && u.branch_id == CourseID
                        select new BranchCourseEntity()
                        {
                            course_dtl_id=u.course_dtl_id,
                            course = new CourseEntity()
                            {
                                CourseID = u.COURSE_MASTER.course_id,
                                CourseName = u.COURSE_MASTER.course_name
                            },
                            
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).FirstOrDefault();
            return data;
        }

        public bool RemoveCourse(long CourseID, string lastupdatedby)
        {
            var data = (from u in this.context.COURSE_DTL_MASTER
                        where u.branch_id == CourseID
                        select u).ToList();
            if (data != null)
            {
                foreach(var item in data)
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
