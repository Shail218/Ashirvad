using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class CompetitionAnsDetailsController : BaseController
    {
        private readonly ICompetitonService _competitonService;
        public CompetitionAnsDetailsController(ICompetitonService competitonService)
        {
            _competitonService = competitonService;
        }
        // GET: CompetitionAnsDetails
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CompetitionAnsSheetMaintenance(long competitonID)
        {
            CompetitonMaintenanceModel model = new CompetitonMaintenanceModel();
            var result = await _competitonService.GetAllDistinctAnswerSheetDatabyCompetitionId(competitonID);
            model.competitionAnswersData = result;
            return View("Index", model.competitionAnswersData);
        }

        public async Task<JsonResult> SaveZipFile(long competitionId, long StudentID, DateTime Competition, string Student, string Class)
        {
            string[] array = new string[2];
            string FileName = "";
            try
            {

                var homeworks = _competitonService.GetStudentAnswerSheetbyCompetitionID(competitionId, StudentID).Result;
                //string randomfilename = Common.Common.RandomString(20);
                string randomfilename = "Competition_" + Competition.ToString("ddMMyyyy") + "_Student_" + Student + "_Class_" + Class+"_z";
                FileName = "/ZipFiles/CompetitionDetail/" + randomfilename + ".zip";
                if (homeworks.Count > 0)
                {

                    string Ex = ".pdf";
                    if (System.IO.File.Exists(Server.MapPath
                                   ("~/ZipFiles/CompetitionDetail/" + randomfilename + ".zip")))
                    {
                        System.IO.File.Delete(Server.MapPath
                                      ("~/ZipFiles/CompetitionDetail/" + randomfilename + ".zip"));
                    }
                    ZipArchive zip = System.IO.Compression.ZipFile.Open(Server.MapPath
                             ("~/ZipFiles/CompetitionDetail/" + randomfilename + ".zip"), ZipArchiveMode.Create);

                    foreach (var item in homeworks)
                    {
                        zip.CreateEntryFromFile(Server.MapPath
                           ("~/" + item.CompetitionFilepath), item.CompetitionSheetName);

                    }
                    zip.Dispose();
                }



            }
            catch (Exception ex)
            {

            }
            return Json(FileName);

        }

        public JsonResult UpdateCompetitionAnswerSheetRemarks(long competitionId, long StudentID,string Remarks)
        {
            var result = _competitonService.UpdateCompetitionAnswerSheetRemarks(competitionId,StudentID,Remarks);
            return Json(result);
        }
    }
}