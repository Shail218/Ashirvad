﻿using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/link/v1")]
    [AshirvadAuthorization]
    public class LinkController : ApiController
    {
        private readonly ILinkService _linkService = null;
        public LinkController(ILinkService linkService)
        {
            this._linkService = linkService;
        }

        [Route("LiveVideoMaintenance")]
        [HttpPost]
        public OperationResult<LinkEntity> LiveVideoMaintenance(LinkEntity linkInfo)
        {
            linkInfo.LinkType = 1;
            var data = this._linkService.LinkMaintenance(linkInfo);
            OperationResult<LinkEntity> result = new OperationResult<LinkEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetLiveVideoLinks")]
        [HttpGet]
        public OperationResult<List<LinkEntity>> GetLiveVideoLinks()
        {
            var data = this._linkService.GetAllLink(1);
            OperationResult<List<LinkEntity>> result = new OperationResult<List<LinkEntity>>();
            result = data.Result;
            return result;
        }

        [Route("GetLiveVideoLinksByBranch")]
        [HttpGet]
        public OperationResult<List<LinkEntity>> GetLiveVideoLinks(long branchID)
        {
            var data = this._linkService.GetAllLink(1, branchID);
            OperationResult<List<LinkEntity>> result = new OperationResult<List<LinkEntity>>();
            result = data.Result;
            return result;
        }

        [Route("GetLiveVideoLinksByBranchAndSTD")]
        [HttpGet]
        public OperationResult<List<LinkEntity>> GetLiveVideoLinks(long branchID, long stdID)
        {
            var data = this._linkService.GetAllLink(1, branchID, stdID);
            OperationResult<List<LinkEntity>> result = new OperationResult<List<LinkEntity>>();
            result = data.Result;
            return result;
        }


        [Route("GetLinkByID")]
        [HttpPost]
        public OperationResult<LinkEntity> GetLinkByID(long uniqueID)
        {
            var data = this._linkService.GetLinkByUniqueID(uniqueID);
            OperationResult<LinkEntity> result = new OperationResult<LinkEntity>();
            result = data.Result;
            return result;
        }


        [Route("YouTubeVideoMaintenance")]
        [HttpPost]
        public OperationResult<LinkEntity> YouTubeVideoMaintenance(LinkEntity linkInfo)
        {
            linkInfo.LinkType = 2;
            var data = this._linkService.LinkMaintenance(linkInfo);
            OperationResult<LinkEntity> result = new OperationResult<LinkEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetYouTubeVideoLinks")]
        [HttpGet]
        public OperationResult<List<LinkEntity>> GetYouTubeVideoLinks()
        {
            var data = this._linkService.GetAllLink(2);
            OperationResult<List<LinkEntity>> result = new OperationResult<List<LinkEntity>>();
            result = data.Result;
            return result;
        }

        [Route("GetYouTubeVideoLinksByBranch")]
        [HttpGet]
        public OperationResult<List<LinkEntity>> GetYouTubeVideoLinks(long branchID)
        {
            var data = this._linkService.GetAllLink(2, branchID);
            OperationResult<List<LinkEntity>> result = new OperationResult<List<LinkEntity>>();
            result = data.Result;
            return result;
        }

        [Route("GetYouTubeVideoLinksByBranchSTD")]
        [HttpGet]
        public OperationResult<List<LinkEntity>> GetYouTubeVideoLinks(long branchID, long stdID)
        {
            var data = this._linkService.GetAllLink(2, branchID, stdID);
            OperationResult<List<LinkEntity>> result = new OperationResult<List<LinkEntity>>();
            result = data.Result;
            return result;
        }

        [Route("RemoveLink")]
        [HttpPost]
        public OperationResult<bool> RemoveLink(long uniqueID, string lastupdatedby)
        {
            var data = this._linkService.RemoveLink(uniqueID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

    }
}
