﻿using Ashirvad.Common;
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
    public class BranchSubject : ModelAccess, IBranchSubjectAPI
    {

        public async Task<long> CheckSubject(int SubjectDetailID, int SubjectID, int BranchID, int CourseDetailID, int ClassID)
        {
            long result;
            bool isExists = this.context.SUBJECT_DTL_MASTER.Where(s => (SubjectDetailID == 0 || s.subject_dtl_id != SubjectDetailID)
            && s.class_dtl_id == ClassID && s.subject_id == SubjectID && s.branch_id == BranchID && s.course_dtl_id == CourseDetailID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<long> SubjectMaintenance(BranchSubjectEntity SubjectInfo)
        {
            Model.SUBJECT_DTL_MASTER SubjectMaster = new Model.SUBJECT_DTL_MASTER();
            if (CheckSubject((int)SubjectInfo.Subject_dtl_id, (int)SubjectInfo.Subject.SubjectID, 
                (int)SubjectInfo.branch.BranchID, (int)SubjectInfo.BranchCourse.course_dtl_id, (int)SubjectInfo.Class_dtl_id).Result != -1)
            {
                bool isUpdate = true;
                var data = (from Subject in this.context.SUBJECT_DTL_MASTER
                            where Subject.subject_dtl_id == SubjectInfo.Subject_dtl_id
                            select new
                            {
                                SubjectMaster = Subject
                            }).FirstOrDefault();
                if (data == null)
                {
                    SubjectMaster = new Model.SUBJECT_DTL_MASTER();
                    isUpdate = false;
                }
                else
                {
                    SubjectMaster = data.SubjectMaster;
                    SubjectInfo.Transaction.TransactionId = data.SubjectMaster.trans_id;
                }

                SubjectMaster.subject_id = SubjectInfo.Subject.SubjectID;
                SubjectMaster.branch_id = SubjectInfo.branch.BranchID;
                SubjectMaster.row_sta_cd = (int)Enums.RowStatus.Active;
                SubjectMaster.is_subject = SubjectInfo.isSubject;
                SubjectMaster.course_dtl_id = SubjectInfo.BranchCourse.course_dtl_id;
                SubjectMaster.class_dtl_id = SubjectInfo.BranchClass.Class_dtl_id;
                SubjectMaster.trans_id = this.AddTransactionData(SubjectInfo.Transaction);
                this.context.SUBJECT_DTL_MASTER.Add(SubjectMaster);
                if (isUpdate)
                {
                    this.context.Entry(SubjectMaster).State = System.Data.Entity.EntityState.Modified;
                }
                var result = this.context.SaveChanges();
                if (result > 0)
                {
                    SubjectInfo.Subject_dtl_id = SubjectMaster.subject_dtl_id;
                    //var result2 = SubjectDetailMaintenance(SubjectInfo).Result;
                    return result > 0 ? SubjectInfo.Subject_dtl_id : 0;
                }
                return this.context.SaveChanges() > 0 ? 1 : 0;
            }
            return -1;
        }

        public async Task<List<BranchSubjectEntity>> GetAllSubject(long BranchID)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                        .Include("Subject_MASTER")
                        .Include("BRANCH_MASTER")
                        where (BranchID == 0 || u.branch_id == BranchID) && u.row_sta_cd == 1 && u.is_subject == true
                        select new BranchSubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Subject = new SuperAdminSubjectEntity()
                            {
                                SubjectID = u.SUBJECT_BRANCH_MASTER.subject_id,
                                SubjectName = u.SUBJECT_BRANCH_MASTER.subject_name
                            },
                            branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            isSubject = u.is_subject == true ? true : false,
                            Subject_dtl_id = u.subject_dtl_id,
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id,

                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id,

                            },
                            Class = new ClassEntity()
                            {
                                ClassID = u.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            if (data.Count > 0)
            {
                data[0].BranchSubjectData = (from u in this.context.SUBJECT_DTL_MASTER
                              .Include("Subject_MASTER")
                              .Include("BRANCH_MASTER")
                                             where (BranchID == 0 || u.branch_id == BranchID) && u.row_sta_cd == 1
                                             select new BranchSubjectEntity()
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
                                                 BranchClass = new BranchClassEntity()
                                                 {
                                                     Class_dtl_id = u.class_dtl_id,

                                                 },
                                                 Class = new ClassEntity()
                                                 {
                                                     ClassID = u.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                                     ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                                 },
                                             }).Distinct().ToList();
            }
            else
            {
                BranchSubjectEntity entity = new BranchSubjectEntity();
                entity.BranchSubjectData = new List<BranchSubjectEntity>();
                data.Add(entity);
            }
            return data;

        }


        public async Task<List<BranchSubjectEntity>> GetSubjectBySubjectID(long SubjectID, long BranchID,long CourseID)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                       .Include("Subject_MASTER")
                        where u.row_sta_cd == 1 && u.class_dtl_id == SubjectID && u.branch_id == BranchID && u.course_dtl_id== CourseID
                        select new BranchSubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Subject = new SuperAdminSubjectEntity()
                            {
                                SubjectID = u.SUBJECT_BRANCH_MASTER.subject_id,
                                SubjectName = u.SUBJECT_BRANCH_MASTER.subject_name
                            },
                            Subject_dtl_id = u.subject_dtl_id,
                            isSubject = u.is_subject == true ? true : false,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            if (data.Count > 0)
            {
                data[0].branchSubject = (from u in this.context.SUBJECT_DTL_MASTER
                              .Include("Subject_MASTER")
                              .Include("BRANCH_MASTER")
                                         where u.row_sta_cd == 1 && u.class_dtl_id == SubjectID && u.branch_id == BranchID
                                         select new BranchSubjectEntity()
                                         {
                                             BranchCourse = new BranchCourseEntity()
                                             {
                                                 course_dtl_id = u.course_dtl_id,
                                                 course = new CourseEntity()
                                                 {
                                                     CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                                 }
                                             },
                                             BranchClass = new BranchClassEntity()
                                             {
                                                 Class_dtl_id = u.class_dtl_id,
                                                 
                                             },
                                         }).FirstOrDefault();
            }
            return data;
        }
        public async Task<BranchSubjectEntity> GetSubjectbyID(long SubjectID)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                       .Include("Subject_MASTER")
                        where u.row_sta_cd == 1 && u.branch_id == SubjectID
                        select new BranchSubjectEntity()
                        {
                            Subject_dtl_id = u.subject_dtl_id,
                            Subject = new SuperAdminSubjectEntity()
                            {
                                SubjectID = u.SUBJECT_BRANCH_MASTER.subject_id,
                                SubjectName = u.SUBJECT_BRANCH_MASTER.subject_name
                            },

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).FirstOrDefault();
            return data;
        }

        public bool RemoveSubject(long CourseID, long ClassID, long BranchID, string lastupdatedby)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                        where u.branch_id == BranchID && u.course_dtl_id == CourseID && u.class_dtl_id==ClassID
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