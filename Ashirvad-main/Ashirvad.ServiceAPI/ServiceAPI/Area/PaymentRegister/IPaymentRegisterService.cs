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
        Task<ResponseModel> PaymentRegisterMaintenance(PaymentRegisterEntity entity);
        Task<List<PaymentRegisterEntity>> GetPaymentListForStudent(long studentID, long branchID);
        Task<List<PaymentRegisterEntity>> GetPaymentRegisterList( long BranchID, long CourseID = 0, long ClassID = 0, long studentID = 0);
        Task<ResponseModel> UpdatePaymentRegisterbyAdmin(PaymentRegisterEntity entity);
    }
}
