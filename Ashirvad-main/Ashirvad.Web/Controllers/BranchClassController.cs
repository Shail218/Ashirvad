﻿using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class BranchClassController : BaseController
    {

        BranchClassMaintenanceModel branchClass = new BranchClassMaintenanceModel();

        ResponseModel response = new ResponseModel();
        private readonly IClassService _ClassService;
        private readonly IBranchClassService _branchClassService;
        public BranchClassController(IClassService ClassService, IBranchClassService branchClassService)
        {
            _ClassService = ClassService;
            _branchClassService = branchClassService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> ClassMaintenance(long ClassID)
        {
            branchClass.BranchClassInfo = new BranchClassEntity();
            branchClass.BranchClassInfo.ClassData = new List<ClassEntity>();
            if (ClassID > 0)
            {
                var result = await _branchClassService.GetBranchClassByBranchClassID(ClassID, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                if (result.Count > 0)
                {
                    branchClass.BranchClassInfo = result[0].branchClass;
                }
                branchClass.BranchClassInfo.BranchClassData = result;

                branchClass.BranchClassInfo.IsEdit = true;
            }

            var ClassData = await _ClassService.GetAllClass();
            branchClass.BranchClassInfo.ClassData = ClassData.Data;

            var BranchClass = await _branchClassService.GetAllBranchClass(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branchClass.BranchClassData = BranchClass;
            return View("Index", branchClass);
        }

        [HttpPost]
        public async Task<JsonResult> SaveClassDetails(BranchClassEntity branchClass)
        {
            response.Status = false;


            BranchClassEntity branchClassEntity = new BranchClassEntity();

            long CourseDetailID = branchClass.BranchCourse.course_dtl_id;

            var List = JsonConvert.DeserializeObject<List<BranchClassEntity>>(branchClass.JsonData);
            foreach (var item in List)
            {
                branchClass.branch = new BranchEntity();
                branchClass.Class = new ClassEntity();
                branchClass.BranchCourse = new BranchCourseEntity();
                branchClass.branch.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
                branchClass.Transaction = GetTransactionData(item.Class_dtl_id > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                branchClass.Class_dtl_id = item.Class_dtl_id;
                branchClass.BranchCourse.course_dtl_id = CourseDetailID;
                branchClass.Class.ClassID = item.Class.ClassID;
                branchClass.isClass = item.isClass;
                branchClass = await _branchClassService.BranchClassMaintenance(branchClass);
                if ((long)branchClass.Data < 0)
                {
                    break;
                }
            }
            if ((long)branchClass.Data > 0)
            {
                response.Status = true;
                response.Message = branchClass.Class_dtl_id > 0 ? "Updated Successfully!!" : "Created Successfully!!";


            }
            else if ((long)branchClass.Data < 0)
            {
                response.Status = false;
                response.Message = "Already Exists!!";
            }
            else
            {
                response.Status = false;
                response.Message = branchClass.Class_dtl_id > 0 ? "Failed To Update!!" : "Failed To Create!!";
            }
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveClassDetail(long PackageRightID)
        {
            response.Status = false;
            response.Message = "Failed To Delete";
            var result = _branchClassService.RemoveBranchClass(PackageRightID, SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.Username);
            if (result)
            {
                response.Status = true;
                response.Message = "Successfully Deleted!!";
            }

            return Json(response);
        }

        public async Task<JsonResult> GetClassDDL(long CourseID)
        {
           
            try
            {
                var BranchCourse = await _branchClassService.GetAllBranchClass(SessionContext.Instance.LoginUser.BranchInfo.BranchID, CourseID);
                if (BranchCourse.Count > 0)
                {
                    return Json(BranchCourse);
                }
                else
                {
                    return Json(null);
                }

            }
            catch (Exception ex)
            {
                return Json(null);
            }

        }

    }
}