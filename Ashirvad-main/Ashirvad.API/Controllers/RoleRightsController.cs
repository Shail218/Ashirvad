using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Page;
using Ashirvad.ServiceAPI.ServiceAPI.Area.RoleRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/RoleRights/v1")]
    [AshirvadAuthorization]
    public class RoleRightsController : ApiController
    {
        private readonly IRoleRightsService _RoleRightService;
        private readonly IPageService _pageService;
        private readonly IBranchRightsService _BranchRightService;
        public RoleRightsController(IRoleRightsService RoleRightService, IPageService pageService, IBranchRightsService BranchRightService)
        {

            _RoleRightService = RoleRightService;
            _pageService = pageService;
            _BranchRightService = BranchRightService;
        }

        [Route("RoleRightsMaintenance")]
        [HttpPost]
        public OperationResult<RoleRightsEntity> RoleRightsMaintenance(List<RoleRightsEntity> roleInfo)
        {
            OperationResult<RoleRightsEntity> result = new OperationResult<RoleRightsEntity>();
            try
            {

            
            foreach (var roleIn in roleInfo)
            {
                var data = this._RoleRightService.RoleRightsMaintenance(roleIn);

                result.Completed = data.Result.Status;
                if (data.Result.Status && data.Result.Data != null)
                {
                    result.Data = (RoleRightsEntity)data.Result.Data;
                }
                result.Message = data.Result.Message;
            }
            }
            catch (Exception ex)
            {
                result.Completed = false;
                result.Message = ex.Message;
            }
            return result;
        }

        [Route("GetAllRightsbyBranch")]
        [HttpGet]
        public OperationResult<List<RoleRightsEntity>> GetAllRightsbyBranch(long branchID)
        {
            var data = this._RoleRightService.GetAllRightsbyBranch(branchID);
            OperationResult<List<RoleRightsEntity>> result = new OperationResult<List<RoleRightsEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }
        
        [Route("GetPageListbyBranchID")]
        [HttpGet]
        public OperationResult<List<RoleRightsEntity>> GetPageListbyBranchID(long branchID)
        {
            var data = this._RoleRightService.GetPageListbyBranchID(branchID);
            OperationResult<List<RoleRightsEntity>> result = new OperationResult<List<RoleRightsEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetRolerightsByID")]
        [HttpGet]
        public OperationResult<RoleRightsEntity> GetRolerightsByID(long roleId)
        {
            var data = this._RoleRightService.GetRolerightsByID(roleId);
            OperationResult<RoleRightsEntity> result = new OperationResult<RoleRightsEntity>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetRoleRightsByRoleRightsID")]
        [HttpGet]
        public OperationResult<List<RoleRightsEntity>> GetRoleRightsByRoleRightsID(long roleId, long branchId)
        {
            var data = this._RoleRightService.GetRoleRightsByRoleRightsID(roleId, branchId);
            OperationResult<List<RoleRightsEntity>> result = new OperationResult<List<RoleRightsEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetRoleRightsByRoleID")]
        [HttpGet]
        public OperationResult<RoleRightMaintenanceModel> GetRoleRightsByRoleID(long roleId, long branchId)
        {
            RoleRightMaintenanceModel maintenanceModel = new RoleRightMaintenanceModel();
            maintenanceModel.RoleRightsInfo = this._RoleRightService.GetRolerightsByID(roleId).Result;
            maintenanceModel.RoleRightsInfo.list = this._RoleRightService.GetRoleRightsByRoleRightsID(roleId, branchId).Result;
            var branchData = this._BranchRightService.GetBranchRightsByBranchID(branchId).Result;
            foreach (var da in branchData)
            {
                maintenanceModel.RoleRightsInfo.PageList.Add(da.PageInfo);
            }
            OperationResult<RoleRightMaintenanceModel> result = new OperationResult<RoleRightMaintenanceModel>();
            result.Data = maintenanceModel;
            result.Completed = true;
            return result;
        }



        [Route("RemoveRoleRights")]
        [HttpPost]
        public OperationResult<bool> RemoveRoleRights(long roleRightsId, string lastupdatedby)
        {
            var data = this._RoleRightService.RemoveRoleRights(roleRightsId, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }

    }
}

