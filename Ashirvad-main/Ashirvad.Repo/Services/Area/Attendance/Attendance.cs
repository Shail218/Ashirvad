using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Attendance;
using Ashirvad.Repo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Attendance
{
    public class Attendance : ModelAccess, IAttendanceAPI
    {
        public async Task<long> AttendanceMaintenance(AttendanceEntity attendanceInfo)
        {
            Model.ATTENDANCE_HDR attendanceMaster = new Model.ATTENDANCE_HDR();
            bool isUpdate = true;
            try
            {
                var data = (from atd in this.context.ATTENDANCE_HDR
                            where atd.attendance_hdr_id == attendanceInfo.AttendanceID
                            select atd).FirstOrDefault();
                if (data == null)
                {
                    attendanceMaster = new Model.ATTENDANCE_HDR();
                    isUpdate = false;
                }
                else
                {
                    attendanceMaster = data;
                    attendanceInfo.Transaction.TransactionId = data.trans_id;
                }

                attendanceMaster.std_id = attendanceInfo.Standard.StandardID;
                attendanceMaster.batch_time_type = attendanceInfo.BatchTypeID;
                attendanceMaster.attendance_dt = attendanceInfo.AttendanceDate;
                attendanceMaster.branch_id = attendanceInfo.Branch.BranchID;
                attendanceMaster.row_sta_cd = attendanceInfo.RowStatus.RowStatusId;
                attendanceMaster.trans_id = this.AddTransactionData(attendanceInfo.Transaction);
                this.context.ATTENDANCE_HDR.Add(attendanceMaster);
                if (isUpdate)
                {
                    this.context.Entry(attendanceMaster).State = System.Data.Entity.EntityState.Modified;
                }

                attendanceInfo.AttendanceID = attendanceMaster.attendance_hdr_id;
                var detail = (from dtl in this.context.ATTENDANCE_DTL
                              where dtl.attd_hdr_id == attendanceInfo.AttendanceID
                              select dtl).ToList();
                if (detail?.Count > 0)
                {
                    this.context.ATTENDANCE_DTL.RemoveRange(detail);
                }

                foreach (var item in attendanceInfo.AttendanceDetail)
                {
                    ATTENDANCE_DTL dtl = new ATTENDANCE_DTL()
                    {
                        absent_fg = item.IsAbsent ? 1 : 0,
                        present_fg = item.IsPresent ? 1 : 0,
                        attd_hdr_id = attendanceInfo.AttendanceID,
                        remarks = item.Remarks,
                        student_id = item.Student.StudentID
                    };
                    this.context.ATTENDANCE_DTL.Add(dtl);
                }
            }
            catch(Exception ex)
            {

            }
            return this.context.SaveChanges() > 0 ? attendanceMaster.attendance_hdr_id : 0;
        }

        public async Task<List<StudentEntity>> GetAllStudentByBranchStdBatch(long branchID, long stdID, int batchID)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        where u.branch_id == branchID && u.STD_MASTER.std_id == stdID && u.batch_time == batchID
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            StudentID = u.student_id,
                            Address = u.address,
                            DOB = u.dob,
                            FirstName = u.first_name,
                            LastName = u.last_name,
                            MiddleName = u.middle_name,
                            ContactNo = u.contact_no,
                            LastYearResult = u.last_yr_result,
                            LastYearClassName = u.last_yr_class_name,
                            Grade = u.grade,
                            AdmissionDate = u.admission_date,
                            GrNo = u.gr_no,
                            SchoolTime = u.school_time,
                            StudentImgByte = u.stud_img,

                            //StudImage = u.stud_img.Length > 0 ? Convert.ToBase64String(u.stud_img) : "",
                            StandardInfo = new StandardEntity() { StandardID = u.std_id, Standard = u.STD_MASTER.standard },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening },
                            StudentMaint = new StudentMaint()
                            {
                                ParentName = maint.parent_name,
                                ParentID = maint.parent_id,
                                ContactNo = maint.contact_no,
                                FatherOccupation = maint.father_occupation,
                                MotherOccupation = maint.mother_occupation,
                                StudentID = maint.student_id
                            },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }

        public async Task<List<AttendanceEntity>> GetAllAttendanceByBranch(long branchID)
        {
            var data = (from u in this.context.ATTENDANCE_HDR
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        where u.branch_id == branchID
                        select new AttendanceEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Standard = new StandardEntity()
                            {
                                StandardID = u.STD_MASTER.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            AttendanceDate = u.attendance_dt,
                            AttendanceID = u.attendance_hdr_id,
                            BatchTypeID = u.batch_time_type,
                            BatchTypeText = u.batch_time_type == 1 ? "Morning" : u.batch_time_type == 2 ? "Afternoon" : "Evening",
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    long hdrID = data[idx].AttendanceID;
                    var detailInfo = (from dtl in this.context.ATTENDANCE_DTL
                                      join stu in this.context.STUDENT_MASTER on dtl.student_id equals stu.student_id
                                      where dtl.attd_hdr_id == hdrID
                                      select new AttendanceDetailEntity()
                                      {

                                          DetailID = dtl.attd_dtl_id,
                                          HeaderID = dtl.attd_hdr_id,
                                          IsAbsent = dtl.absent_fg == 1,
                                          IsPresent = dtl.present_fg == 1,
                                          Remarks = dtl.remarks,
                                          Student = new StudentEntity()
                                          {
                                              StudentID = stu.student_id,
                                              FirstName = stu.first_name,
                                              LastName = stu.last_name,
                                              GrNo = stu.gr_no
                                          }
                                      }).ToList();

                    data[idx].AttendanceDetail = detailInfo;
                }
            }
            return data;
        }

        public async Task<List<AttendanceEntity>> GetAllAttendanceByFilter(DateTime fromDate, DateTime toDate, long branchID, long stdID, int batchTimeID)
        {
            var data = (from u in this.context.ATTENDANCE_HDR
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        where (0 == branchID || u.branch_id == branchID)
                        && (0 == stdID || u.std_id == stdID)
                        && (0 == batchTimeID || u.batch_time_type == batchTimeID)
                        && (u.attendance_dt >= fromDate && u.attendance_dt <= toDate)
                        select new AttendanceEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Standard = new StandardEntity()
                            {
                                StandardID = u.STD_MASTER.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            AttendanceDate = u.attendance_dt,
                            AttendanceID = u.attendance_hdr_id,
                            BatchTypeID = u.batch_time_type,
                            BatchTypeText = u.batch_time_type == 1 ? "Morning" : u.batch_time_type == 2 ? "Afternoon" : "Evening",
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    long hdrID = data[idx].AttendanceID;
                    var detailInfo = (from dtl in this.context.ATTENDANCE_DTL
                                      join stu in this.context.STUDENT_MASTER on dtl.student_id equals stu.student_id
                                      where dtl.attd_hdr_id == hdrID
                                      select new AttendanceDetailEntity()
                                      {

                                          DetailID = dtl.attd_dtl_id,
                                          HeaderID = dtl.attd_hdr_id,
                                          IsAbsent = dtl.absent_fg == 1,
                                          IsPresent = dtl.present_fg == 1,
                                          Remarks = dtl.remarks,
                                          Student = new StudentEntity()
                                          {
                                              StudentID = stu.student_id,
                                              FirstName = stu.first_name,
                                              LastName = stu.last_name,
                                              GrNo = stu.gr_no
                                          }
                                      }).ToList();

                    data[idx].AttendanceDetail = detailInfo;
                }
            }
            return data;
        }

        public async Task<AttendanceEntity> GetAttendanceByID(long attendanceID)
        {
            var data = (from u in this.context.ATTENDANCE_HDR
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        where u.attendance_hdr_id == attendanceID
                        select new AttendanceEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Standard = new StandardEntity()
                            {
                                StandardID = u.STD_MASTER.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            AttendanceDate = u.attendance_dt,
                            AttendanceID = u.attendance_hdr_id,
                            BatchTypeID = u.batch_time_type,
                            BatchTypeText = u.batch_time_type == 1 ? "Morning" : u.batch_time_type == 2 ? "Afternoon" : "Evening",
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            if (data != null)
            {
                long hdrID = data.AttendanceID;
                var detailInfo = (from dtl in this.context.ATTENDANCE_DTL
                                  join stu in this.context.STUDENT_MASTER on dtl.student_id equals stu.student_id
                                  where dtl.attd_hdr_id == hdrID
                                  select new AttendanceDetailEntity()
                                  {

                                      DetailID = dtl.attd_dtl_id,
                                      HeaderID = dtl.attd_hdr_id,
                                      IsAbsent = dtl.absent_fg == 1,
                                      IsPresent = dtl.present_fg == 1,
                                      Remarks = dtl.remarks,
                                      Student = new StudentEntity()
                                      {
                                          StudentID = stu.student_id,
                                          FirstName = stu.first_name,
                                          LastName = stu.last_name,
                                          GrNo = stu.gr_no
                                      }
                                  }).ToList();

                data.AttendanceDetail = detailInfo;
            }
            return data;
        }

        public async Task<List<AttendanceEntity>> GetAttendanceByStudentID(long studentID)
        {
            var data = (from u in this.context.ATTENDANCE_HDR
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("ATTENDANCE_DTL")
                        join detail in this.context.ATTENDANCE_DTL on u.attendance_hdr_id equals detail.attd_hdr_id
                        join stu in this.context.STUDENT_MASTER on detail.student_id equals stu.student_id
                        where detail.student_id == studentID
                        select new AttendanceEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Standard = new StandardEntity()
                            {
                                StandardID = u.STD_MASTER.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            AttendanceDate = u.attendance_dt,
                            AttendanceID = u.attendance_hdr_id,
                            BatchTypeID = u.batch_time_type,
                            BatchTypeText = u.batch_time_type == 1 ? "Morning" : u.batch_time_type == 2 ? "Afternoon" : "Evening",
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            AttendanceDetail = new List<AttendanceDetailEntity>()
                            {
                                new AttendanceDetailEntity()
                                {
                                    DetailID = detail.attd_dtl_id,
                                    HeaderID = detail.attd_hdr_id,
                                    IsAbsent = detail.absent_fg == 1,
                                    IsPresent = detail.present_fg == 1,
                                    Remarks = detail.remarks,
                                    Student = new StudentEntity()
                                    {
                                        StudentID = stu.student_id,
                                          FirstName = stu.first_name,
                                          LastName = stu.last_name,
                                          GrNo = stu.gr_no
                                    }
                                }
                            }
                        }).ToList();

            return data;
        }

        public bool RemoveAttendance(long attendanceID, string lastupdatedby)
        {
            var data = (from u in this.context.ATTENDANCE_HDR
                        where u.attendance_hdr_id == attendanceID
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
