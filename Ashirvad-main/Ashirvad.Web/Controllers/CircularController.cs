using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Circular;
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
    public class CircularController : BaseController
    {
        private readonly ICircularService _circularService;
        public CircularController(ICircularService circularService)
        {
            _circularService = circularService;
        }

        // GET: Banner
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CircularMaintenance(long circularID)
        {
            CircularMaintenanceModel branch = new CircularMaintenanceModel();
            if (circularID > 0)
            {
                var result = await _circularService.GetCircularById(circularID);
                branch.CircularEntity = result;
            }
            else
            {
                branch.CircularEntity = new CircularEntity();
            }
            branch.CircularEntitiesData = new List<CircularEntity>();
            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCircular(CircularEntity circularEntity)
        {
            if (circularEntity.ImageFile != null)
            {
                string _FileName = Path.GetFileName(circularEntity.ImageFile.FileName);
                string extension = Path.GetExtension(circularEntity.ImageFile.FileName);
                string randomfilename = RandomString(20);
                string _Filepath = "/CircularFiles/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/CircularFiles"), randomfilename + extension);
                circularEntity.ImageFile.SaveAs(_path);
                circularEntity.FileName = _FileName;
                circularEntity.FilePath = _Filepath;
            }
            circularEntity.Transaction = GetTransactionData(circularEntity.CircularId > 0 ? Enums.TransactionType.Update : Enums.TransactionType.Insert);
            circularEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _circularService.CircularMaintenance(circularEntity);
           

            return Json(data);
        }

        [HttpPost]
        public JsonResult RemoveCircular(long circularID)
        {
            var result = _circularService.RemoveCircular(circularID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> GetAllCircular(DataTableAjaxPostModel model)
        {
            var branchData = await _circularService.GetAllCircular();
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData.Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData
            });

        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            var branchData = await _circularService.GetAllCustomCircular(model);
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