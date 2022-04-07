using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

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

        public async Task<ResponseModel> CategoryMaintenance(CategoryEntity CategoryInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
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
                    var res = this.context.SaveChanges() > 0 ? CategoryMaster.category_id : 0;
                    if (res > 0)
                    {
                        CategoryInfo.CategoryID = CategoryMaster.category_id;
                        responseModel.Data = CategoryInfo;
                        responseModel.Status = true;
                        responseModel.Message = isUpdate==true?"Category Updated Successfully.":"Category Inserted Successfully.";
                    }
                    else
                    {
                        responseModel.Status = false;
                        responseModel.Message = isUpdate == true ? "Category Not Updated." : "Category Not Inserted.";
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Category Already Exists.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
           
            
        }

        public async Task<List<CategoryEntity>> GetAllCategorys(long branchID)
        {
            var data = (from u in this.context.CATEGORY_MASTER orderby u.category_id descending
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

        public async Task<List<CategoryEntity>> GetAllCustomCategory(DataTableAjaxPostModel model, long branchID)
        {
            var Result = new List<CategoryEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = this.context.CATEGORY_MASTER.Where(s => s.row_sta_cd == 1 && (s.branch_id == branchID || branchID == 0)).Count();
            var data = (from u in this.context.CATEGORY_MASTER where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.category_name.ToLower().Contains(model.search.value))
                        orderby u.category_id descending
                        select new CategoryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Count = count,
                            Category = u.category_name,
                            CategoryID = u.category_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }

        public async Task<List<CategoryEntity>> GetAllCategorys()
        {
            var data = (from u in this.context.CATEGORY_MASTER
                        orderby u.category_id descending
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

        public ResponseModel RemoveCategory(long CategoryID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            Check_Delete check = new Check_Delete();
            string message = "";
            try
            {
                var data = (from u in this.context.CATEGORY_MASTER
                            where u.category_id == CategoryID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    var data1 = (from a in this.context.LIBRARY_MASTER
                                 where a.category_id == data.category_id && a.row_sta_cd == 1
                                 select a).ToList();
                    foreach (var item in data1)
                    {
                        var data_course = check.check_remove_category_superadmin(item.category_id.Value).Result;
                        if (data_course.Status)
                        {
                            data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                            data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                            item.row_sta_cd = (int)Enums.RowStatus.Inactive;
                            item.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = item.trans_id.Value, LastUpdateBy = lastupdatedby });
                            this.context.SaveChanges();
                        }
                        else
                        {
                            message = message + data_course.Message;
                            break;
                        }
                    }
                    responseModel.Status = message == "" ? true : false;
                    responseModel.Message = message == "" ? "Category Removed Successfully!!" : message;
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
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
