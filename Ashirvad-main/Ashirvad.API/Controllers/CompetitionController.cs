﻿using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/competition/v1")]
    [AshirvadAuthorization]
    public class CompetitionController : ApiController
    {
        private readonly ICompetitonService _competitionService;
        public CompetitionController(ICompetitonService competitionService)
        {
            _competitionService = competitionService;
        }

        [Route("GetAllCompetition")]
        [HttpGet]
        public OperationResult<List<CompetitionEntity>> GetAllCompetition()
        {
            var data = _competitionService.GetAllCompetiton();
            OperationResult<List<CompetitionEntity>> result = new OperationResult<List<CompetitionEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }
    }
}
