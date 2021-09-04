using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Payment
{
    public class StudentPayment : ModelAccess, IStudentPaymentAPI
    {
        public async Task<long> PaymentMaintenance(PaymentEntity paymentInfo)
        {
            Model.STUDENT_PAYMENT_MASTER paymentMaster = new Model.STUDENT_PAYMENT_MASTER();
            Model.STUDENT_PAYMENT_REL paymentData = new Model.STUDENT_PAYMENT_REL();
            bool isUpdate = true;
            var data = (from payment in this.context.STUDENT_PAYMENT_MASTER.Include("STUDENT_PAYMENT_REL")
                        where payment.payment_id == paymentInfo.PaymentID
                        select new
                        {
                            paymentMaster = payment,
                            paymentData = payment.STUDENT_PAYMENT_REL
                        }).FirstOrDefault();
            if (data == null)
            {
                paymentMaster = new Model.STUDENT_PAYMENT_MASTER();
                paymentData = new Model.STUDENT_PAYMENT_REL();
                isUpdate = false;
            }
            else
            {
                paymentMaster = data.paymentMaster;
                paymentData = data.paymentMaster.STUDENT_PAYMENT_REL.FirstOrDefault();
                paymentInfo.TransactionData.TransactionId = data.paymentMaster.trans_id;
            }

            paymentMaster.row_sta_cd = paymentInfo.RowStatusData.RowStatusId;
            paymentMaster.trans_id = this.AddTransactionData(paymentInfo.TransactionData);
            paymentMaster.branch_id = paymentInfo.BranchData.BranchID;
            paymentMaster.admin_remark = paymentInfo.AdminRemarks;
            paymentMaster.status_id = (int)paymentInfo.PaymentStatus;
            paymentMaster.student_remark = paymentInfo.StudentRemarks;
            paymentMaster.student_id = paymentInfo.StudentData.StudentID;
            this.context.STUDENT_PAYMENT_MASTER.Add(paymentMaster);
            if (isUpdate)
            {
                this.context.Entry(paymentMaster).State = System.Data.Entity.EntityState.Modified;
            }
            if (!isUpdate)
            {
                paymentData.payment_id = paymentMaster.payment_id;
            }

            paymentData.payment_content = !string.IsNullOrEmpty(paymentInfo.PaymentData.PaymentContentText) ? Convert.FromBase64String(paymentInfo.PaymentData.PaymentContentText) : paymentInfo.PaymentData.PaymentContent;
            paymentData.file_name = paymentInfo.PaymentData.PaymentContentFileName;
            paymentData.trans_id = paymentInfo.TransactionData.TransactionId;
            paymentData.row_sta_cd = paymentInfo.RowStatusData.RowStatusId;
            this.context.STUDENT_PAYMENT_REL.Add(paymentData);
            if (isUpdate)
            {
                this.context.Entry(paymentData).State = System.Data.Entity.EntityState.Modified;
            }
            return this.context.SaveChanges() > 0 ? paymentMaster.payment_id : 0;
        }

        public async Task<List<PaymentEntity>> GetAllPayment(long branchID)
        {
            var data = (from u in this.context.STUDENT_PAYMENT_MASTER
                        .Include("STUDENT_PAYMENT_REL")
                        .Include("BRANCH_MASTER")
                        join stu in this.context.STUDENT_MASTER on u.student_id equals stu.student_id
                        join std in this.context.STD_MASTER on stu.std_id equals std.std_id
                        where (0 == u.branch_id || u.branch_id == branchID)
                        && u.row_sta_cd == 1
                        select new PaymentEntity()
                        {
                            RowStatusData = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            PaymentData = new PaymentContentEntity()
                            {
                                PaymentContent = u.STUDENT_PAYMENT_REL.FirstOrDefault().payment_content,
                                PaymentContentFileName = u.STUDENT_PAYMENT_REL.FirstOrDefault().file_name,
                                ContentID = u.STUDENT_PAYMENT_REL.FirstOrDefault().payment_rel_id
                            },
                            AdminRemarks = u.admin_remark,
                            BranchData = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            PaymentID = u.payment_id,
                            PaymentStatus = u.status_id == 1 ? Enums.PaymentStatus.Approve : u.status_id == 2 ? Enums.PaymentStatus.Reject : Enums.PaymentStatus.Pending,
                            StudentData = new StudentEntity()
                            {
                                StudentID = stu.student_id,
                                FirstName = stu.first_name,
                                LastName = stu.last_name,
                                StandardInfo = new StandardEntity()
                                {
                                    StandardID = std.std_id,
                                    Standard = std.standard
                                }
                            },
                            StudentRemarks = u.student_remark
                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].PaymentData.PaymentContentText = data[idx].PaymentData.PaymentContent.Length > 0 ? Convert.ToBase64String(data[idx].PaymentData.PaymentContent) : "";
                }
            }

            return data;
        }

        public async Task<List<PaymentEntity>> GetAllPaymentWithoutContent(long branchID)
        {
            var data = (from u in this.context.STUDENT_PAYMENT_MASTER
                        .Include("STUDENT_PAYMENT_REL")
                        .Include("BRANCH_MASTER")
                        join stu in this.context.STUDENT_MASTER on u.student_id equals stu.student_id
                        join std in this.context.STD_MASTER on stu.std_id equals std.std_id
                        where (0 == u.branch_id || u.branch_id == branchID)
                        && u.row_sta_cd == 1
                        select new PaymentEntity()
                        {
                            RowStatusData = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            PaymentData = new PaymentContentEntity()
                            {
                                PaymentContentFileName = u.STUDENT_PAYMENT_REL.FirstOrDefault().file_name,
                                ContentID = u.STUDENT_PAYMENT_REL.FirstOrDefault().payment_rel_id
                            },
                            AdminRemarks = u.admin_remark,
                            BranchData = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            PaymentID = u.payment_id,
                            PaymentStatus = u.status_id == 1 ? Enums.PaymentStatus.Approve : u.status_id == 2 ? Enums.PaymentStatus.Reject : Enums.PaymentStatus.Pending,
                            StudentData = new StudentEntity()
                            {
                                StudentID = stu.student_id,
                                FirstName = stu.first_name,
                                LastName = stu.last_name,
                                StandardInfo = new StandardEntity()
                                {
                                    StandardID = std.std_id,
                                    Standard = std.standard
                                }
                            },
                            StudentRemarks = u.student_remark
                        }).ToList();

            return data;
        }

        public async Task<PaymentEntity> GetPaymentByID(long paymentID)
        {
            var data = (from u in this.context.STUDENT_PAYMENT_MASTER
                        .Include("STUDENT_PAYMENT_REL")
                        .Include("BRANCH_MASTER")
                        join stu in this.context.STUDENT_MASTER on u.student_id equals stu.student_id
                        join std in this.context.STD_MASTER on stu.std_id equals std.std_id
                        where u.payment_id == paymentID
                        && u.row_sta_cd == 1
                        select new PaymentEntity()
                        {
                            RowStatusData = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            PaymentData = new PaymentContentEntity()
                            {
                                PaymentContent = u.STUDENT_PAYMENT_REL.FirstOrDefault().payment_content,
                                PaymentContentFileName = u.STUDENT_PAYMENT_REL.FirstOrDefault().file_name,
                                ContentID = u.STUDENT_PAYMENT_REL.FirstOrDefault().payment_rel_id
                            },
                            AdminRemarks = u.admin_remark,
                            BranchData = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            PaymentID = u.payment_id,
                            PaymentStatus = u.status_id == 1 ? Enums.PaymentStatus.Approve : u.status_id == 2 ? Enums.PaymentStatus.Reject : Enums.PaymentStatus.Pending,
                            StudentData = new StudentEntity()
                            {
                                StudentID = stu.student_id,
                                FirstName = stu.first_name,
                                LastName = stu.last_name,
                                StandardInfo = new StandardEntity()
                                {
                                    StandardID = std.std_id,
                                    Standard = std.standard
                                }
                            },
                            StudentRemarks = u.student_remark
                        }).FirstOrDefault();

            if (data != null)
            {
                data.PaymentData.PaymentContentText = data.PaymentData.PaymentContent.Length > 0 ? Convert.ToBase64String(data.PaymentData.PaymentContent) : "";
            }

            return data;
        }

        public bool RemovePayment(long paymentID, string lastupdatedby)
        {
            var data = (from u in this.context.STUDENT_PAYMENT_MASTER
                        where u.payment_id == paymentID
                        select u).FirstOrDefault();
            if (data != null)
            {
                data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                this.context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
