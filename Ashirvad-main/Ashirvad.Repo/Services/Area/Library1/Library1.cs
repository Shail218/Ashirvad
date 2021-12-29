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
            Model.NEW_LIBRARY_MASTER LibraryMaster = new Model.NEW_LIBRARY_MASTER();
            LibraryInfo.branchid = LibraryInfo.BranchInfo.BranchID;
            if(LibraryInfo.BranchInfo.BranchID == 0)
            {
                LibraryInfo.branchid = null;
            }
            bool isUpdate = true;
            var data = (from Library in this.context.NEW_LIBRARY_MASTER
                        where Library.new_library_id == LibraryInfo.NewLibraryID
                        select new
                        {
                            LibraryMaster = Library
                        }).FirstOrDefault();
            if (data == null)
            {
                LibraryMaster = new Model.NEW_LIBRARY_MASTER();
                isUpdate = false;
            }
            else
            {
                LibraryMaster = data.LibraryMaster;
                LibraryInfo.Transaction.TransactionId = data.LibraryMaster.trans_id;
            }
            LibraryMaster.library_id = LibraryInfo.LibraryID;
            LibraryMaster.branch_id = LibraryInfo.branchid;
            LibraryMaster.library_title = LibraryInfo.Title;
            LibraryMaster.video_link = LibraryInfo.link;
            LibraryMaster.description = LibraryInfo.Description;
            LibraryMaster.category_id = LibraryInfo.CategoryInfo.CategoryID;
            LibraryMaster.type = LibraryInfo.Type;
            LibraryMaster.thumbnail_img = LibraryInfo.FileName;
            LibraryMaster.thumbnail_path = LibraryInfo.FilePath;
            LibraryMaster.row_sta_cd = LibraryInfo.RowStatus.RowStatusId;
            LibraryMaster.trans_id = this.AddTransactionData(LibraryInfo.Transaction);
            this.context.NEW_LIBRARY_MASTER.Add(LibraryMaster);
            if (isUpdate)
            {
                this.context.Entry(LibraryMaster).State = System.Data.Entity.EntityState.Modified;
            }
            
            return this.context.SaveChanges() > 0 ? LibraryMaster.new_library_id : 0;
        }

        public async Task<List<LibraryEntity1>> GetAllLibrary(int Type, int BranchID)
        {
            var data = (from u in this.context.NEW_LIBRARY_MASTER orderby u.new_library_id descending
                        where (u.row_sta_cd == 1 && u.type == Type) && (u.branch_id == BranchID || BranchID == 0)
                        select new LibraryEntity1()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            NewLibraryID = u.new_library_id,
                            LibraryID = u.library_id.HasValue ? 0 : u.library_id.Value,
                            link = u.video_link,
                            Title = u.library_title,
                            Description = u.description,
                            FilePath = "http://highpack-001-site12.dtempurl.com" + u.thumbnail_path,                            
                            FileName = u.thumbnail_img,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            CategoryInfo = new CategoryEntity() {
                                CategoryID = u.category_id.HasValue ? 0 : u.category_id.Value,
                                Category = u.CATEGORY_MASTER.category_name },
                            BranchInfo = new BranchEntity() {                      
                                BranchID = u.branch_id.HasValue ? 0 : u.branch_id.Value,
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
            var data = (from u in this.context.NEW_LIBRARY_MASTER
                        where u.new_library_id == LibraryID
                        select new LibraryEntity1()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            NewLibraryID = u.new_library_id,
                            LibraryID = u.library_id.HasValue ? 0 : u.library_id.Value,
                            Title = u.library_title,
                            Description = u.description,
                            link = u.video_link,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchInfo = new BranchEntity() {
                                BranchID = u.branch_id.HasValue ? 0 : u.branch_id.Value},
                            CategoryInfo = new CategoryEntity() { CategoryID = u.category_id.HasValue ? 0 : u.category_id.Value },
                            FilePath = u.thumbnail_path,
                            FileName = u.thumbnail_img,
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveLibrary(long LibraryID, string lastupdatedby)
        {
            var data = (from u in this.context.NEW_LIBRARY_MASTER
                        where u.new_library_id == LibraryID
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
