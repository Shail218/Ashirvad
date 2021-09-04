using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Payment
{
    public interface IStudentPaymentService
    {
        Task<PaymentEntity> PaymentMaintenance(PaymentEntity paymentInfo);
        Task<OperationResult<List<PaymentEntity>>> GetAllPayments(long branchID);
        Task<OperationResult<List<PaymentEntity>>> GetAllPaymentWithoutContent(long branchID = 0);
        Task<OperationResult<PaymentEntity>> GetPaymentByPaymentID(long paymentID);
        bool RemovePayment(long paymentID, string lastupdatedby);
    }
}
