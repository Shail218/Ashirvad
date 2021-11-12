﻿using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.SuperAdminSubject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class SuperAdminSubjectController : BaseController
    {

        private readonly ISuperAdminSubjectService _subjectService;
        public SuperAdminSubjectController(ISuperAdminSubjectService subjectservice)
        {
            _subjectService = subjectservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SubjectMaintenance(long subjectID)
        {
            SuperAdminSubjectModel cl = new SuperAdminSubjectModel();
            if (subjectID > 0)
            {
                var result = await _subjectService.GetSubjectBySubjectID(subjectID);
                cl.subjectInfo = result.Data;
            }

            var classData = await _subjectService.GetAllSubject();
            cl.subjectData = classData.Data;

            return View("Index", cl);
        }

        [HttpPost]
        public async Task<JsonResult> SaveSubject(SuperAdminSubjectEntity cl)
        {
            cl.Transaction = GetTransactionData(cl.SubjectID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            cl.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _subjectService.SubjectMaintenance(cl);
            if (data != null)
            {
                return Json(data);
            }

            return Json(0);
        }

        [HttpPost]
        public JsonResult RemoveSubject(long subjectID)
        {
            var result = _subjectService.RemoveSubject(subjectID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }
    }
}