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
        public async Task<ResponseModel> CourseMaintenance(BranchCourseEntity CourseInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try{
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
                    CourseMaster.row_sta_cd = CourseInfo.RowStatus.RowStatus == Enums.RowStatus.Active ? (int)Enums.RowStatus.Active : (int)Enums.RowStatus.Inactive;
                    CourseMaster.is_course = CourseInfo.iscourse;
                    CourseMaster.trans_id = CourseInfo.Transaction.TransactionId > 0 ? CourseInfo.Transaction.TransactionId : this.AddTransactionData(CourseInfo.Transaction);
                    this.context.COURSE_DTL_MASTER.Add(CourseMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(CourseMaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    var result = this.context.SaveChanges();
                    if (result > 0)
                    {
                        CourseInfo.course_dtl_id = CourseMaster.course_dtl_id;
                        responseModel.Data = CourseInfo;
                        //var result2 = courseDetailMaintenance(courseInfo).Result;
                        responseModel.Message = isUpdate == true ? "Course Updated Successfully." : "Course Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "Course Not Updated." : "Course Not Inserted.";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.Message = "Course Already Exists.";
                    responseModel.Status = false;
                }
            }
            catch(Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
          
            return responseModel;
        }
        public async Task<List<BranchCourseEntity>> GetAllCourse(long BranchID)
        {
           
            var data = (from u in this.context.COURSE_DTL_MASTER
                        .Include("COURSE_MASTER")
                        .Include("BRANCH_MASTER") orderby u.course_dtl_id descending
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
                        orderby u.course_dtl_id descending
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
        public ResponseModel RemoveCourse(long CourseID, string lastupdatedby)
        {
            Check_Delete check = new Check_Delete();
            ResponseModel model = new ResponseModel();
            string message = "";
            var data = (from u in this.context.COURSE_DTL_MASTER
                        where u.branch_id == CourseID
                        select u).ToList();
            if (data != null)
            {
                foreach(var item in data)
                {
                    var data_course = check.check_remove_course(CourseID, item.course_dtl_id).Result;
                    if(data_course.Status)
                    {
                        item.row_sta_cd = (int)Enums.RowStatus.Inactive;
                        item.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = item.trans_id, LastUpdateBy = lastupdatedby });
                        this.context.SaveChanges();

                    }
                    else
                    {
                        message = message + data_course.Message;
                    }
                  
                }
                model.Status = message == "" ? true : false;
                model.Message = message == "" ? "Course Deleted Successfully!!" : message;
               
                return model;
            }

            return model;
        }
        public async Task<List<BranchCourseEntity>> GetAllSelectedCourses(long BranchID)
        {
            var data = (from u in this.context.COURSE_DTL_MASTER
                        .Include("COURSE_MASTER")
                        .Include("BRANCH_MASTER")
                        where(u.branch_id==BranchID || BranchID==0)&& u.row_sta_cd == 1 && u.is_course == true
                        select new BranchCourseEntity()
                        {
                            course = new CourseEntity()
                            {
                                CourseID = u.COURSE_MASTER.course_id,
                                CourseName = u.COURSE_MASTER.course_name
                            },
                        }).Distinct().OrderByDescending(a => a.course.CourseName).ToList();
         
            return data;

        }

        public async Task<List<BranchCourseEntity>> GetAllCourseforExport(long BranchID)
        {

            var data = (from u in this.context.COURSE_DTL_MASTER
                        .Include("COURSE_MASTER")
                        .Include("BRANCH_MASTER")
                        orderby u.course_dtl_id descending
                        where (BranchID == 0 || u.branch_id == BranchID) && u.row_sta_cd == 1 && u.is_course == true
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
                            branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            iscourse = u.is_course == true ? true : false,
                            course_dtl_id = u.course_dtl_id,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
           
            return data;

        }

    }
}
