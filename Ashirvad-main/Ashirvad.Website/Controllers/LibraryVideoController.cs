using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
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
        private readonly ILibrary1Service _LibraryService;
        private readonly ICategoryService _CategoryService;
        WebsiteModel websiteModel = new WebsiteModel();
        public LibraryVideoController(ILibrary1Service LibraryService, ICategoryService categoryService)
        {
            _LibraryService = LibraryService;
            _CategoryService = categoryService;
        }
        public async Task<ActionResult> LibraryVideo()
        {
            websiteModel = SessionContext.Instance.websiteModel;
            LibraryVideoEntity libraryVideo = new LibraryVideoEntity();
            websiteModel.Categorylist = new List<CategoryEntity>();

            websiteModel.Videolist = await _LibraryService.GetAllLibrary(1, 0);
            websiteModel.Categorylist = await _CategoryService.GetAllCategorys(0);

            return View(websiteModel);

        }
    }
}