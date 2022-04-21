using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area
{
    public class Check_Delete : ModelAccess
    {
        ResponseModel model = new ResponseModel();
        public async Task<ResponseModel> check_delete_course(long branchID,long coursedetailid)
        {
            long total_count = 0;
            long count = 0;             
            string message = "";
            string total_message = "<br />";
            count = this.context.BATCH_MASTER.Where(s => ( s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Batch Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.STUDENT_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Student Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.FACULTY_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Faculty Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.CLASS_DTL_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.is_class == true).Count();
            total_count = total_count + count;
            message = count > 0 ? " Class Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.SUBJECT_DTL_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.is_subject == true).Count();
            total_count = total_count + count;
            message = count > 0 ? " Subject Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.FEE_STRUCTURE_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Student Fee Structure Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.ATTENDANCE_HDR.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Attendance Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.TEST_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Test Paper Entry = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.MARKS_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Marks Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.LINK_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.type == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Online Class Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.LINK_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.type == 2).Count();
            total_count = total_count + count;
            message = count > 0 ? " Youtube Video Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 2).Count();
            total_count = total_count + count;
            message = count > 0 ? " Library Book Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Library Video Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.PRACTICE_PAPER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Practice Paper Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.HOMEWORK_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Homework Master = " + count + "<br />" : "";
            total_message = total_message + message;

            model.Status =total_count > 0? false:true;
            model.Message = "This Course is Already active in " + total_message + " places. Please delete this course from that places first.";
            return model;
        }

        public async Task<ResponseModel> check_delete_class(long branchID, long classdetailid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string total_message = "<br />";
            count = this.context.BATCH_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Batch Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.STUDENT_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Student Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.FACULTY_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Faculty Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.SUBJECT_DTL_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1 && s.is_subject == true).Count();
            total_count = total_count + count;
            message = count > 0 ? " Subject Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.FEE_STRUCTURE_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Student Fee Structure Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.ATTENDANCE_HDR.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Attendance Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.TEST_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Test Paper Entry = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.MARKS_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Marks Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.LINK_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1 && s.type == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Online Class Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.LINK_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1 && s.type == 2).Count();
            total_count = total_count + count;
            message = count > 0 ? " Youtube Video Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.class_dtl_id == classdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 2).Count();
            total_count = total_count + count;
            message = count > 0 ? " Library Book Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.class_dtl_id == classdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Library Video Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.PRACTICE_PAPER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Practice Paper Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.HOMEWORK_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Homework Master = " + count + "<br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Class is Already active in " + total_message + " places. Please delete this class from that places first.";
            return model;
        }

        public async Task<ResponseModel> check_delete_subject(long branchID, long subjectdetailid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string total_message = "<br />";

            count = this.context.FACULTY_MASTER.Where(s => (s.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Faculty Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.TEST_MASTER.Where(s => (s.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Test Paper Entry = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.MARKS_MASTER.Where(s => (s.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Marks Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 2).Count();
            total_count = total_count + count;
            message = count > 0 ? " Library Book Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Library Video Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.PRACTICE_PAPER.Where(s => (s.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Practice Paper Master = " + count + "<br />" : "";
            total_message = total_message + message;

            count = this.context.HOMEWORK_MASTER.Where(s => (s.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).Count();
            total_count = total_count + count;
            message = count > 0 ? " Homework Master = " + count + "<br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Subject is Already active in " + total_message + " places. Please delete this subject from that places first.";
            return model;
        }

        public async Task<ResponseModel> check_remove_course(long branchID, long coursedetailid)
        {
            long total_count = 0;
            int count = 0;
            string message = "";
            string course = "";
            string total_message = "<br />";
            var data = this.context.BATCH_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            course = count > 0 ? data[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Batch Master = " + count + " Course Name = "+ course + "  <br />" : "";
            total_message = total_message + message;

            var data1 = this.context.STUDENT_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data1.Count();
            total_count = total_count + count;
            course = count > 0 ? data1[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Student Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data2 = this.context.FACULTY_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data2.Count();
            total_count = total_count + count;
            course = count > 0 ? data2[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Faculty Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data3 = this.context.CLASS_DTL_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.is_class == true).ToList();
            count = data3.Count();
            total_count = total_count + count;
            course = count > 0 ? data3[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Class Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data4 = this.context.SUBJECT_DTL_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.is_subject == true).ToList();
            count = data4.Count();
            total_count = total_count + count;
            course = count > 0 ? data4[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Subject Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data5 = this.context.FEE_STRUCTURE_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data5.Count();
            total_count = total_count + count;
            course = count > 0 ? data5[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Student Fee Structure Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data6 = this.context.ATTENDANCE_HDR.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data6.Count();
            total_count = total_count + count;
            course = count > 0 ? data6[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Attendance Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data7 = this.context.TEST_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data7.Count();
            total_count = total_count + count;
            course = count > 0 ? data7[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Test Paper Entry = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data8 = this.context.MARKS_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data8.Count();
            total_count = total_count + count;
            course = count > 0 ? data8[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Marks Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data9 = this.context.LINK_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.type == 1).ToList();
            count = data9.Count();
            total_count = total_count + count;
            course = count > 0 ? data9[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Online Class Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data10 = this.context.LINK_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.type == 2).ToList();
            count = data10.Count();
            total_count = total_count + count;
            course = count > 0 ? data10[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Youtube Video Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data11 = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 2).ToList();
            count = data11.Count();
            total_count = total_count + count;
            course = count > 0 ? data11[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Library Boook Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data12 = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 1).ToList();
            count = data12.Count();
            total_count = total_count + count;
            course = count > 0 ? data12[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Library Video Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data13 = this.context.PRACTICE_PAPER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data13.Count();
            total_count = total_count + count;
            course = count > 0 ? data13[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Practice Paper Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data14 = this.context.HOMEWORK_MASTER.Where(s => (s.branch_id == branchID) && s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data14.Count();
            total_count = total_count + count;
            course = count > 0 ? data14[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Homework Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Course is Already active in " + total_message + " places. Please delete this course from that places first.";
            return model;
        }

        public async Task<ResponseModel> check_remove_class(long branchID, long classdetailid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string cls = "";
            string total_message = "<br />";
            var data = this.context.BATCH_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            cls = count > 0 ? data[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Batch Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data1 = this.context.STUDENT_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data1.Count();
            total_count = total_count + count;
            cls = count > 0 ? data1[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Student Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data2 = this.context.FACULTY_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data2.Count();
            total_count = total_count + count;
            cls = count > 0 ? data2[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Faculty Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data3 = this.context.SUBJECT_DTL_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1 && s.is_subject == true).ToList();
            count = data3.Count();
            total_count = total_count + count;
            cls = count > 0 ? data3[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Subject Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data4 = this.context.FEE_STRUCTURE_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data4.Count();
            total_count = total_count + count;
            cls = count > 0 ? data4[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Student Fee Structure Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data5 = this.context.ATTENDANCE_HDR.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data5.Count();
            total_count = total_count + count;
            cls = count > 0 ? data5[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Attendance Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data6 = this.context.TEST_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data6.Count();
            total_count = total_count + count;
            cls = count > 0 ? data6[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Test Paper Entry = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data7 = this.context.MARKS_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data7.Count();
            total_count = total_count + count;
            cls = count > 0 ? data7[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Marks Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data8 = this.context.LINK_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1 && s.type == 1).ToList();
            count = data8.Count();
            total_count = total_count + count;
            cls = count > 0 ? data8[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Online Class Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data9 = this.context.LINK_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1 && s.type == 2).ToList();
            count = data9.Count();
            total_count = total_count + count;
            cls = count > 0 ? data9[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Youtube Video Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data10 = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.class_dtl_id == classdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 2).ToList();
            count = data10.Count();
            total_count = total_count + count;
            cls = count > 0 ? data10[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Library Book Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data11 = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.class_dtl_id == classdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 1).ToList();
            count = data11.Count();
            total_count = total_count + count;
            cls = count > 0 ? data11[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Library Video Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data12 = this.context.PRACTICE_PAPER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data12.Count();
            total_count = total_count + count;
            cls = count > 0 ? data12[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Practice Paper Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data13 = this.context.HOMEWORK_MASTER.Where(s => (s.branch_id == branchID) && s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data13.Count();
            total_count = total_count + count;
            cls = count > 0 ? data13[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Homework Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Class is Already active in " + total_message + " places. Please delete this class from that places first.";
            return model;
        }

        public async Task<ResponseModel> check_remove_subject(long branchID, long subjectdetailid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string subject = "";
            string total_message = "<br />";

            var data = this.context.FACULTY_MASTER.Where(s => (s.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            subject = count > 0 ? data[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Faculty Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data1 = this.context.TEST_MASTER.Where(s => (s.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).ToList();
            count = data1.Count();
            total_count = total_count + count;
            subject = count > 0 ? data1[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Test Paper Entry = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data2 = this.context.MARKS_MASTER.Where(s => (s.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).ToList();
            count = data2.Count();
            total_count = total_count + count;
            subject = count > 0 ? data2[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Marks Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data3 = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 2).ToList();
            count = data3.Count();
            total_count = total_count + count;
            subject = count > 0 ? data3[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Library Book Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data4 = this.context.LIBRARY_STD_MASTER.Where(s => (s.LIBRARY_MASTER.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 1).ToList();
            count = data4.Count();
            total_count = total_count + count;
            subject = count > 0 ? data4[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Library Video Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data5 = this.context.PRACTICE_PAPER.Where(s => (s.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).ToList();
            count = data5.Count();
            total_count = total_count + count;
            subject = count > 0 ? data5[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Practice Paper Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data6 = this.context.HOMEWORK_MASTER.Where(s => (s.branch_id == branchID) && s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).ToList();
            count = data6.Count();
            total_count = total_count + count;
            subject = count > 0 ? data6[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Homework Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Subject is Already active in " + total_message + " places. Please delete this subject from that places first.";
            return model;
        }

        public async Task<ResponseModel> check_remove_course_superadmin(long coursedetailid)
        {
            long total_count = 0;
            int count = 0;
            string message = "";
            string course = "";
            string total_message = "<br />";
            var data = this.context.BATCH_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            course = count > 0 ? data[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Batch Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data1 = this.context.STUDENT_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data1.Count();
            total_count = total_count + count;
            course = count > 0 ? data1[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Student Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data2 = this.context.FACULTY_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data2.Count();
            total_count = total_count + count;
            course = count > 0 ? data2[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Faculty Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data3 = this.context.CLASS_DTL_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.is_class == true).ToList();
            count = data3.Count();
            total_count = total_count + count;
            course = count > 0 ? data3[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Class Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data4 = this.context.SUBJECT_DTL_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.is_subject == true).ToList();
            count = data4.Count();
            total_count = total_count + count;
            course = count > 0 ? data4[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Subject Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data5 = this.context.FEE_STRUCTURE_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data5.Count();
            total_count = total_count + count;
            course = count > 0 ? data5[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Student Fee Structure Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data6 = this.context.ATTENDANCE_HDR.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data6.Count();
            total_count = total_count + count;
            course = count > 0 ? data6[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Attendance Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data7 = this.context.TEST_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data7.Count();
            total_count = total_count + count;
            course = count > 0 ? data7[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Test Paper Entry = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data8 = this.context.MARKS_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data8.Count();
            total_count = total_count + count;
            course = count > 0 ? data8[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Marks Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data9 = this.context.LINK_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.type == 1).ToList();
            count = data9.Count();
            total_count = total_count + count;
            course = count > 0 ? data9[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Online Class Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data10 = this.context.LINK_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1 && s.type == 2).ToList();
            count = data10.Count();
            total_count = total_count + count;
            course = count > 0 ? data10[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Youtube Video Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data11 = this.context.LIBRARY_STD_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 2).ToList();
            count = data11.Count();
            total_count = total_count + count;
            course = count > 0 ? data11[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Library Boook Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data12 = this.context.LIBRARY_STD_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 1).ToList();
            count = data12.Count();
            total_count = total_count + count;
            course = count > 0 ? data12[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Library Video Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data13 = this.context.PRACTICE_PAPER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data13.Count();
            total_count = total_count + count;
            course = count > 0 ? data13[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Practice Paper Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            var data14 = this.context.HOMEWORK_MASTER.Where(s => s.course_dtl_id == coursedetailid && s.row_sta_cd == 1).ToList();
            count = data14.Count();
            total_count = total_count + count;
            course = count > 0 ? data14[0].COURSE_DTL_MASTER.COURSE_MASTER.course_name : "";
            message = count > 0 ? " Homework Master = " + count + " Course Name = " + course + "  <br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Course is Already active in " + total_message + " places. Please delete this course from that places first.";
            return model;
        }

        public async Task<ResponseModel> check_remove_class_superadmin(long classdetailid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string cls = "";
            string total_message = "<br />";
            var data = this.context.BATCH_MASTER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            cls = count > 0 ? data[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Batch Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data1 = this.context.STUDENT_MASTER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data1.Count();
            total_count = total_count + count;
            cls = count > 0 ? data1[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Student Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data2 = this.context.FACULTY_MASTER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data2.Count();
            total_count = total_count + count;
            cls = count > 0 ? data2[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Faculty Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data3 = this.context.SUBJECT_DTL_MASTER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1 && s.is_subject == true).ToList();
            count = data3.Count();
            total_count = total_count + count;
            cls = count > 0 ? data3[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Subject Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data4 = this.context.FEE_STRUCTURE_MASTER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data4.Count();
            total_count = total_count + count;
            cls = count > 0 ? data4[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Student Fee Structure Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data5 = this.context.ATTENDANCE_HDR.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data5.Count();
            total_count = total_count + count;
            cls = count > 0 ? data5[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Attendance Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data6 = this.context.TEST_MASTER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data6.Count();
            total_count = total_count + count;
            cls = count > 0 ? data6[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Test Paper Entry = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data7 = this.context.MARKS_MASTER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data7.Count();
            total_count = total_count + count;
            cls = count > 0 ? data7[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Marks Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data8 = this.context.LINK_MASTER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1 && s.type == 1).ToList();
            count = data8.Count();
            total_count = total_count + count;
            cls = count > 0 ? data8[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Online Class Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data9 = this.context.LINK_MASTER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1 && s.type == 2).ToList();
            count = data9.Count();
            total_count = total_count + count;
            cls = count > 0 ? data9[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Youtube Video Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data10 = this.context.LIBRARY_STD_MASTER.Where(s => s.class_dtl_id == classdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 2).ToList();
            count = data10.Count();
            total_count = total_count + count;
            cls = count > 0 ? data10[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Library Book Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data11 = this.context.LIBRARY_STD_MASTER.Where(s => s.class_dtl_id == classdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 1).ToList();
            count = data11.Count();
            total_count = total_count + count;
            cls = count > 0 ? data11[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Library Video Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data12 = this.context.PRACTICE_PAPER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data12.Count();
            total_count = total_count + count;
            cls = count > 0 ? data12[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Practice Paper Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            var data13 = this.context.HOMEWORK_MASTER.Where(s => s.class_dtl_id == classdetailid && s.row_sta_cd == 1).ToList();
            count = data13.Count();
            total_count = total_count + count;
            cls = count > 0 ? data13[0].CLASS_DTL_MASTER.CLASS_MASTER.class_name : "";
            message = count > 0 ? " Homework Master = " + count + " Class Name = " + cls + "  <br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Class is Already active in " + total_message + " places. Please delete this class from that places first.";
            return model;
        }

        public async Task<ResponseModel> check_remove_subject_superadmin(long subjectdetailid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string subject = "";
            string total_message = "<br />";

            var data = this.context.FACULTY_MASTER.Where(s => s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            subject = count > 0 ? data[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Faculty Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data1 = this.context.TEST_MASTER.Where(s => s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).ToList();
            count = data1.Count();
            total_count = total_count + count;
            subject = count > 0 ? data1[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Test Paper Entry = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data2 = this.context.MARKS_MASTER.Where(s => s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).ToList();
            count = data2.Count();
            total_count = total_count + count;
            subject = count > 0 ? data2[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Marks Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data3 = this.context.LIBRARY_STD_MASTER.Where(s => s.subject_dtl_id == subjectdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 2).ToList();
            count = data3.Count();
            total_count = total_count + count;
            subject = count > 0 ? data3[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Library Book Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data4 = this.context.LIBRARY_STD_MASTER.Where(s => s.subject_dtl_id == subjectdetailid && s.LIBRARY_MASTER.row_sta_cd == 1 && s.LIBRARY_MASTER.library_type == 1).ToList();
            count = data4.Count();
            total_count = total_count + count;
            subject = count > 0 ? data4[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Library Video Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data5 = this.context.PRACTICE_PAPER.Where(s => s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).ToList();
            count = data5.Count();
            total_count = total_count + count;
            subject = count > 0 ? data5[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Practice Paper Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            var data6 = this.context.HOMEWORK_MASTER.Where(s => s.subject_dtl_id == subjectdetailid && s.row_sta_cd == 1).ToList();
            count = data6.Count();
            total_count = total_count + count;
            subject = count > 0 ? data6[0].SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : "";
            message = count > 0 ? " Homework Master = " + count + " Subject Name = " + subject + "  <br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Subject is Already active in " + total_message + " places. Please delete this subject from that places first.";
            return model;
        }

        public async Task<ResponseModel> check_remove_category_superadmin(long categoryid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string category = "";
            string total_message = "<br />";

            var data = this.context.LIBRARY_MASTER.Where(s => s.category_id == categoryid && s.row_sta_cd == 1 && s.library_type == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            category = count > 0 ? data[0].CATEGORY_MASTER.category_name : "";
            message = count > 0 ? " Library Video Master = " + count + " Category Name = " + category + "  <br />" : "";
            total_message = total_message + message;

            var data1 = this.context.LIBRARY_MASTER.Where(s => s.category_id == categoryid && s.row_sta_cd == 1 && s.library_type == 2).ToList();
            count = data1.Count();
            total_count = total_count + count;
            category = count > 0 ? data1[0].CATEGORY_MASTER.category_name : "";
            message = count > 0 ? " Library Book Master = " + count + " Category Name = " + category + "  <br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Category is Already active in " + total_message + " places. Please delete this Category from that places first.";
            return model;
        }
        public async Task<ResponseModel> check_remove_package_rights(long packageid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string category = "";
            string total_message = "<br />";

            var data = this.context.BRANCH_RIGHTS_MASTER.Where(s => s.package_id == packageid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            category = count > 0 ? data[0].BRANCH_MASTER.branch_name: "";
            message = count > 0 ? " Branch Rights Master = " + count + " Branch Name = " + category + "  <br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Pakage Rights is Already active in " + total_message + " places. Please delete this Package Rights from that places first.";
            return model;
        }
        public async Task<ResponseModel> check_remove_package(long packageid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string category = "";
            string total_message = "<br />";

            var data = this.context.BRANCH_RIGHTS_MASTER.Where(s => s.package_id == packageid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            category = count > 0 ? data[0].BRANCH_MASTER.branch_name: "";
            message = count > 0 ? " Branch Rights Master = " + count + " Branch Name = " + category + "  <br />" : "";
            total_message = total_message + message;

            var data1 = this.context.PACKAGE_RIGHTS_MASTER.Where(s => s.package_id == packageid && s.row_sta_cd == 1).ToList();
            count = data1.Count();
            total_count = total_count + count;
            category = count > 0 ? data1[0].PACKAGE_MASTER.package : "";
            message = count > 0 ? " Package Rights Master = " + count + "Package Name = " + category + "  <br />" : "";
            total_message = total_message + message;


            model.Status = total_count > 0 ? false : true;
            model.Message = "This Pakage is Already active in " + total_message + " places. Please delete this Package from that places first.";
            return model;
        }
        public async Task<ResponseModel> check_remove_role(long roleid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string category = "";
            string total_message = "<br />";

            var data = this.context.USER_RIGHTS_MASTER.Where(s => s.role_id == roleid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            category = count > 0 ? data[0].USER_DEF.username: "";
            message = count > 0 ? " User Rights Master = " + count + " User Name = " + category + "  <br />" : "";
            total_message = total_message + message;

            var data1 = this.context.ROLE_RIGHTS_MASTER.Where(s => s.role_id == roleid && s.row_sta_cd == 1).ToList();
            count = data1.Count();
            total_count = total_count + count;
            category = count > 0 ? data1[0].ROLE_MASTER.role_name : "";
            message = count > 0 ? " Role Rights Master = " + count + "Role Name = " + category + "  <br />" : "";
            total_message = total_message + message;


            model.Status = total_count > 0 ? false : true;
            model.Message = "This Role is Already active in " + total_message + " places. Please delete this Role from that places first.";
            return model;
        }
        public async Task<ResponseModel> check_remove_role_rights(long roleid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string category = "";
            string total_message = "<br />";

            var data = this.context.USER_RIGHTS_MASTER.Where(s => s.role_id == roleid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            category = count > 0 ? data[0].USER_DEF.username: "";
            message = count > 0 ? " User Rights Master = " + count + " User Name = " + category + "  <br />" : "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Role Rights is Already active in " + total_message + " places. Please delete this Role Rights from that places first.";
            return model;
        }
         public async Task<ResponseModel> check_remove_branch(long branchid)
        {
            long total_count = 0;
            long count = 0;
            string message = "";
            string category = "";
            string total_message = "<br />";

            var data = this.context.BRANCH_AGREEMENT.Where(s => s.branch_id == branchid && s.row_sta_cd == 1).ToList();
            count = data.Count();
            total_count = total_count + count;
            category = count > 0 ? data[0].BRANCH_MASTER.branch_name: "";
            total_message = total_message + message;

            model.Status = total_count > 0 ? false : true;
            model.Message = "This Branch is Already active in Agreement. Please inactive this Branch from Agreement first.";
            return model;
        }

    }
}
