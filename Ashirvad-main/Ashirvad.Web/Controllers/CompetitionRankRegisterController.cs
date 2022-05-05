using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class CompetitionRankRegisterController : BaseController
    {
        private readonly ICompetitonService _competitonService;
        public CompetitionRankRegisterController(ICompetitonService competitonService)
        {
            _competitonService = competitonService;
        }
        // GET: CompetitionRankRegister
        public async Task<ActionResult> Index()
        {
            CompetitonMaintenanceModel maintenanceModel = new CompetitonMaintenanceModel();
            var result = await this._competitonService.GetCompetitionRankDistinctList();
            maintenanceModel.competitionRankData = result.Data;
            return View(maintenanceModel);
        }
        public async Task<ActionResult> GetCompetitionRankStudentListbyCompetitionId(long CompetitionId)
        {
            CompetitonMaintenanceModel maintenanceModel = new CompetitonMaintenanceModel();
            var result = await this._competitonService.GetCompetitionRankListbyCompetitionId(CompetitionId);
            maintenanceModel.competitionRankData = result.Data;
            return View("~/Views/CompetitionRankRegister/CompetitionRankDetails.cshtml", maintenanceModel.competitionRankData);
        }
        public async Task<JsonResult> UpdateCompetitionRankDetail(long CompetitionId, long CompetitionRankId, string Rank)
        {
            var result = this._competitonService.UpdateRankDetail(CompetitionId, CompetitionRankId, Rank);  
            return Json(result);
        }
    }
}