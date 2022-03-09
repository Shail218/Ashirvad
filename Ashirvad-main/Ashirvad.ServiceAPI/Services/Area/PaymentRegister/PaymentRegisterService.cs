
using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.PaymentRegister;
using Ashirvad.ServiceAPI.ServiceAPI.Area.PaymentRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.PaymentRegister
{
    public class PaymentRegisterService : IPaymentRegisterService
    {
        private readonly IPaymentRegisterAPI _paymentContext;
        public PaymentRegisterService(IPaymentRegisterAPI paymentContext)
        {
            _paymentContext = paymentContext;
        }

        public async Task<PaymentRegisterEntity> PaymentRegisterMaintenance(PaymentRegisterEntity entity)
        {
            PaymentRegisterEntity paymententity = new PaymentRegisterEntity();
            try
            {
                var data = await _paymentContext.PaymentRegisterMaintenance(entity);
                paymententity.payment_id = data;
                return paymententity;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return paymententity;
        }

        public async Task<List<PaymentRegisterEntity>> GetPaymentListForStudent(long studentID,long branchID)
        {
            try
            {
                var data = await _paymentContext.GetPaymentListForStudent(studentID,branchID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<PaymentRegisterEntity>> GetPaymentRegisterList(string financialyear, long BranchID, long CourseID, long ClassID, long studentID)
        {
            try
            {
                var data = await _paymentContext.GetPaymentRegisterList(financialyear, BranchID,CourseID,ClassID, studentID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
       
    }
}
