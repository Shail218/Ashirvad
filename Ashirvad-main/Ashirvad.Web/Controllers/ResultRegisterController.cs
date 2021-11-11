using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class ResultRegisterController : BaseController
    {
        private readonly IMarksService _MarksService;

        public ResultRegisterController(IMarksService marksService)
        {
            _MarksService = marksService;
        }
        // GET: ResultRegister
        public ActionResult Index()
        {
            MarksMaintenanceModel model = new MarksMaintenanceModel();
            model.MarksInfo = new MarksEntity();
            return View(model);
        }

        //public async Task<ActionResult> GetStudentByStd(long Std, long BatchTime)
        //{
        //    var result = _MarksService.GetStudentByStd(Std, SessionContext.Instance.LoginUser.BranchInfo.BranchID, BatchTime).Result;
        //    return View("~/Views/ResultEntry/Manage.cshtml", result);
        //}
    }
}