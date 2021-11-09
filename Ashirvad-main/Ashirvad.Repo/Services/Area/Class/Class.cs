using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Class
{
    public class Class : ModelAccess, IClassAPI
    {
        public async Task<long> CheckClass(string name, long Id)
        {
            long result;
            bool isExists = this.context.CLASS_MASTER.Where(s => (Id == 0 || s.class_id != Id) && s.class_name == name && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> ClassMaintenance(ClassEntity classEntity)
        {
            Model.CLASS_MASTER classMaster = new Model.CLASS_MASTER();
            if (CheckClass(classEntity.ClassName, classEntity.ClassID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from cl in this.context.CLASS_MASTER
                            where cl.class_id == classEntity.ClassID
                            select new
                            {
                                classMaster = cl
                            }).FirstOrDefault();
                if (data == null)
                {
                    classMaster = new Model.CLASS_MASTER();
                    isUpdate = false;
                }
                else
                {
                    classMaster = data.classMaster;
                    classEntity.Transaction.TransactionId = data.classMaster.trans_id;
                }
                classMaster.class_name = classEntity.ClassName;
                classMaster.row_sta_cd = classEntity.RowStatus.RowStatusId;
                classMaster.trans_id = this.AddTransactionData(classEntity.Transaction);
                this.context.CLASS_MASTER.Add(classMaster);
                if (isUpdate)
                {
                    this.context.Entry(classMaster).State = System.Data.Entity.EntityState.Modified;
                }
                return this.context.SaveChanges() > 0 ? classMaster.class_id : 0;
            }
            else
            {
                return -1;
            }
        }

        public async Task<ClassEntity> GetClassByClassID(long classID)
        {
            var data = (from u in this.context.CLASS_MASTER
                        where u.class_id == classID
                        select new ClassEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            ClassID = u.class_id,
                            ClassName = u.class_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }

        public async Task<List<ClassEntity>> GetAllClass()
        {
            var data = (from u in this.context.CLASS_MASTER
                        where u.row_sta_cd == 1
                        select new ClassEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            ClassID = u.class_id,
                            ClassName = u.class_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },

                        }).ToList();

            return data;
        }

        public bool RemoveClass(long classID, string lastupdatedby)
        {
            var data = (from u in this.context.CLASS_MASTER
                        where u.class_id == classID
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
