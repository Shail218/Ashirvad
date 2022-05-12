using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class WinnerEntryController : BaseController
    {
        // GET: WinnerEntry
        private readonly ICompetitonService _competitonService;
        private readonly IBranchService _branchService;
        public WinnerEntryController(ICompetitonService competitonService, IBranchService branchService)
        {
            _competitonService = competitonService;
            _branchService = branchService;
        }
        public ActionResult Index()
        {
            CompetitonMaintenanceModel competitonMaintenanceModel = new CompetitonMaintenanceModel();
            competitonMaintenanceModel.competitionWinnerEntity = new CompetitionWinnerEntity();
            competitonMaintenanceModel.competitionWinnerData = new List<CompetitionWinnerEntity>();
            return View(competitonMaintenanceModel);
        }
        public async Task<ActionResult> CompetitionWinnerMaintenance(long CompetitionWinnerId)
        {
            CompetitonMaintenanceModel competitonMaintenanceModel = new CompetitonMaintenanceModel();
            competitonMaintenanceModel.competitionWinnerEntity = new CompetitionWinnerEntity();
            competitonMaintenanceModel.competitionWinnerData = new List<CompetitionWinnerEntity>();
            if (CompetitionWinnerId > 0)
            {
                var data = await _competitonService.GetCompetitionWinnerDetailbyId(CompetitionWinnerId);
                competitonMaintenanceModel.competitionWinnerEntity = data.Data;
            }
            var da = await _competitonService.GetCompetitionWinnerListbyCompetitionId();
            competitonMaintenanceModel.competitionWinnerData = da.Data;
            return View("Index", competitonMaintenanceModel);
        }
        public async Task<JsonResult> GetStudentListbyCompetitionIdandBranchId(long CompetitionId, long BranchId)
        {
            var result = await _competitonService.GetCompetitionRankListByCompetitionIdandBranchID(CompetitionId, BranchId);
            return Json(result);
        }
        public async Task<JsonResult> GetBranchList()
        {
            var branchData = await _branchService.GetAllBranch();
            return Json(branchData);
        }
        public async Task<JsonResult> GetCompetitionList()
        {
            var result = _competitonService.GetAllCompetitonData();
            return Json(result);
        }

        public async Task<JsonResult> SaveCompetitionWinner(CompetitionWinnerEntity winnerEntity)
        {
            winnerEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            winnerEntity.Transaction = GetTransactionData(winnerEntity.competition_winner_id > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _competitonService.CompetitionWinnerMaintenance(winnerEntity);
            return Json(data);
        }
        public async Task<JsonResult> RemoveCompetitionWinner(long CompetitionWinnerId)
        {
           var data = await _competitonService.DeleteCompetitionWinner(CompetitionWinnerId,SessionContext.Instance.LoginUser.Username);
            return Json(data);
        }

    }
}