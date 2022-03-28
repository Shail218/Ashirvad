
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

        public async Task<ResponseModel> PaymentRegisterMaintenance(PaymentRegisterEntity entity)
        {
            ResponseModel responseModel = new ResponseModel();
            PaymentRegisterEntity paymententity = new PaymentRegisterEntity();
            try
            {
                responseModel = await _paymentContext.PaymentRegisterMaintenance(entity);
                //paymententity.payment_id = data;
                //return paymententity;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
        public async Task<ResponseModel> UpdatePaymentRegisterbyAdmin(PaymentRegisterEntity entity)
        {
            ResponseModel paymententity = new ResponseModel();
            try
            {
                var data = await _paymentContext.UpdatePaymentRegisterbyAdmin(entity);
                paymententity = data;
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
        public async Task<List<PaymentRegisterEntity>> GetPaymentRegisterList( long BranchID, long CourseID, long ClassID, long studentID)
        {
            try
            {
                var data = await _paymentContext.GetPaymentRegisterList(BranchID,CourseID,ClassID, studentID);
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
