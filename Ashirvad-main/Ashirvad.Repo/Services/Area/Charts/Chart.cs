using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Charts
{
    public class Chart : ModelAccess, IChartAPI
    {
        public async Task<List<StudentEntity>> GetAllStudentsName(long branchID)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        where u.branch_id == branchID
                        select new StudentEntity()
                        {
                            StudentID = u.student_id,
                            Name = u.first_name + " " + u.last_name,
                        }).ToList();
            return data;
        }

        public async Task<List<StudentEntity>> GetAllStudentsNameByStandard(long StdID,long courseid)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        where u.class_dtl_id == StdID && u.course_dtl_id == courseid 
                        select new StudentEntity()
                        {
                            StudentID = u.student_id,
                            Name = u.first_name + " " + u.last_name,
                        }).ToList();
            return data;
        }

        public async Task<List<StandardEntity>> GetAllClassDDL(long BranchID)
        {

            var data = (from u in this.context.STD_MASTER
                        where u.row_sta_cd == 1 && u.branch_id == BranchID
                        select new StandardEntity()
                        {
                            StandardID = u.std_id,
                            Standard = u.standard
                        }).Distinct().ToList();
            return data;
        }

        public async Task<StudentEntity> GetStudentContentByID(long studentID)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        where u.student_id == studentID
                        select new StudentEntity()
                        {
                            StudentID = u.student_id,
                            Name = u.first_name + " " + u.middle_name + " " + u.last_name,
                            Address = u.address,
                            ContactNo = u.contact_no,
                            FilePath = u.file_path,
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3 },
                        }).FirstOrDefault();
            return data;
        }

        public async Task<List<StudentEntity>> GetStudentContent(long stdID, long branchID, long batchID)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        where branchID == 0 || u.branch_id == branchID
                        && (0 == batchID || u.batch_time == batchID) && u.class_dtl_id == stdID && u.row_sta_cd == 1
                        select new StudentEntity()
                        {
                            StudentID = u.student_id,
                            Name = u.first_name + " " + u.middle_name + " " + u.last_name,
                            Address = u.address,
                            ContactNo = u.contact_no,
                            FilePath = u.file_path,
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3 },
                        }).ToList();
            return data;
        }

        public async Task<List<BranchStandardEntity>> AllBranchStandardWithCountByBranch(long branchid)
        {
            List<BranchStandardEntity> branches = new List<BranchStandardEntity>();
            BranchStandardEntity standardEntity = new BranchStandardEntity();
            ArrayList data1 = new ArrayList();
            int count = (from u in this.context.STUDENT_MASTER
                         where (u.branch_id == branchid && u.row_sta_cd == 1 && u.batch_time == 1)
                         select new BranchStandardEntity()
                         {
                             branchid = u.branch_id
                         }).Count();
            data1.Add("Morning");
            data1.Add(count);
            standardEntity.data.Add(data1);
            ArrayList data2 = new ArrayList();
            int count1 = (from u in this.context.STUDENT_MASTER
                          where (u.branch_id == branchid && u.row_sta_cd == 1 && u.batch_time == 2)
                          select new BranchStandardEntity()
                          {
                              branchid = u.branch_id
                          }).Count();
            data2.Add("Afternoon");
            data2.Add(count1);
            standardEntity.data.Add(data2);
            ArrayList data3 = new ArrayList();
            int count2 = (from u in this.context.STUDENT_MASTER
                          where (u.branch_id == branchid && u.row_sta_cd == 1 && u.batch_time == 3)
                          select new BranchStandardEntity()
                          {
                              branchid = u.branch_id
                          }).Count();
            data3.Add("Evening");
            data3.Add(count2);
            standardEntity.data.Add(data3);
            ArrayList data4 = new ArrayList();
            int count3 = (from u in this.context.STUDENT_MASTER
                          where (u.branch_id == branchid && u.row_sta_cd == 1 && u.batch_time == 4)
                          select new BranchStandardEntity()
                          {
                              branchid = u.branch_id
                          }).Count();
            data4.Add("Morning2");
            data4.Add(count3);
            standardEntity.data.Add(data4);
            ArrayList data5 = new ArrayList();
            int count4 = (from u in this.context.STUDENT_MASTER
                          where (u.branch_id == branchid && u.row_sta_cd == 1 && u.batch_time == 5)
                          select new BranchStandardEntity()
                          {
                              branchid = u.branch_id
                          }).Count();
            data5.Add("Afternoon2");
            data5.Add(count4);
            standardEntity.data.Add(data5);
            ArrayList data6 = new ArrayList();
            int count5 = (from u in this.context.STUDENT_MASTER
                          where (u.branch_id == branchid && u.row_sta_cd == 1 && u.batch_time == 6)
                          select new BranchStandardEntity()
                          {
                              branchid = u.branch_id
                          }).Count();
            data6.Add("Evening2");
            data6.Add(count5);
            standardEntity.data.Add(data6);
            ArrayList data7 = new ArrayList();
            int count6 = (from u in this.context.STUDENT_MASTER
                          where (u.branch_id == branchid && u.row_sta_cd == 1 && u.batch_time == 7)
                          select new BranchStandardEntity()
                          {
                              branchid = u.branch_id
                          }).Count();
            data7.Add("Morning3");
            data7.Add(count6);
            standardEntity.data.Add(data7);
            ArrayList data8 = new ArrayList();
            int count7 = (from u in this.context.STUDENT_MASTER
                          where (u.branch_id == branchid && u.row_sta_cd == 1 && u.batch_time == 8)
                          select new BranchStandardEntity()
                          {
                              branchid = u.branch_id
                          }).Count();
            data8.Add("Afternoon3");
            data8.Add(count7);
            standardEntity.data.Add(data8);
            ArrayList data9 = new ArrayList();
            int count8 = (from u in this.context.STUDENT_MASTER
                          where (u.branch_id == branchid && u.row_sta_cd == 1 && u.batch_time == 9)
                          select new BranchStandardEntity()
                          {
                              branchid = u.branch_id
                          }).Count();
            data9.Add("Evening3");
            data9.Add(count8);
            standardEntity.data.Add(data9);
            standardEntity.id = "Batch";
            branches.Add(standardEntity);
            return branches;
        }

        public async Task<List<AttendanceEntity>> GetStudentAttendanceDetails(long studentID, long type)
        {
            List<AttendanceEntity> data = new List<AttendanceEntity>();
            if (type == 0)
            {
                data = (from u in this.context.ATTENDANCE_HDR
                        join t in this.context.ATTENDANCE_DTL on u.attendance_hdr_id equals t.attd_hdr_id
                        where t.student_id == studentID && u.row_sta_cd == 1
                        select new AttendanceEntity()
                        {
                            AttendanceID = u.attendance_hdr_id,
                            AttendanceDate = u.attendance_dt,
                            AttendanceRemarks = u.attendance_remarks,
                            AttendanceDatetxt = t.present_fg == 1 ? "Present" : "Absent"
                        }).OrderByDescending(a => a.AttendanceID).ToList();
            }
            else if (type == 1)
            {
                data = (from u in this.context.ATTENDANCE_HDR
                        join t in this.context.ATTENDANCE_DTL on u.attendance_hdr_id equals t.attd_hdr_id
                        where t.student_id == studentID && t.present_fg == 1 && u.row_sta_cd == 1
                        select new AttendanceEntity()
                        {
                            AttendanceID = u.attendance_hdr_id,
                            AttendanceDate = u.attendance_dt,
                            AttendanceRemarks = u.attendance_remarks,
                            AttendanceDatetxt = "Present"
                        }).OrderByDescending(a => a.AttendanceID).ToList();
            }
            else if (type == 2)
            {
                data = (from u in this.context.ATTENDANCE_HDR
                        join t in this.context.ATTENDANCE_DTL on u.attendance_hdr_id equals t.attd_hdr_id
                        where t.student_id == studentID && t.absent_fg == 1 && u.row_sta_cd == 1
                        select new AttendanceEntity()
                        {
                            AttendanceID = u.attendance_hdr_id,
                            AttendanceDate = u.attendance_dt,
                            AttendanceRemarks = u.attendance_remarks,
                            AttendanceDatetxt = "Absent"
                        }).OrderByDescending(a => a.AttendanceID).ToList();
            }
            return data;
        }
    }
}
