using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public ActionResult Download(string filedata,string FileName)
        {
            string[] extention = FileName.Split('.');
            if(extention.Count() >0)
            {
                using (var client = new WebClient())
                {
                    var buffer = client.DownloadData(filedata);
                    return File(buffer, "application/" + extention[1] + "", FileName);
                }
            }
            else
            {
                using (var client = new WebClient())
                {
                    var buffer = client.DownloadData("~/Themedata/assets/images/noimage.png");
                    return File(buffer, "application/png", "noimage.png");
                }
            }
        }

        
    }
}