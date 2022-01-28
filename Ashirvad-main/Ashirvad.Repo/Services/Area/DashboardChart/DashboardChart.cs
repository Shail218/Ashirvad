using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.DataAcceessAPI.Area.DashboardChart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.DashboardChart
{
    public class DashboardChart : ModelAccess, IDashboardChartAPI
    {
        public async Task<List<ChartBranchEntity>> AllBranchWithCount()
        {
            var data = (from u in this.context.BRANCH_MASTER
                        join s in this.context.STUDENT_MASTER on u.branch_id equals s.branch_id
                        where (u.branch_id != 1 && u.row_sta_cd == 1)
                        select new ChartBranchEntity()
                        {
                            name = u.branch_name,
                            drilldown = u.branch_name,
                            branchid = u.branch_id
                        }).Distinct().ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    item.y = (from u in this.context.STUDENT_MASTER
                              where (u.branch_id == item.branchid && u.row_sta_cd == 1)
                              select new ChartBranchEntity()
                              {
                                  name = u.first_name
                              }).Distinct().Count();
                    item.branchstandardlist = AllBranchStandardWithCount(item);
                }
            }
            return data;
        }

        public List<BranchStandardEntity> AllBranchStandardWithCount(ChartBranchEntity chart)
        {
            List<BranchStandardEntity> branches = new List<BranchStandardEntity>();
            BranchStandardEntity standardEntity = new BranchStandardEntity();
            var data = (from u in this.context.STUDENT_MASTER
                       .Include("STD_MASTER")
                        where (u.branch_id == chart.branchid && u.row_sta_cd == 1)
                        select new BranchStandardEntity()
                        {
                            name = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                            branchid = u.branch_id
                        }).Distinct().ToArray();
            foreach (var item in data)
            {
                ArrayList data1 = new ArrayList();
                int count = (from u in this.context.STUDENT_MASTER
                       .Include("STD_MASTER")
                             where (u.branch_id == chart.branchid && u.row_sta_cd == 1 && u.CLASS_DTL_MASTER.CLASS_MASTER.class_name == item.name)
                             select new BranchStandardEntity()
                             {
                                 branchid = u.branch_id
                             }).Count();
                data1.Add("Standard " + item.name);
                data1.Add(count);
                standardEntity.data.Add(data1);
            }
            standardEntity.id = chart.name;
            branches.Add(standardEntity);
            return branches;
        }

        public async Task<List<BranchStandardEntity>> AllBranchStandardWithCountByBranch(long branchid)
        {
            List<BranchStandardEntity> branches = new List<BranchStandardEntity>();
            BranchStandardEntity standardEntity = new BranchStandardEntity();
            var data = (from u in this.context.STUDENT_MASTER
                       .Include("STD_MASTER")
                        where (u.branch_id == branchid && u.row_sta_cd == 1)
                        select new BranchStandardEntity()
                        {
                            name = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                            branchid = u.branch_id,
                            id = u.BRANCH_MASTER.branch_name
                        }).Distinct().ToArray();
            foreach (var item in data)
            {
                ArrayList data1 = new ArrayList();
                int count = (from u in this.context.STUDENT_MASTER
                       .Include("STD_MASTER")
                             where (u.branch_id == branchid && u.row_sta_cd == 1 && u.CLASS_DTL_MASTER.CLASS_MASTER.class_name == item.name)
                             select new BranchStandardEntity()
                             {
                                 branchid = u.branch_id
                             }).Count();
                data1.Add("Standard " + item.name);
                data1.Add(count);
                standardEntity.data.Add(data1);
                standardEntity.id = item.id;
            }
            branches.Add(standardEntity);
            return branches;
        }

        public async Task<List<DataPoints>> GetStudentAttendanceDetails(long studentid)
        {
            DataPoints points = new DataPoints();
            List<DataPoints> list = new List<DataPoints>();

            decimal totalattendancecount = this.context.ATTENDANCE_DTL.Where(s => s.student_id == studentid && s.ATTENDANCE_HDR.row_sta_cd == 1).Count();

            decimal presentcount = this.context.ATTENDANCE_DTL.Where(s => s.student_id == studentid && s.present_fg == 1 && s.ATTENDANCE_HDR.row_sta_cd == 1).Count();
            decimal PresentDecimal = (presentcount / totalattendancecount) * 100;
            points = new DataPoints();
            points.label = "Present";
            points.y = Convert.ToInt32(PresentDecimal);
            list.Add(points);

            decimal absentcount = this.context.ATTENDANCE_DTL.Where(s => s.student_id == studentid && s.absent_fg == 1 && s.ATTENDANCE_HDR.row_sta_cd == 1).Count();
            decimal AbsentDecimal = (absentcount / totalattendancecount) * 100;
            points = new DataPoints();
            points.label = "Absent";
            points.y = Convert.ToInt32(AbsentDecimal);
            list.Add(points);

            return list;
        }

        public async Task<List<ChartBranchEntity>> GetTotalCountList(long studentid)
        {
            ChartBranchEntity entity = new ChartBranchEntity();
            List<ChartBranchEntity> list = new List<ChartBranchEntity>();

            entity = new ChartBranchEntity();
            entity.y = this.context.ATTENDANCE_DTL.Where(s => s.student_id == studentid && s.ATTENDANCE_HDR.row_sta_cd == 1).Count();
            entity.name = "Total Days";
            list.Add(entity);

            entity = new ChartBranchEntity();
            entity.y = this.context.ATTENDANCE_DTL.Where(s => s.student_id == studentid && s.present_fg == 1 && s.ATTENDANCE_HDR.row_sta_cd == 1).Count();
            entity.name = "Present";
            list.Add(entity);

            entity = new ChartBranchEntity();
            entity.y = this.context.ATTENDANCE_DTL.Where(s => s.student_id == studentid && s.absent_fg == 1 && s.ATTENDANCE_HDR.row_sta_cd == 1).Count();
            entity.name = "Absent";
            list.Add(entity);

            return list;
        }

        public async Task<List<DataPoints>> GetHomeworkByStudent(long branchid, long studentid)
        {
            DataPoints point = new DataPoints();
            List<DataPoints> list = new List<DataPoints>();
            point.Days = 0;
            var data = (from u in this.context.SUBJECT_MASTER
                        where (u.branch_id == branchid && u.row_sta_cd == 1)
                        select new ChartBranchEntity()
                        {
                            name = u.subject,
                            branchid = u.subject_id
                        }).ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    item.branchstandardlist = (from u in this.context.HOMEWORK_MASTER
                                               join h in this.context.HOMEWORK_MASTER_DTL on u.homework_id equals h.homework_id
                                               where (h.STUDENT_MASTER.student_id == studentid && u.row_sta_cd == 1 && u.std_id == item.branchid)
                                               select new BranchStandardEntity()
                                               {
                                                   name = u.SUBJECT_MASTER.subject,
                                                   branchid = u.sub_id
                                               }).Distinct().ToList();
                    if (item.branchstandardlist.Count > 0)
                    {
                        foreach (var item1 in item.branchstandardlist)
                        {
                            point = new DataPoints();
                            point.label = item1.name;
                            point.id = item1.branchid;
                            point.y = item.branchstandardlist.Count;
                            point.Days += Convert.ToInt32(point.y);
                            list.Add(point);
                        }
                    }
                }

            }
            return list;
        }

        public async Task<List<TestDataPoints>> GetTestdetailsByStudent(long branchid, long studentid)
        {
            TestDataPoints point = new TestDataPoints();
            List<TestDataPoints> list = new List<TestDataPoints>();
            decimal totalMarks = 0;
            decimal totalAchieveMarks = 0;
            var data = (from u in this.context.SUBJECT_MASTER
                        where (u.branch_id == branchid && u.row_sta_cd == 1)
                        select new ChartBranchEntity()
                        {
                            name = u.subject,
                            branchid = u.subject_id
                        }).ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    try
                    {
                        item.branchstandardlist = (from u in this.context.TEST_MASTER
                                                   join t in this.context.MARKS_MASTER on u.test_id equals t.test_id
                                                   where (t.STUDENT_MASTER.student_id == studentid && u.row_sta_cd == 1 && u.class_dtl_id == item.branchid)
                                                   select new BranchStandardEntity()
                                                   {
                                                       name = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                                       branchid = u.subject_dtl_id.HasValue? u.subject_dtl_id.Value:0,
                                                       totalmarks = u.total_marks,
                                                       achievemarks = t.achive_marks
                                                   }).Distinct().ToList();
                        if (item.branchstandardlist.Count > 0)
                        {
                            foreach (var item1 in item.branchstandardlist)
                            {
                                point = new TestDataPoints();
                                point.label = item1.name;
                                point.id = item1.branchid;
                                point.TotalMarks = item1.totalmarks;
                                point.AchieveMarks = IsNumeric(item1.achievemarks);
                                point.y = Math.Round((point.AchieveMarks / point.TotalMarks) * 100, 2);
                                totalMarks += decimal.Parse(point.TotalMarks.ToString());
                                totalAchieveMarks += decimal.Parse(point.AchieveMarks.ToString());
                                point.Days = Math.Round((totalAchieveMarks / totalMarks) * 100, 2).ToString();
                                list.Add(point);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return list;
        }

        public double IsNumeric(string strNumber)
        {
            if (string.IsNullOrEmpty(strNumber))
            {
                return 0;
            }
            else
            {
                int numberOfChar = strNumber.Count();
                if (numberOfChar > 0)
                {
                    bool r = strNumber.All(char.IsDigit);
                    return double.Parse(strNumber);
                }
                else
                {
                    return 0;
                }
            }
        }

        public async Task<List<MarksEntity>> GetTestDetailsByStudent(DataTableAjaxPostModel model, long studentid, long subjectid)
        {
            int count = this.context.MARKS_MASTER.Where(s => s.row_sta_cd == 1 && s.student_id == studentid && s.subject_id == subjectid).Distinct().Count();
            try
            {
                var data = (from u in this.context.MARKS_MASTER.Include("TEST_MASTER")
                            where u.row_sta_cd == 1 && u.student_id == studentid && u.subject_id == subjectid
                            select new MarksEntity()
                            {
                                testEntityInfo = new TestEntity()
                                {
                                    TestDate = u.TEST_MASTER.test_dt,
                                    Marks = u.TEST_MASTER.total_marks
                                },
                                SubjectInfo = new SubjectEntity()
                                {
                                    Subject = u.SUBJECT_MASTER.subject,
                                    SubjectID = u.subject_id
                                },
                                Count = count,
                                MarksID = u.marks_id,
                                AchieveMarks = u.achive_marks
                            }).Distinct()
                            .OrderByDescending(a => a.MarksID)
                            .Skip(model.start)
                            .Take(model.length)
                            .ToList();
                var data2 = (from u in data
                             where model.search.value == null
                                || model.search.value == ""
                                || u.testEntityInfo.TestDate.ToString().ToLower().Contains(model.search.value)
                                || u.testEntityInfo.Marks.ToString().ToLower().Contains(model.search.value)
                                || u.SubjectInfo.Subject.ToString().ToLower().Contains(model.search.value)
                                || u.AchieveMarks.ToString().ToLower().Contains(model.search.value)
                             select new MarksEntity()
                             {
                                 testEntityInfo = new TestEntity()
                                 {
                                     TestDate = u.testEntityInfo.TestDate,
                                     Marks = u.testEntityInfo.Marks
                                 },
                                 SubjectInfo = new SubjectEntity()
                                 {
                                     Subject = u.SubjectInfo.Subject,
                                     SubjectID = u.SubjectInfo.SubjectID
                                 },
                                 Count = count,
                                 AchieveMarks = u.AchieveMarks,
                                 Percentage = Math.Round(Convert.ToDouble(u.AchieveMarks) * 100.0 / u.testEntityInfo.Marks, 2).ToString()
                             }).ToList();
                return data2;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<List<HomeworkDetailEntity>> GetHomeworkDetailsByStudent(DataTableAjaxPostModel model, long studentid, long subjectid)
        {
            int count = this.context.HOMEWORK_MASTER_DTL.Where(s => s.stud_id == studentid).Distinct().Count();
            var data = (from u in this.context.HOMEWORK_MASTER_DTL.Include("HOMEWORK_MASTER")
                        where (u.stud_id == studentid)
                        select new HomeworkDetailEntity()
                        {
                            HomeworkDetailID = u.homework_id,
                            Count = count
                        }).Distinct()
                        .OrderByDescending(a => a.HomeworkDetailID)
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    var a = (from z in this.context.HOMEWORK_MASTER_DTL
                             where z.homework_id == item.HomeworkDetailID && z.HOMEWORK_MASTER.sub_id == subjectid && (model.search.value == null
                                || model.search.value == ""
                                || z.remarks.ToLower().Contains(model.search.value)
                                || z.HOMEWORK_MASTER.SUBJECT_MASTER.subject.ToLower().Contains(model.search.value)
                                || z.HOMEWORK_MASTER.homework_dt.ToString().ToLower().Contains(model.search.value))
                             select z).FirstOrDefault();
                    if (a != null)
                    {
                        item.StatusText = a.status == (int)Enums.HomeWorkStatus.Done ? "Done" : "Pending";
                        item.Remarks = a.remarks;
                        item.HomeworkEntity = new HomeworkEntity()
                        {
                            HomeworkDate = a.HOMEWORK_MASTER.homework_dt,
                            SubjectName = a.HOMEWORK_MASTER.SUBJECT_MASTER.subject
                        };
                    }
                    else
                    {
                        data = new List<HomeworkDetailEntity>();
                    }
                }
            }
            return data;
        }
    }
}
