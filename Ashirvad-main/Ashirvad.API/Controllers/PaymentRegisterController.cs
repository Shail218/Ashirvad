using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.PaymentRegister;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/payment/v1")]
    [AshirvadAuthorization]
    public class PaymentRegisterController : ApiController
    {
        private readonly IPaymentRegisterService _paymentservice = null;
        ResponseModel model = new ResponseModel();
        public PaymentRegisterController(IPaymentRegisterService paymentservice)
        {
            this._paymentservice = paymentservice;
        }

        [Route("PaymentRegisterMaintenance")]
        [HttpPost]
        public OperationResult<PaymentRegisterEntity> PaymentRegisterMaintenance(string model, bool HasFile)
        {
            OperationResult<PaymentRegisterEntity> result = new OperationResult<PaymentRegisterEntity>();
            var httpRequest = HttpContext.Current.Request;
            PaymentRegisterEntity entity = new PaymentRegisterEntity();
            PaymentRegisterEntity response = new PaymentRegisterEntity();
            entity.studentEntity = new StudentEntity();
            entity.branchEntity = new BranchEntity();
            var paymententity = JsonConvert.DeserializeObject<PaymentRegisterEntity>(model);
            entity.payment_id = paymententity.payment_id;
            entity.studentEntity.StudentID = paymententity.studentEntity.StudentID;
            entity.branchEntity.BranchID = paymententity.branchEntity.BranchID;
            entity.remark = paymententity.remark;
            entity.student_remark = paymententity.student_remark;
            entity.payment_status = paymententity.payment_status;
            entity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            entity.Transaction = new TransactionEntity()
            {
                CreatedBy = paymententity.Transaction.CreatedBy,
                CreatedId = paymententity.Transaction.CreatedId,
                CreatedDate = DateTime.Now,
            };
            if (HasFile)
            {
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        for (int file = 0; file < httpRequest.Files.Count; file++)
                        {
                            string fileName;
                            string extension;
                            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                            string UpdatedPath = currentDir.Replace("WebAPI", "wwwroot");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/PaymentRegisterImage/" + randomfilename + extension;
                            string _Filepath1 = "PaymentRegisterImage/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/PaymentRegisterImage/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            entity.file_name = fileName;
                            entity.file_path = _Filepath;
                            var data = this._paymentservice.PaymentRegisterMaintenance(entity);
                            response = data.Result;
                        }
                        result.Data = null;
                        result.Completed = false;
                        if (response.payment_id > 0)
                        {
                            result.Data = response;
                            result.Completed = true;
                            result.Message = "Payment Register Successfully.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Completed = false;
                    result.Data = null;
                    result.Message = ex.ToString();
                }
            }
            return result;
        }

        [Route("GetPaymentListForStudent")]
        [HttpGet]
        public OperationResult<List<PaymentRegisterEntity>> GetPaymentListForStudent(long studentID,long branchID)
        {
            var data = this._paymentservice.GetPaymentListForStudent(studentID,branchID);
            OperationResult<List<PaymentRegisterEntity>> result = new OperationResult<List<PaymentRegisterEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }
    }
}