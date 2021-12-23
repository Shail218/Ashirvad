using Ashirvad.Data;
using Ashirvad.Repo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo
{
    public abstract class ModelAccess : IDisposable
    {
        public AshirvadDBEntities1 context;
        private bool _disposed;
        public ModelAccess()
        {
            context = new AshirvadDBEntities1();
        }

        public long AddTransactionData(TransactionEntity transactionData)
        {
            bool isUpdate = false;
            var transData = (from t in this.context.TRANSACTION_MASTER
                             where t.trans_id == transactionData.TransactionId
                             select t).FirstOrDefault();
            if (transData == null)
            {
                transData = new TRANSACTION_MASTER();
            }
            else
            {
                isUpdate = true;
            }
            if (!isUpdate)
            {
                transData.created_by = transactionData.CreatedBy;
                transData.created_dt = DateTime.Now;
                transData.created_id = transactionData.CreatedId;
                transData.last_mod_by = transactionData.CreatedBy;
            }
            if (isUpdate)
            {
                transData.last_mod_by = transactionData.LastUpdateBy;
                transData.last_mod_dt = DateTime.Now;
                transData.last_mod_id = transactionData.LastUpdateId;
            }

            this.context.TRANSACTION_MASTER.Add(transData);
            if (isUpdate)
            {
                this.context.Entry(transData).State = System.Data.Entity.EntityState.Modified;
            }
            this.context.SaveChanges();
            return transData.trans_id;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                    // Dispose other managed resources.
                }
                //release unmanaged resources.
            }
            _disposed = true;
        }


    }
}
