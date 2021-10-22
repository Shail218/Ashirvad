using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.AboutUs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class AboutUsController : BaseController
    {
        private readonly IAboutUsService _aboutUsService;

        public AboutUsController(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
        }
        // GET: AboutUs
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AboutUsMaintenance(long aboutID=0, long detailid = 0)
        {
            AboutUsMaintenanceModel model = new AboutUsMaintenanceModel();
            if (detailid > 0)
            {
                var about = await _aboutUsService.GetAboutUsByUniqueID(aboutID, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                model.AboutusInfo = about.Data;
                var aboutdetail = await _aboutUsService.GetAboutUsDetailByUniqueID(detailid);
                model.detailInfo = aboutdetail.Data;
            }
            else
            {
                var about = await _aboutUsService.GetAboutUsByUniqueID(aboutID, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                if(about.Data==null)
                {
                    model.AboutusInfo = new AboutUsEntity();
                }
                else
                {
                    model.AboutusInfo = about.Data;
                }
               
            }

            var list = await _aboutUsService.GetAllAboutUs(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            model.detailData = list;

            return View("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> ManageMaintenance(long aboutID)
        {
            AboutUsMaintenanceModel model = new AboutUsMaintenanceModel();

            var list = await _aboutUsService.GetAllAboutUs(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            model.detailData = list;

            return View("~/Views/AboutUs/Manage.cshtml", model.AboutusData);
        }

        [HttpPost]
        public async Task<ActionResult> DetailMaintenance(long aboutID)
        {
            AboutUsMaintenanceModel model = new AboutUsMaintenanceModel();
            model.detailInfo = new AboutUsDetailEntity();
            if (aboutID > 0)
            {
                var about = await _aboutUsService.GetAboutUsByUniqueID(aboutID);
                model.AboutusInfo = about.Data;
            }

            return View("~/Views/AboutUs/DetailCreate.cshtml", model.detailInfo);
        }

        [HttpPost]
        public async Task<JsonResult> SaveAboutus(AboutUsEntity about)
        {

            if (about.ImageFile != null)
            {

                string _FileName = Path.GetFileName(about.ImageFile.FileName);
                string extension = System.IO.Path.GetExtension(about.ImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/AboutUsHeader/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/AboutUsHeader"), randomfilename + extension);
                about.ImageFile.SaveAs(_path);
                about.HeaderImageName = _FileName;
                about.FilePath = _Filepath;
            }
            if (SessionContext.Instance.LoginUser.UserType != Enums.UserType.SuperAdmin)
            {
                about.BranchInfo.BranchID = (int)SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            }
            about.TransactionInfo = GetTransactionData(about.AboutUsID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            about.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            about.BranchInfo.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            var data = await _aboutUsService.AboutUsMaintenance(about);
            if (data != null)
            {
                return Json(data);
            }
            return Json(0);
        }

        [HttpPost]
        public async Task<JsonResult> SaveDetails(AboutUsDetailEntity entity)
        {
            AboutUsDetailEntity data = new AboutUsDetailEntity();
            
            if (entity.ImageFile != null)
            {

                foreach (var item in entity.ImageFile)
                {
                    string _FileName = Path.GetFileName(item.FileName);
                    string extension = System.IO.Path.GetExtension(item.FileName);
                    string randomfilename = Common.Common.RandomString(20);
                    string _Filepath = "/AboutUsDetail/" + randomfilename + extension;
                    string _path = Path.Combine(Server.MapPath("~/AboutUsDetail"), randomfilename + extension);
                    item.SaveAs(_path);
                    entity.HeaderImageText = _FileName;
                    entity.FilePath = _Filepath;
                    entity.TransactionInfo = GetTransactionData(entity.DetailID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                    entity.RowStatus = new RowStatusEntity()
                    {
                        RowStatusId = (int)Enums.RowStatus.Active
                    };
                    entity.BranchInfo = new BranchEntity();
                    if (SessionContext.Instance.LoginUser.UserType != Enums.UserType.SuperAdmin)
                    {

                        entity.BranchInfo.BranchID = (int)SessionContext.Instance.LoginUser.BranchInfo.BranchID;
                    }                   
                    data = await _aboutUsService.AboutUsDetailMaintenance(entity);
                    
                }

            }
            if (data.DetailID >0)
            {
                return Json(true);
            }

            return Json(false);

        }

        

        [HttpPost]
        public JsonResult RemoveAboutUs(long aboutID)
        {
            var result = _aboutUsService.RemoveAboutUs(aboutID, SessionContext.Instance.LoginUser.Username, true);
            return Json(result);
        }

        [HttpPost]
        public JsonResult RemoveDetails(long detailID)
        {
            var result = _aboutUsService.RemoveAboutUsDetail(detailID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }
    }
}