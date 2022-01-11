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

        public async Task<List<StudentEntity>> GetAllStudentsNameByStandard(long StdID)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        where u.std_id == StdID
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
                            StandardInfo = new StandardEntity() { StandardID = u.std_id, Standard = u.STD_MASTER.standard },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening },
                        }).FirstOrDefault();
            return data;
        }

        public async Task<List<StudentEntity>> GetStudentContent(long stdID, long branchID, long batchID)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        where branchID == 0 || u.branch_id == branchID
                        && (0 == batchID || u.batch_time == batchID) && u.std_id == stdID && u.row_sta_cd == 1
                        select new StudentEntity()
                        {
                            StudentID = u.student_id,
                            Name = u.first_name + " " + u.middle_name + " " + u.last_name,
                            Address = u.address,
                            ContactNo = u.contact_no,
                            FilePath = u.file_path,
                            StandardInfo = new StandardEntity() { StandardID = u.std_id, Standard = u.STD_MASTER.standard },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening },
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
                            AttendanceDatetxt = "Absent"
                        }).OrderByDescending(a => a.AttendanceID).ToList();
            }
            return data;
        }
    }
}
