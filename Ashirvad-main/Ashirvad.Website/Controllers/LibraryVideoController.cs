using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class LibraryVideoController : BaseController
    {
        // GET: LibraryVideo
        private readonly ILibraryService _LibraryService;
        private readonly ICategoryService _CategoryService;
        WebsiteModel websiteModel = new WebsiteModel();
        public LibraryVideoController(ILibraryService LibraryService, ICategoryService categoryService)
        {
            _LibraryService = LibraryService;
            _CategoryService = categoryService;
        }
        public async Task<ActionResult> LibraryVideo()
        {
            websiteModel = SessionContext.Instance.websiteModel;
            LibraryEntity libraryImageEntity = new LibraryEntity();
            websiteModel.Categorylist = await _CategoryService.GetAllCategorys(0);
            websiteModel.librarYImage = await _LibraryService.GetAllLibrary(1, 0);
            return View(websiteModel);
        }
    }
}