using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.PaymentRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class OnlinePaymentListController : BaseController
    {
        private readonly IPaymentRegisterService _paymentRegisterService;
        public OnlinePaymentListController(IPaymentRegisterService paymentRegisterService)
        {
            _paymentRegisterService = paymentRegisterService;
           }
        // GET: StudentOnlinePaymentList
        public ActionResult Index()
        {
            PaymentRegisterDataModel model = new PaymentRegisterDataModel();
            model.registerEntities = new List<Data.PaymentRegisterEntity>();
            return View("Index",model);
        }
        public async Task<ActionResult> GetStudentPaymentData(long CourseID,long ClassId,long StudentId)
        {
            PaymentRegisterDataModel model = new PaymentRegisterDataModel();
            model.registerEntities = await _paymentRegisterService.GetPaymentRegisterList(SessionContext.Instance.LoginUser.FinancialYear, SessionContext.Instance.LoginUser.BranchInfo.BranchID, CourseID, ClassId, StudentId);
            return View("~/Views/OnlinePaymentList/Manage.cshtml", model.registerEntities);
        }
        [HttpPost]
        public async Task<JsonResult> UpdatePaymentDetails(long paymentID, long StudentID,string Remarks,int status)
        {
            PaymentRegisterEntity model = new PaymentRegisterEntity();
            model.payment_id = paymentID;
            model.payment_status = status;
            model.remark = Remarks;
            model.studentEntity.StudentID = StudentID;
            model.Transaction = GetTransactionData(model.payment_id > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = _paymentRegisterService.PaymentRegisterMaintenance(model);
            return Json(model);
        }
    }
}