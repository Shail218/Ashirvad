using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class CompetitionController : BaseController
    {
        private readonly ICompetitonService _competitionService;

        ResponseModel responseModel = new ResponseModel();
        // GET: Competition
        public CompetitionController(ICompetitonService competitionService)
        {
            _competitionService = competitionService;
        }
        public ActionResult Index()
        {
            return View();
        }
        //public async Task<ActionResult> CompetitionMaintenance(long CompetitionID=0)
        //{

        //    CompetitonMaintenanceModel competition = new CompetitonMaintenanceModel();            
        //    if (CompetitionID > 0)
        //    {
        //        var data = await _competitionService.GetCompetitionByID((int)CompetitionID);
        //        //competition.CompetitionData = data;
        //    }
        //    return View("Index");
        //}
    }
}