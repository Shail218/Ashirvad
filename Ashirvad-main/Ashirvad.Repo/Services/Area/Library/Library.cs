using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Library
{
    public class Library : ModelAccess, ILibraryAPI
    {
        private readonly ILibrary1API _library;

        public Library(ILibrary1API library)
        {
            _library = library;
        }

        public async Task<long> LibraryMaintenance(LibraryEntity libraryInfo)
        {
            Model.LIBRARY_MASTER libraryMaster = new Model.LIBRARY_MASTER();
            bool isUpdate = true;
            var data = (from lib in this.context.LIBRARY_MASTER
                        where lib.library_id == libraryInfo.LibraryID
                        select new
                        {
                            libraryMaster = lib
                        }).FirstOrDefault();
            if (data == null)
            {
                libraryMaster = new Model.LIBRARY_MASTER();
                isUpdate = false;
            }
            else
            {
                libraryMaster = data.libraryMaster;
                libraryInfo.Transaction.TransactionId = data.libraryMaster.trans_id.Value;
            }

            libraryMaster.row_sta_cd = libraryInfo.RowStatus.RowStatusId;
            libraryMaster.trans_id = this.AddTransactionData(libraryInfo.Transaction);
            libraryMaster.branch_id = libraryInfo.BranchID;
            libraryMaster.doc_desc = libraryInfo.Description;
            libraryMaster.std_id = libraryInfo.StandardID;
            libraryMaster.subject_id = libraryInfo.SubjectID;
            libraryMaster.thumbnail_img = libraryInfo.ThumbnailFileName;
            libraryMaster.thumbnail_path = libraryInfo.ThumbnailFilePath;
            libraryMaster.library_image = libraryInfo.DocFileName;
            libraryMaster.library_path = libraryInfo.DocFilePath;
            libraryMaster.category_id = libraryInfo.CategoryInfo.CategoryID;
            libraryMaster.library_type = libraryInfo.Library_Type;
            libraryMaster.type = libraryInfo.Type;
            libraryMaster.library_title = libraryInfo.LibraryTitle;
            libraryMaster.video_link = libraryInfo.VideoLink;
            this.context.LIBRARY_MASTER.Add(libraryMaster);
            if (isUpdate)
            {
                this.context.Entry(libraryMaster).State = System.Data.Entity.EntityState.Modified;
            }
            var result = this.context.SaveChanges();
            if (libraryMaster.branch_id == 0 && result > 0)
            {
                libraryInfo.LibraryID = libraryMaster.library_id;
                LibraryMasterMaintenance(libraryInfo);
            }
            return this.context.SaveChanges() > 0 ? libraryMaster.library_id : 0;
        }

        public async Task<long> LibraryMasterMaintenance(LibraryEntity libraryEntity)
        {
            try
            {
                long result = 0;
                LibraryEntity1 library1 = new LibraryEntity1();
                long? branch = libraryEntity.BranchID;
                library1.LibraryID = libraryEntity.LibraryID;
                library1.Title = libraryEntity.LibraryTitle;
                library1.link = libraryEntity.VideoLink;
                library1.Type = libraryEntity.Library_Type;
                library1.FileName = libraryEntity.ThumbnailFileName;
                library1.FilePath = libraryEntity.ThumbnailFilePath;
                library1.Description = libraryEntity.Description;
                library1.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active
                };
                library1.Transaction = libraryEntity.Transaction;
                library1.CategoryInfo = new CategoryEntity()
                {
                    CategoryID = libraryEntity.CategoryInfo.CategoryID
                };
                library1.BranchInfo = new BranchEntity()
                {
                    BranchID = branch.Value
                };
                result = _library.LibraryMaintenance(library1).Result;
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<LibraryEntity>> GetAllLibrary(long branchID, long stdID)
        {
            var data = (from u in this.context.LIBRARY_MASTER
                        where (0 == u.branch_id || u.branch_id == branchID) &&
                        (0 == stdID || u.std_id == stdID)
                        select new LibraryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            LibraryID = u.library_id,
                            BranchID = u.branch_id,
                            Description = u.doc_desc,
                            StandardID = u.std_id,
                            SubjectID = u.subject_id,
                            ThumbnailFileName = u.thumbnail_img,
                            ThumbnailFilePath = "http://highpack-001-site12.dtempurl.com" + u.thumbnail_path,
                            DocFileName = u.library_image,
                            DocFilePath = "http://highpack-001-site12.dtempurl.com" + u.library_path,
                            CategoryInfo = new CategoryEntity()
                            {
                                CategoryID = u.category_id.HasValue ? 0 : u.category_id.Value
                            },
                            Type = u.type.Value,
                            Library_Type = u.library_type.Value,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value },
                        }).ToList();
            return data;
        }

        public async Task<List<LibraryEntity>> GetAllLibraryWithoutContent(long branchID, long stdID)
        {
            var data = (from u in this.context.LIBRARY_MASTER
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempBranch
                        from branch in tempBranch.DefaultIfEmpty()
                        where (0 == branchID || u.branch_id == null || u.branch_id == 0 || u.branch_id == branchID) &&
                        (0 == stdID || u.std_id == stdID || u.std_id == null || u.std_id == 0) && u.row_sta_cd == 1
                        select new LibraryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            LibraryID = u.library_id,
                            BranchID = u.branch_id,
                            Description = u.doc_desc,
                            StandardID = u.std_id,
                            SubjectID = u.subject_id,
                            ThumbnailFileName = u.thumbnail_img,
                            ThumbnailFilePath = "http://highpack-001-site12.dtempurl.com" + u.thumbnail_path,
                            DocFileName = u.library_image,
                            DocFilePath = "http://highpack-001-site12.dtempurl.com" + u.library_path,
                            CategoryInfo = new CategoryEntity()
                            {
                                CategoryID = u.category_id.Value
                            },
                            Library_Type = u.library_type.Value,
                            Type = u.type.Value,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value },
                            BranchData = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                        }).ToList();


            return data;
        }

        public async Task<LibraryEntity> GetLibraryByLibraryID(long library)
        {
            var data = (from u in this.context.LIBRARY_MASTER
                        where u.library_id == library
                        select new LibraryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            LibraryID = u.library_id,
                            BranchID = u.branch_id,
                            Description = u.doc_desc,
                            StandardID = u.std_id,
                            SubjectID = u.subject_id,
                            ThumbnailFileName = u.thumbnail_img,
                            ThumbnailFilePath = "http://highpack-001-site12.dtempurl.com" + u.thumbnail_path,
                            DocFileName = u.library_image,
                            DocFilePath = "http://highpack-001-site12.dtempurl.com" + u.library_path,
                            CategoryInfo = new CategoryEntity()
                            {
                                CategoryID = u.category_id.Value
                            },
                            Library_Type = u.library_type.Value,
                            Type = u.type.Value,
                            LibraryTitle = u.library_title,
                            VideoLink = u.video_link,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value }
                        }).FirstOrDefault();
            return data;
        }

        public async Task<List<LibraryEntity>> GetAllLibrary(int Type, long BranchID)
        {
            var data = (from u in this.context.LIBRARY_MASTER
                        where u.row_sta_cd == 1 && u.library_type == Type && (u.branch_id == BranchID || BranchID == 0)
                        select new LibraryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            LibraryID = u.library_id,
                            BranchID = u.branch_id,
                            VideoLink = u.video_link,
                            LibraryTitle = u.library_title,
                            ThumbnailFileName = u.thumbnail_img,
                            ThumbnailFilePath = "http://highpack-001-site12.dtempurl.com" + u.thumbnail_path,
                            Type = u.type.Value,
                            StandardID = u.std_id,
                            SubjectID = u.subject_id,
                            Description = u.doc_desc,
                            DocFileName = u.library_image,
                            DocFilePath = "http://highpack-001-site12.dtempurl.com" + u.library_path,                            
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value },
                            CategoryInfo = new CategoryEntity()
                            {
                                CategoryID = u.category_id.HasValue ? 0 : u.category_id.Value,
                            }
                        }).ToList();

            return data;
        }

        public bool RemoveLibrary(long libraryID, string lastupdatedby)
        {
            var data = (from u in this.context.LIBRARY_MASTER
                        where u.library_id == libraryID
                        select u).FirstOrDefault();
            if (data != null)
            {
                data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id.Value, LastUpdateBy = lastupdatedby });
                this.context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
