using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Page;
using Ashirvad.ServiceAPI.ServiceAPI.Area.RoleRights;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class RoleRightsController : BaseController
    {
        private readonly IRoleRightsService _RoleRightService;
        private readonly IPageService _pageService;

        ResponseModel response = new ResponseModel();
        public RoleRightsController(IRoleRightsService RoleRightService, IPageService pageService)
        {

            _RoleRightService = RoleRightService;
            _pageService = pageService;
        }
        // GET: RoleRight
        // GET: RoleRights

        public ActionResult Index()
        {
            RoleRightMaintenanceModel RoleRight = new RoleRightMaintenanceModel();
            RoleRight.RoleRightsData = new List<RoleRightsEntity>();
            return View(RoleRight);
        }

        public ActionResult NoRights()
        {

            return View();
        }

        public async Task<ActionResult> RoleRightMaintenance(long RoleRightID)
        {
                RoleRightMaintenanceModel RoleRight = new RoleRightMaintenanceModel();
                RoleRight.RoleRightsInfo = new RoleRightsEntity();
                RoleRight.RoleRightsInfo.list = new List<RoleRightsEntity>();
                if (RoleRightID > 0)
                {
                    var result = await _RoleRightService.GetRolerightsByID(RoleRightID);
                
                    var result2 = await _RoleRightService.GetRoleRightsByRoleRightsID(RoleRightID);
                    RoleRight.RoleRightsInfo = result;
                    RoleRight.RoleRightsInfo.list = result2;
                }
                //var RoleRightData = await _RoleRightService.GetAllRoleRights();
                RoleRight.RoleRightsData = new List<RoleRightsEntity>();

                var branchData = await _pageService.GetAllPages(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                RoleRight.RoleRightsInfo.PageList = branchData;
                return View("Index", RoleRight);
            
        }

        [HttpPost]
        public async Task<JsonResult> SaveRoleRight(RoleRightsEntity RoleRight)
        {
            response.Status = false;

            long rightsID = RoleRight.RoleRightsId;
            RoleRightsEntity RoleRightsEntity = new RoleRightsEntity();

            RoleRight.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var List = JsonConvert.DeserializeObject<List<RoleRightsEntity>>(RoleRight.JasonData);
            foreach (var item in List)
            {
                RoleRight.Transaction = GetTransactionData(rightsID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                RoleRight.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active
                };
                RoleRight.RoleRightsId = rightsID == 0 ? rightsID : item.RoleRightsId;
                RoleRight.PageInfo = item.PageInfo;
                RoleRight.Createstatus = item.Createstatus;
                RoleRight.Viewstatus = item.Viewstatus;
                RoleRight.Deletestatus = item.Deletestatus;
                response = await _RoleRightService.RoleRightsMaintenance(RoleRight);
                if (!response.Status)
                {
                    break;
                }
            }
            //if (RoleRightsEntity.RoleRightsId > 0)
            //{
            //    response.Status = true;
            //    response.Message = RoleRight.RoleRightsId > 0 ? "Updated Successfully!!" : "Created Successfully!!";
            //}
            //else if (RoleRightsEntity.RoleRightsId < 0)
            //{
            //    response.Status = false;
            //    response.Message = "Already Exists!!";
            //}
            //else
            //{
            //    response.Status = false;
            //    response.Message = RoleRight.RoleRightsId > 0 ? "Failed To Update!!" : "Failed To Create!!";
            //}
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveRoleRight(long RoleRightID)
        {
            var result = _RoleRightService.RemoveRoleRights(RoleRightID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        //public async Task<JsonResult> RoleRightData()
        //{
        //    var RoleRightData = await _RoleRightService.GetAllRoleRights();

        //    return Json(RoleRightData);
        //}

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            var branchData = await _RoleRightService.GetAllCustomRights(model,SessionContext.Instance.LoginUser.BranchInfo.BranchID);
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