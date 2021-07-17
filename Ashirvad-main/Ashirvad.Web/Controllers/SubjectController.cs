﻿using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class SubjectController : BaseController
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        // GET: Subject
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SubjectMaintenance(long branchID)
        {
            long subID = branchID;
            SubjectMaintenanceModel branch = new SubjectMaintenanceModel();
            if (subID > 0)
            {
                var result = await _subjectService.GetSubjectByIDAsync(subID);
                branch.SubjectInfo = result;
            }

            var branchData = await _subjectService.GetAllSubjects(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.SubjectData = branchData;

            return View("Index", branch);
        }

        public async Task<ActionResult> EditSubject(long subjectID, long branchID)
        {
            SubjectMaintenanceModel branch = new SubjectMaintenanceModel();
            if (subjectID > 0)
            {
                var result = await _subjectService.GetSubjectByIDAsync(subjectID);
                branch.SubjectInfo = result;
            }

            if (branchID > 0)
            {
                var result = await _subjectService.GetAllSubjects(branchID);
                branch.SubjectData = result;
            }

            var branchData = await _subjectService.GetAllSubjects();
            branch.SubjectData = branchData;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveSubject(SubjectEntity branch)
        {
            branch.Transaction = GetTransactionData(branch.SubjectID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            branch.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _subjectService.SubjectMaintenance(branch);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveSubject(long subjectID)
        {
            var result = _subjectService.RemoveSubject(subjectID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> SubjectData()
        {
            var branchData = await _subjectService.GetAllSubjects();
            return Json(branchData);
        }

        public async Task<JsonResult> SubjectDataByBranch(long branchID)
        {
            var branchData = await _subjectService.GetAllSubjects(branchID);
            return Json(branchData);
        }
    }
}