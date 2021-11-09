using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area
{
    public class Category : ModelAccess, ICategoryAPI
    {
        public async Task<long> CheckCategory(string name,long branch, long Id)
        {
            long result;
            bool isExists = this.context.CATEGORY_MASTER.Where(s => (Id == 0 || s.category_id != Id) && s.category_name == name && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> CategoryMaintenance(CategoryEntity CategoryInfo)
        {
            Model.CATEGORY_MASTER CategoryMaster = new Model.CATEGORY_MASTER();
            if (CheckCategory(CategoryInfo.Category, CategoryInfo.BranchInfo.BranchID, CategoryInfo.CategoryID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from Category in this.context.CATEGORY_MASTER
                            where Category.category_id == CategoryInfo.CategoryID
                            select Category).FirstOrDefault();
                if (data == null)
                {
                    CategoryMaster = new Model.CATEGORY_MASTER();
                    isUpdate = false;
                }

                else
                {
                    CategoryMaster = data;
                    CategoryInfo.Transaction.TransactionId = CategoryMaster.trans_id;
                }
                CategoryMaster.category_name = CategoryInfo.Category;
                CategoryMaster.branch_id = CategoryInfo.BranchInfo.BranchID;
                CategoryMaster.row_sta_cd = CategoryInfo.RowStatus.RowStatusId;
                CategoryMaster.trans_id = this.AddTransactionData(CategoryInfo.Transaction);
                this.context.CATEGORY_MASTER.Add(CategoryMaster);
                if (isUpdate)
                {
                    this.context.Entry(CategoryMaster).State = System.Data.Entity.EntityState.Modified;
                }
                return this.context.SaveChanges() > 0 ? CategoryMaster.category_id : 0;
            }
            else
            {
                return -1;
            }
        }

        public async Task<List<CategoryEntity>> GetAllCategorys(long branchID)
        {
            var data = (from u in this.context.CATEGORY_MASTER
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new CategoryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },

                            Category = u.category_name,
                            CategoryID = u.category_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<CategoryEntity>> GetAllCategorys()
        {
            var data = (from u in this.context.CATEGORY_MASTER
                        select new CategoryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },

                            Category = u.category_name,
                            CategoryID = u.category_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public bool RemoveCategory(long CategoryID, string lastupdatedby)
        {
            var data = (from u in this.context.CATEGORY_MASTER
                        where u.category_id == CategoryID
                        select u).FirstOrDefault();
            if (data != null)
            {
                data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                data.trans_id = this.AddTransactionData(new TransactionEntity()
                {
                    TransactionId = data.trans_id,
                    LastUpdateBy = lastupdatedby
                });
                this.context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<CategoryEntity> GetCategorysByID(long branchID)
        {
            var data = (from u in this.context.CATEGORY_MASTER
                        where u.category_id == branchID
                        select new CategoryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },

                            Category = u.category_name,
                            CategoryID = u.category_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }
    }
}
