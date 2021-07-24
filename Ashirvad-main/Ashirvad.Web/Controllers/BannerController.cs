using System;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Banner;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Ashirvad.Web.Controllers
{
    public class BannerController : BaseController
    {
        private readonly IBannerService _bannerService;
        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        // GET: Banner
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> BannerMaintenance(long bannerID)
        {
            BannerMaintenanceModel branch = new BannerMaintenanceModel();
            if (bannerID > 0)
            {
                var result = await _bannerService.GetBannerByBannerID(bannerID);
                branch.BannerInfo = result.Data;
            }
            else
            {
                branch.BannerInfo = new BannerEntity();
                //branch.BannerInfo.BannerType = new List<BannerTypeEntity>();
                //branch.BannerInfo.BannerType.Add(new BannerTypeEntity()
                //{
                //    TypeID = 1,
                //    TypeText = "Admin"
                //});
                //branch.BannerInfo.BannerType.Add(new BannerTypeEntity()
                //{
                //    TypeID = 2,
                //    TypeText = "Teacher"
                //});
                //branch.BannerInfo.BannerType.Add(new BannerTypeEntity()
                //{
                //    TypeID = 3,
                //    TypeText = "Student"
                //});
            }

            //var branchData = await _bannerService.GetAllBannerWithoutImage(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            var branchData = await _bannerService.GetAllBanner(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : (int)SessionContext.Instance.LoginUser.UserType);
            branch.BannerData = branchData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveBanner(BannerEntity bannerEntity)
        {
            var banner = JsonConvert.DeserializeObject<List<BannerTypeEntity>>(bannerEntity.JSONData);
            bannerEntity.BannerType = banner;
            if (bannerEntity.ImageFile != null)
            {
                bannerEntity.BannerImage = Common.Common.ReadFully(bannerEntity.ImageFile.InputStream);
            }

            bannerEntity.Transaction = GetTransactionData(bannerEntity.BannerID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            bannerEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _bannerService.BannerMaintenance(bannerEntity);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveBanner(long bannerID)
        {
            var result = _bannerService.RemoveBanner(bannerID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }


        [HttpPost]
        public async Task<string> GetBannerImage(long bannerID)
        {
            var data = await _bannerService.GetBannerByBannerID(bannerID);
            var result = data.Data;
            return "data:image/jpg;base64, " + result.BannerImageText;
        }

    }
}