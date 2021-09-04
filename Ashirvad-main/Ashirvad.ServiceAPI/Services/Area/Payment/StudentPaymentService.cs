using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Payment;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Payment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Payment
{
    public class StudentPaymentService : IStudentPaymentService
    {

        private readonly IStudentPaymentAPI _studentPaymentContext;
        public StudentPaymentService(IStudentPaymentAPI studentContext)
        {
            _studentPaymentContext = studentContext;
        }

        public async Task<PaymentEntity> PaymentMaintenance(PaymentEntity paymentInfo)
        {
            PaymentEntity payment = new PaymentEntity();
            try
            {
                if (paymentInfo.PaymentData != null)
                {
                    if (paymentInfo.PaymentData.FileInfo != null)
                    {
                        paymentInfo.PaymentData.PaymentContent = Common.Common.ReadFully(paymentInfo.PaymentData.FileInfo.InputStream);
                        paymentInfo.PaymentData.PaymentContentFileName = Path.GetFileName(paymentInfo.PaymentData.FileInfo
                            .FileName);
                    }
                }

                long paymentID = await _studentPaymentContext.PaymentMaintenance(paymentInfo);
                if (paymentID > 0)
                {
                    payment.PaymentID = paymentID;
                    if (!string.IsNullOrEmpty(Common.Common.GetStringConfigKey("DocDirectory")))
                    {
                        Common.Common.SaveFile(paymentInfo.PaymentData.PaymentContent, paymentInfo.PaymentData.PaymentContentFileName, "Paper\\");
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return payment;
        }

        public async Task<OperationResult<List<PaymentEntity>>> GetAllPayments(long branchID)
        {
            try
            {
                OperationResult<List<PaymentEntity>> payments = new OperationResult<List<PaymentEntity>>();
                payments.Data = await _studentPaymentContext.GetAllPayment(branchID);
                payments.Completed = true;
                return payments;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<OperationResult<List<PaymentEntity>>> GetAllPaymentWithoutContent(long branchID = 0)
        {
            try
            {
                OperationResult<List<PaymentEntity>> payment = new OperationResult<List<PaymentEntity>>();
                payment.Data = await _studentPaymentContext.GetAllPaymentWithoutContent(branchID);
                payment.Completed = true;
                return payment;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<PaymentEntity>> GetPaymentByPaymentID(long paymentID)
        {
            try
            {
                OperationResult<PaymentEntity> payment = new OperationResult<PaymentEntity>();
                payment.Data = await _studentPaymentContext.GetPaymentByID(paymentID);
                payment.Completed = true;
                return payment;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        
        public bool RemovePayment(long paymentID, string lastupdatedby)
        {
            try
            {
                return this._studentPaymentContext.RemovePayment(paymentID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}
