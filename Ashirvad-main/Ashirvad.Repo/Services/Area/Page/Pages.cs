using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Page;
using Ashirvad.Repo.DataAcceessAPI.Area.RoleRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Page
{
    public class Pages : ModelAccess, IPageAPI
    {
        private readonly IPackageRightsAPI _packageRights;
        private readonly IRoleRightsAPI _roleRights;

        public Pages(IPackageRightsAPI packageRights, IRoleRightsAPI roleRights)
        {
            this._packageRights = packageRights;
            this._roleRights = roleRights;
        }

        public async Task<long> CheckPage(string name, long branch, long Id)
        {
            long result;
            bool isExists = this.context.PAGE_MASTER.Where(s => (Id == 0 || s.page_id != Id) && s.page == name && s.branch_id == branch && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> PageMaintenance(PageEntity pageInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.PAGE_MASTER pageMaster = new Model.PAGE_MASTER();

                if (CheckPage(pageInfo.Page, pageInfo.BranchInfo.BranchID, pageInfo.PageID).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from page in this.context.PAGE_MASTER
                                where page.page_id == pageInfo.PageID
                                select page).FirstOrDefault();
                    if (data == null)
                    {
                        pageMaster = new Model.PAGE_MASTER();

                        isUpdate = false;
                    }
                    else
                    {
                        pageMaster = data;
                        pageInfo.Transaction.TransactionId = data.trans_id;
                    }

                    pageMaster.page = pageInfo.Page;
                    pageMaster.branch_id = pageInfo.BranchInfo.BranchID;
                    pageMaster.row_sta_cd = pageInfo.RowStatus.RowStatusId;
                    pageMaster.trans_id = this.AddTransactionData(pageInfo.Transaction);
                    this.context.PAGE_MASTER.Add(pageMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(pageMaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    var Result = this.context.SaveChanges();
                    //  return this.context.SaveChanges() > 0 ? pageMaster.page_id : 0;
                    if (Result > 0)
                    {
                        pageInfo.PageID = pageMaster.page_id;
                        pageInfo.Transaction.TransactionId = pageMaster.trans_id;
                        PageMasterMaintenance(pageInfo);
                    }
                    var res= Result > 0 ? pageInfo.PageID : 0;
                    if (res > 0)
                    {
                        pageInfo.PageID = pageMaster.page_id;
                        responseModel.Data = pageInfo;
                        responseModel.Status = true;
                        responseModel.Message = isUpdate == true ? "Page Updated Successfully." : "Page Inserted Successfully.";
                    }
                    else
                    {
                        responseModel.Status = false;
                        responseModel.Message = isUpdate == true ? "Page Not Updated." : "Page Not Inserted.";
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Page Already Exists.";
                    //return -1;
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
        }

        public async Task<List<PageEntity>> GetAllPages(long branchID)
        {
            var data = (from u in this.context.PAGE_MASTER orderby u.page_id descending
                        where u.row_sta_cd == 1
                        select new PageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Page = u.page,
                            PageID = u.page_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<PageEntity>> GetAllCustomPages(DataTableAjaxPostModel model)
        {
            var Result = new List<PageEntity>();
            long count = this.context.PAGE_MASTER.Where(s => s.row_sta_cd == 1).Distinct().ToList().Count;
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            var data = (from u in this.context.PAGE_MASTER orderby u.page_id descending                   
                        where u.row_sta_cd == 1
                        && (model.search.value == null 
                        || model.search.value=="" 
                        || u.page.ToLower().Contains(model.search.value))
                        select new PageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Page = u.page,
                            PageID = u.page_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Count=count,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        })
                        // have to give a default order when skipping .. so use the PK
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }
        public async Task<List<PageEntity>> GetAllPages()
        {
            var data = (from u in this.context.PAGE_MASTER orderby u.page_id descending
                        select new PageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Page = u.page,
                            PageID = u.page_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public ResponseModel RemovePage(long PageID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.PAGE_MASTER
                            where u.page_id == PageID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    //return true;
                    responseModel.Status = true;
                    responseModel.Message = "Page Removed Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Page Not Found.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
            //return false;
        }


        public async Task<PageEntity> GetPageByID(long pageID)
        {
            var data = (from u in this.context.PAGE_MASTER
                        where u.page_id == pageID
                        select new PageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Page = u.page,
                            PageID = u.page_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }

        public async Task<ResponseModel> PageMasterMaintenance(PageEntity pageEntity)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                long result = 0;
                var data = (from Package in this.context.PACKAGE_RIGHTS_MASTER
                            where Package.row_sta_cd==1
                            select new PackageEntity
                            {
                                PackageID = Package.package_id
                            }).Distinct().ToList();
                var data2 = (from Role in this.context.ROLE_RIGHTS_MASTER
                             where Role.row_sta_cd == 1
                             select new RoleEntity
                             {
                                 RoleID = Role.role_id
                             }).Distinct().ToList();

                PackageRightEntity packageRight = new PackageRightEntity();
               
                packageRight.PageInfo = new PageEntity()
                {
                    PageID = pageEntity.PageID,
                    Page = pageEntity.Page
                };
                
                packageRight.Transaction = new TransactionEntity();
                packageRight.Transaction = pageEntity.Transaction;
                packageRight.Createstatus = false;
                packageRight.Deletestatus = false;
                packageRight.Viewstatus = false;
                packageRight.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active,
                    RowStatus = Enums.RowStatus.Active
                };
               
                foreach (var item in data)
                {
                    packageRight.Packageinfo = new PackageEntity()
                    {
                        PackageID = item.PackageID,

                    };
                    packageRight.PackageRightsId = 0;
                    responseModel = _packageRights.RightsMaintenance(packageRight).Result;
                    //var da = _packageRights.RightsMaintenance(packageRight).Result;
                }
                RoleRightsEntity roleRight = new RoleRightsEntity();
                roleRight.PageInfo = new PageEntity()
                {
                    PageID = pageEntity.PageID,
                    Page = pageEntity.Page
                };
                roleRight.Transaction = new TransactionEntity();
                roleRight.Transaction = pageEntity.Transaction;
                roleRight.Createstatus = false;
                roleRight.Deletestatus = false;
                roleRight.Viewstatus = false;
                roleRight.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active,
                    RowStatus = Enums.RowStatus.Active
                };
                foreach(var item2 in data2)
                {
                    roleRight.Roleinfo = new RoleEntity()
                    {
                        RoleID = item2.RoleID,

                    };
                    roleRight.RoleRightsId = 0;
                    responseModel = _roleRights.RightsMaintenance(roleRight).Result;
                    //var da = _packageRights.RightsMaintenance(packageRight).Result;
                }
                //return result;
                return responseModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }      
    }
}
