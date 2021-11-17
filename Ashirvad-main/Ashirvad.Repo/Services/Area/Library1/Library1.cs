using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Ashirvad.Repo.Services.Area
{
    public class Library1 : ModelAccess, ILibrary1API
    {
        public async Task<long> LibraryMaintenance(LibraryEntity1 LibraryInfo)
        {
            Model.LIBRARY1_MASTER LibraryMaster = new Model.LIBRARY1_MASTER();
            bool isUpdate = true;
            var data = (from Library in this.context.LIBRARY1_MASTER
                        where Library.library_id == LibraryInfo.LibraryID
                        select new
                        {
                            LibraryMaster = Library
                        }).FirstOrDefault();
            if (data == null)
            {
                LibraryMaster = new Model.LIBRARY1_MASTER();
                isUpdate = false;
            }
            else
            {
                LibraryMaster = data.LibraryMaster;
                LibraryInfo.Transaction.TransactionId = data.LibraryMaster.trans_id;
            }

            LibraryMaster.branch_id = LibraryInfo.BranchInfo.BranchID;
            LibraryMaster.library_title = LibraryInfo.Title;
            LibraryMaster.video_link = LibraryInfo.link;
            LibraryMaster.description = LibraryInfo.Description;
            LibraryMaster.category_id = LibraryInfo.CategoryInfo.CategoryID;
            LibraryMaster.row_sta_cd = LibraryInfo.RowStatus.RowStatusId;
            LibraryMaster.trans_id = this.AddTransactionData(LibraryInfo.Transaction);
            this.context.LIBRARY1_MASTER.Add(LibraryMaster);
            if (isUpdate)
            {
                this.context.Entry(LibraryMaster).State = System.Data.Entity.EntityState.Modified;
            }
            var result = this.context.SaveChanges();
            if (result > 0)
            {
                LibraryInfo.LibraryID = LibraryMaster.library_id;
                var result2 = LibraryDetailMaintenance(LibraryInfo).Result;
                return result2 > 0 ? LibraryInfo.LibraryID : 0;
            }
            return this.context.SaveChanges() > 0 ? LibraryMaster.library_id : 0;
        }

        public async Task<long> LibraryDetailMaintenance(LibraryEntity1 LibraryInfo)
        {
            Model.LIBRARY_MASTER_DTL LibraryMaster = new Model.LIBRARY_MASTER_DTL();
            bool isUpdate = true;
            var data = (from Library in this.context.LIBRARY_MASTER_DTL
                        where Library.library_dtl_id == LibraryInfo.LibraryDetailID
                        select new
                        {
                            LibraryMaster = Library
                        }).FirstOrDefault();
            if (data == null)
            {
                LibraryMaster = new Model.LIBRARY_MASTER_DTL();
                isUpdate = false;
            }
            else
            {
                LibraryMaster = data.LibraryMaster;
                LibraryInfo.Transaction.TransactionId = data.LibraryMaster.trans_id;
            }

            LibraryMaster.library_content = LibraryInfo.FileContent;
            LibraryMaster.type = LibraryInfo.Type;
            LibraryMaster.library_id = LibraryInfo.LibraryID;
            LibraryMaster.library_name = LibraryInfo.FileName;
            LibraryMaster.library_filepath = LibraryInfo.FilePath;
            LibraryMaster.row_sta_cd = LibraryInfo.RowStatus.RowStatusId;
            LibraryMaster.branch_id = LibraryInfo.BranchInfo.BranchID;
            LibraryMaster.trans_id = this.AddTransactionData(LibraryInfo.Transaction);
            this.context.LIBRARY_MASTER_DTL.Add(LibraryMaster);
            if (isUpdate)
            {
                this.context.Entry(LibraryMaster).State = System.Data.Entity.EntityState.Modified;
            }

            return this.context.SaveChanges() > 0 ? LibraryMaster.library_dtl_id : 0;
        }

        public async Task<List<LibraryEntity1>> GetAllLibrary(int Type, int BranchID)
        {
            var data = (from u in this.context.LIBRARY1_MASTER
                        join b in this.context.LIBRARY_MASTER_DTL on u.library_id equals b.library_id
                        where (u.row_sta_cd == 1 && b.type == Type) && (u.branch_id == BranchID || BranchID == 0)
                        select new LibraryEntity1()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            LibraryID = u.library_id,
                            link = u.video_link,
                            Title = u.library_title,
                            Description = u.description,
                            FilePath = "http://highpack-001-site12.dtempurl.com" + b.library_filepath,
                            LibraryDetailID = b.library_dtl_id,
                            FileName = b.library_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            CategoryInfo = new CategoryEntity() { CategoryID = u.category_id, Category = u.CATEGORY_MASTER.category_name },
                            BranchInfo = new BranchEntity() {                      
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name}
                        }).ToList();

            return data;
        }

        public Task<List<LibraryEntity1>> GetAllLibraryWithoutImage()
        {
            throw new NotImplementedException();
        }

        public async Task<LibraryEntity1> GetLibraryByLibraryID(long LibraryID)
        {
            var data = (from u in this.context.LIBRARY1_MASTER
                        join b in this.context.LIBRARY_MASTER_DTL on u.library_id equals b.library_id
                        where u.library_id == LibraryID
                        select new LibraryEntity1()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            LibraryID = u.library_id,
                            Title = u.library_title,
                            Description = u.description,
                            link = u.video_link,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            CategoryInfo = new CategoryEntity() { CategoryID = u.category_id },
                            LibraryDetailID = b.library_dtl_id,
                            FilePath = b.library_filepath,
                            FileName = b.library_name,
                            FileContent = b.library_content
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveLibrary(long LibraryID, string lastupdatedby)
        {
            var data = (from u in this.context.LIBRARY1_MASTER
                        where u.library_id == LibraryID
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
