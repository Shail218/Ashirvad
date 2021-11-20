using Ashirvad.Data;
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
        public LibraryVideoController(ILibrary1Service LibraryService, ICategoryService categoryService)
        {
            _LibraryService = LibraryService;
            _CategoryService = categoryService;
        }
        public async Task<ActionResult> LibraryVideo()
        {
            LibraryVideoEntity libraryVideo = new LibraryVideoEntity();
            libraryVideo.Categorylist = new List<CategoryEntity>();

            libraryVideo.Videolist = await _LibraryService.GetAllLibrary(1, 0);
            libraryVideo.Categorylist = await _CategoryService.GetAllCategorys(0);

            return View(libraryVideo);

        }
    }
}