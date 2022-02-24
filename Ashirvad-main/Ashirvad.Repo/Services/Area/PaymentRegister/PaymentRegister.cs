using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.PaymentRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.PaymentRegister
{
    public class PaymentRegister : ModelAccess, IPaymentRegisterAPI
    {
        public async Task<long> PaymentRegisterMaintenance(PaymentRegisterEntity entity)
        {
            Model.PAYMENT_MASTER payment = new Model.PAYMENT_MASTER();
            bool isUpdate = true;
            var data = (from t in this.context.PAYMENT_MASTER
                        where t.payment_id == entity.payment_id
                        select t).FirstOrDefault();
            if (data == null)
            {
                data = new Model.PAYMENT_MASTER();
                isUpdate = false;
            }
            else
            {
                data = new Model.PAYMENT_MASTER();
                isUpdate = false;

            }
            payment.row_sta_cd = entity.RowStatus.RowStatusId;
            payment.trans_id = this.AddTransactionData(entity.Transaction);
            payment.student_id = entity.studentEntity.StudentID;
            payment.branch_id = entity.branchEntity.BranchID;
            payment.file_path = entity.file_path;
            payment.file_name = entity.file_name;
            payment.remark = entity.remark;
            payment.payment_status = entity.payment_status;
            payment.extra1 = entity.student_remark;
            this.context.PAYMENT_MASTER.Add(payment);
            if (isUpdate)
            {
                this.context.Entry(payment).State = System.Data.Entity.EntityState.Modified;
            }

            return this.context.SaveChanges() > 0 ? payment.payment_id : 0;
        }

        public async Task<List<PaymentRegisterEntity>> GetPaymentListForStudent(long studentID,long branchID)
        {

            var data = (from u in this.context.PAYMENT_MASTER
                        orderby u.payment_id descending
                        where u.student_id == studentID && u.branch_id == branchID && u.row_sta_cd == 1
                        select new PaymentRegisterEntity()
                        {
                            studentEntity = new StudentEntity()
                            {
                                StudentID = u.student_id,
                                Name = u.STUDENT_MASTER.first_name + " " + u.STUDENT_MASTER.last_name
                            },
                            branchEntity = new BranchEntity()
                            {
                                BranchID = u.branch_id
                            },
                            file_name = u.file_name,
                            file_path = "https://mastermind.org.in" + u.file_path,
                            remark = u.remark,
                            student_remark = u.extra1,
                            payment_status = u.payment_status.HasValue ? u.payment_status.Value : 0,
                            status_txt = u.payment_status == 1 ? "Pending" : u.payment_status == 2 ? "Approved" : "Rejected",
                        }).Distinct().ToList();
            return data;
        }
    }
}
