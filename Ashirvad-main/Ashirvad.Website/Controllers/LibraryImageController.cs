using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class LibraryImageController : BaseController
    {

        private readonly ILibraryService _LibraryService;
        private readonly ICategoryService _CategoryService=null;
        WebsiteModel websiteModel = new WebsiteModel();
        public LibraryImageController(ILibraryService libraryService,ICategoryService categoryService)
        {
            _LibraryService = libraryService;
            _CategoryService = categoryService;
        }
        // GET: LibraryImage
        public async Task<ActionResult> LibraryImage()
        {
            websiteModel = SessionContext.Instance.websiteModel;
            LibraryEntity libraryImageEntity = new LibraryEntity();
            websiteModel.Categorylist = await _CategoryService.GetAllCategorys(0);
            websiteModel.librarYImage = await _LibraryService.GetAllLibrary(2, 0);

            return View(websiteModel);
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