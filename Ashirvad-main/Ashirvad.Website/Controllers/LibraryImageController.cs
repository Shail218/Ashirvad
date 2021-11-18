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
    public class LibraryImageController : Controller
    {

        private readonly ILibrary1Service _LibraryService=null;
        private readonly ICategoryService _CategoryService=null;
        public LibraryImageController(ILibrary1Service LibraryService,ICategoryService categoryService)
        {
            _LibraryService = LibraryService;
            _CategoryService = categoryService;
        }
        // GET: LibraryImage
        public async Task<ActionResult> LibraryImage()
        {
            LibraryImageEntity libraryImageEntity = new LibraryImageEntity();
            libraryImageEntity.imagelist = await _LibraryService.GetAllLibrary(2, 0);
            libraryImageEntity.Categorylist = await _CategoryService.GetAllCategorys(0);
            return View(libraryImageEntity);
        }
        
    }
}