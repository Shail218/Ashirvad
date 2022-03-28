using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Payment
{
    public interface IStudentPaymentAPI
    {
        Task<ResponseModel> PaymentMaintenance(PaymentEntity paymentInfo);

        Task<List<PaymentEntity>> GetAllPayment(long branchID);
        Task<List<PaymentEntity>> GetAllPaymentWithoutContent(long branchID);
        Task<PaymentEntity> GetPaymentByID(long paymentID);

        ResponseModel RemovePayment(long paymentID, string lastupdatedby);
    }
}
