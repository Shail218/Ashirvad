﻿using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class LibraryVideoController : Controller
    {
        // GET: LibraryVideo
        private readonly ILibrary1Service _LibraryService;
        public NewLibraryController(ILibrary1Service LibraryService)
        {
            _LibraryService = LibraryService;
        }
        public ActionResult LibraryVideo()
        {
            int BranchID = 0;
            var LibraryData = await _LibraryService.GetAllLibrary(Type, BranchID);
            return LibraryData;
        }
    }
}