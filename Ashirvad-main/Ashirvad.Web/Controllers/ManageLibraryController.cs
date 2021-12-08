using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class ManageLibraryController : BaseController
    {
        private readonly ILibraryService _libraryService;

        public ManageLibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        // GET: ManageLibrary
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ManageLibraryMaintenance()
        {
            LibraryMaintenanceModel library = new LibraryMaintenanceModel();
            var branchData = await _libraryService.GetAllLibraryApproval(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            library.LibraryData = branchData; 
            return View("Index", library.LibraryData);
        }

        [HttpPost]
        public async Task<JsonResult> SaveLibraryApproval(long LibraryID,string LibraryStatus,long ApprovalID)
        {
            ApprovalEntity approvalEntity = new ApprovalEntity();
            approvalEntity.library = new LibraryEntity();
            approvalEntity.library.LibraryID = LibraryID;
            approvalEntity.Branch_id = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            approvalEntity.Approval_id = ApprovalID;
            approvalEntity.Library_Status = LibraryStatus=="1"? Enums.ApprovalStatus.Pending:LibraryStatus == "2"?Enums.ApprovalStatus.Approve: Enums.ApprovalStatus.Reject;
            approvalEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            approvalEntity.TransactionInfo = GetTransactionData(ApprovalID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _libraryService.LibraryApprovalMaintenance(approvalEntity);
            if (data != null)
            {
                return Json(true);
            }
            return Json(false);
        }
    }
}