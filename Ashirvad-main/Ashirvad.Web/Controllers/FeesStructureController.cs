using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class FeesStructureController : BaseController
    {
        // GET: FeesStructure
        private readonly IFeesService _FeesService;
 
        public FeesStructureController(IFeesService FeesService)
        {
            _FeesService = FeesService;
        }
        // GET: Fees
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> FeesMaintenance(long FeesID)
        {
            FeesMaintenanceModel Fees = new FeesMaintenanceModel();
            if (FeesID > 0)
            {
                var result = await _FeesService.GetFeesByFeesID(FeesID);
                Fees.FeesInfo = result;
            }
            Fees.FeesData = new List<FeesEntity>();
            return View("Index", Fees);
        }       

        [HttpPost]
        public async Task<JsonResult> SaveFees(FeesEntity Fees)
        {
            if (Fees.ImageFile != null)
            {
                string _FileName = Path.GetFileName(Fees.ImageFile.FileName);
                string extension = System.IO.Path.GetExtension(Fees.ImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/FeesImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/FeesImage"), randomfilename + extension);
                Fees.ImageFile.SaveAs(_path);
                Fees.FileName = _FileName;
                Fees.FilePath = _Filepath;
            }

            Fees.Transaction = GetTransactionData(Fees.FeesID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            Fees.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _FeesService.FeesMaintenance(Fees);
            
            return Json(data);
        }

        [HttpPost]
        public JsonResult RemoveFees(long FeesID)
        {
            var result = _FeesService.RemoveFees(FeesID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> FeesData()
        {
            var FeesData = await _FeesService.GetAllFeesWithoutImage();           

            return Json(FeesData);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("Remark");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _FeesService.GetAllCustomFees(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
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