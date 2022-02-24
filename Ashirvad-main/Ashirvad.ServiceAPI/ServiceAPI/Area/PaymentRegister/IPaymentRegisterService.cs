using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.PaymentRegister
{
    public interface IPaymentRegisterService
    {
        Task<PaymentRegisterEntity> PaymentRegisterMaintenance(PaymentRegisterEntity entity);
        Task<List<PaymentRegisterEntity>> GetPaymentListForStudent(long studentID, long branchID);
    }
}
