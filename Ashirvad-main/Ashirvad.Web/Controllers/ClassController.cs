using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class ClassController : BaseController
    {

        private readonly IClassService _classService;
        public ClassController(IClassService classservice)
        {
            _classService = classservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ClassMaintenance(long classID)
        {
            ClassMaintenanceModel cl = new ClassMaintenanceModel();
            if (classID > 0)
            {
                var result = await _classService.GetClassByClassID(classID);
                cl.ClassInfo = result.Data;
            }

            //var classData = await _classService.GetAllClass();
            cl.ClassData = new List<ClassEntity>();

            return View("Index", cl);
        }

        [HttpPost]
        public async Task<JsonResult> SaveClass(ClassEntity cl)
        {
            cl.Transaction = GetTransactionData(cl.ClassID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            cl.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            cl.UserType = SessionContext.Instance.LoginUser.UserType;
            cl.branchEntity = new BranchEntity()
            {
                BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
            };
            var data = await _classService.ClassMaintenance(cl);
            if (data != null)
            {
                return Json(data);
            }

            return Json(0);
        }        

        [HttpPost]
        public JsonResult RemoveClass(long classID)
        {
            var result = _classService.RemoveClass(classID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            var branchData = await _classService.GetAllCustomClass(model);
            long total = 0;
            if (branchData.Data.Count > 0)
            {
                total = branchData.Data[0].Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData.Data
            });

        }
    }
}