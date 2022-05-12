using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class CompetitionRankController : BaseController
    {
        private readonly ICompetitonService _competitonService;
        public CompetitionRankController(ICompetitonService competitonService)
        {
            _competitonService = competitonService;
        }
        // GET: CompetitionRank
        public ActionResult Index()
        {
            CompetitonMaintenanceModel competitonMaintenanceModel = new CompetitonMaintenanceModel();
            competitonMaintenanceModel.CompetitonInfo = new CompetitionEntity();
            competitonMaintenanceModel.CompetitionData = new List<CompetitionEntity>();
            competitonMaintenanceModel.competitionAnswersData = new List<CompetitionAnswerSheetEntity>();
            return View(competitonMaintenanceModel);
        }
        public async Task<JsonResult> GetCompetitionData()
        {
            var result = _competitonService.GetAllCompetitonData();
            return Json(result);
        }
        public async Task<ActionResult> GetStudentListforCompetitionRank(long competitonID)
        {
            CompetitonMaintenanceModel competitonMaintenanceModel = new CompetitonMaintenanceModel();
            var data = await _competitonService.GetStudentListforCompetitionRankEntry(competitonID);
            ViewBag.Status = data.Status.ToString();
            ViewBag.Message = data.Message;
            return View("~/Views/CompetitionRank/Manage.cshtml", data.Data);
        }
        public async Task<JsonResult> CompetitionRankMaintenance(string JsonData)
        {
            ResponseModel model = new ResponseModel();
            var line = JsonConvert.DeserializeObject<List<CompetitionRankEntity>>(JsonData);
            if (line?.Count > 0)
            {
                foreach (var i in line)
                {
                    i.RankDate = DateTime.Now;
                    i.RowStatus = new RowStatusEntity() { RowStatusId = (int)Enums.RowStatus.Active };
                    i.Transaction = GetTransactionData(i.CompetitionRankId > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                    model = await _competitonService.CompetitionRankMaintenance(i);
                }
            }
            return Json(model);
        }
    }
}