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
using System.IO;
using static Ashirvad.Common.Common;

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
            }
            //var branchData = await _bannerService.GetAllBanner(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 :
            //   SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 :
            //   (int)SessionContext.Instance.LoginUser.UserType);

            branch.BannerData = new List<BannerEntity>();
            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveBanner(BannerEntity bannerEntity)
        {

            if (SessionContext.Instance.LoginUser.UserType == Enums.UserType.Admin)
            {
                bannerEntity.BranchInfo = new BranchEntity()
                {
                    BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
                };
            }
            else
            {
                if (bannerEntity.BranchType == 1 && bannerEntity.BannerID == 0)
                {
                    bannerEntity.BranchInfo = new BranchEntity()
                    {
                        BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
                    };
                }
                else if (bannerEntity.BranchType == 0)
                {
                    bannerEntity.BranchInfo = new BranchEntity()
                    {
                        BranchID = 0
                    };
                }
                else if (bannerEntity.BranchType == 1 && bannerEntity.BranchInfo.BranchID == 0)
                {
                    bannerEntity.BranchInfo = new BranchEntity()
                    {
                        BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
                    };
                }
            }
            var banner = JsonConvert.DeserializeObject<List<BannerTypeEntity>>(bannerEntity.JSONData);
            bannerEntity.BannerType = banner;
            if (bannerEntity.ImageFile != null)
            {
                string _FileName = Path.GetFileName(bannerEntity.ImageFile.FileName);
                string extension = System.IO.Path.GetExtension(bannerEntity.ImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/BannerImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/BannerImage"), randomfilename + extension);
                bannerEntity.ImageFile.SaveAs(_path);
                bannerEntity.FileName = _FileName;
                bannerEntity.FilePath = _Filepath;
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

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            var branchData = await _bannerService.GetAllCustomBanner(model, SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : (int)SessionContext.Instance.LoginUser.UserType);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData[0].Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData
            });

        }
    }
}