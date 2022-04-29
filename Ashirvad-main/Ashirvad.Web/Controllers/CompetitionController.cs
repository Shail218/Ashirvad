using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class CompetitionController : BaseController
    {
        private readonly ICompetitonService _competitonService;
        public CompetitionController(ICompetitonService competitonService)
        {
            _competitonService = competitonService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> CompetitionMaintenance(long competitonID)
        {
            CompetitonMaintenanceModel model = new CompetitonMaintenanceModel();
            if (competitonID > 0)
            {
                var result = await _competitonService.GetCompetitionByID(competitonID);
                model.CompetitonInfo = result;
            }
            var result1 = await _competitonService.GetAllCompetiton();
            model.CompetitionData = result1;
            return View("Index", model);
        }
        [HttpPost]
        public async Task<JsonResult> SaveCompetition(CompetitionEntity competitionEntity)
        {
            if (competitionEntity.FileInfo != null)
            {
                string _FileName = Path.GetFileName(competitionEntity.FileInfo.FileName);
                string extension = Path.GetExtension(competitionEntity.FileInfo.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/CompetitionImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/CompetitionImage"), randomfilename + extension);
                competitionEntity.FileInfo.SaveAs(_path);
                competitionEntity.FileName = _FileName;
                competitionEntity.FilePath = _Filepath;
                competitionEntity.DocLink = null;
                competitionEntity.DocType = true;
            }
            else
            {
                competitionEntity.FileName = null;
                competitionEntity.FilePath = null;
                competitionEntity.DocType = false;
            }
            competitionEntity.Transaction = GetTransactionData(competitionEntity.CompetitionID > 0 ? Enums.TransactionType.Update : Enums.TransactionType.Insert);
            competitionEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _competitonService.CompetitionMaintenance(competitionEntity);
            return Json(data);
        }
        [HttpPost]
        public JsonResult RemoveCompetition(long competitonID)
        {
            var result = _competitonService.DeleteCompetition(competitonID, SessionContext.Instance.LoginUser.Username.ToString());
            return Json(result);
        }
        public async Task<JsonResult> GetCompetitionDDL()
        {
            try
            {
                var competition = await _competitonService.GetAllCompetiton();

                if (competition.Count > 0)
                {
                    return Json(competition);
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