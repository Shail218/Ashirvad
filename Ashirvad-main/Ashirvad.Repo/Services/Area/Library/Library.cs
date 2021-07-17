using Ashirvad.Common;
using Ashirvad.Data;
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
        public async Task<long> LibraryMaintenance(LibraryEntity libraryInfo)
        {
            Model.LIBRARY_MASTER libraryMaster = new Model.LIBRARY_MASTER();
            Model.LIBRARY_DATA libData = new Model.LIBRARY_DATA();
            bool isUpdate = true;
            var data = (from lib in this.context.LIBRARY_MASTER.Include("LIBRARY_DATA")
                        where lib.library_id == libraryInfo.LibraryID
                        select new
                        {
                            libraryMaster = lib,
                            libData = lib.LIBRARY_DATA
                        }).FirstOrDefault();
            if (data == null)
            {
                libraryMaster = new Model.LIBRARY_MASTER();
                libData = new Model.LIBRARY_DATA();
                isUpdate = false;
            }
            else
            {
                libraryMaster = data.libraryMaster;
                libData = data.libraryMaster.LIBRARY_DATA.FirstOrDefault();
                libraryInfo.Transaction.TransactionId = data.libraryMaster.trans_id.Value;
            }

            libraryMaster.row_sta_cd = libraryInfo.RowStatus.RowStatusId;
            libraryMaster.trans_id = this.AddTransactionData(libraryInfo.Transaction);
            libraryMaster.branch_id = libraryInfo.BranchID;
            libraryMaster.doc_desc = libraryInfo.Description;
            libraryMaster.std_id = libraryInfo.StandardID;
            libraryMaster.sub_id = libraryInfo.SubjectID;
            libraryMaster.thumbnail_doc = libraryInfo.ThumbDocName;
            libraryMaster.thumbnail_img = libraryInfo.ThumbImageName;
            libraryMaster.type = libraryInfo.Type;
            this.context.LIBRARY_MASTER.Add(libraryMaster);
            if (isUpdate)
            {
                this.context.Entry(libraryMaster).State = System.Data.Entity.EntityState.Modified;
            }
            if (!isUpdate)
            {
                libData.library_id = libraryMaster.library_id;
            }

            libData.doc_content = !string.IsNullOrEmpty(libraryInfo.LibraryData.DocContentText) ? Convert.FromBase64String(libraryInfo.LibraryData.DocContentText) : libraryInfo.LibraryData.DocContent;
            libData.doc_img_ext = libraryInfo.LibraryData.DocContentExt;
            libData.doc_img_name = libraryInfo.LibraryData.DocContentFileName;
            libData.thumb_img_content = !string.IsNullOrEmpty(libraryInfo.LibraryData.ThumbImageContentText) ? Convert.FromBase64String(libraryInfo.LibraryData.ThumbImageContentText) : libraryInfo.LibraryData.ThumbImageContent;
            libData.thumb_img_ext = libraryInfo.LibraryData.ThumbImageExt;
            libData.thumb_img_name = libraryInfo.LibraryData.ThumbImageFileName;
            this.context.LIBRARY_DATA.Add(libData);
            if (isUpdate)
            {
                this.context.Entry(libData).State = System.Data.Entity.EntityState.Modified;
            }
            return this.context.SaveChanges() > 0 ? libraryMaster.library_id : 0;
        }

        public async Task<List<LibraryEntity>> GetAllLibrary(long branchID, long stdID)
        {
            var data = (from u in this.context.LIBRARY_MASTER.Include("LIBRARY_DATA")
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
                            SubjectID = u.sub_id,
                            ThumbDocName = u.thumbnail_doc,
                            ThumbImageName = u.thumbnail_img,
                            Type = u.type.Value,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value },
                            LibraryData = new LibraryDataEntity()
                            {
                                DocContent = u.LIBRARY_DATA.FirstOrDefault().doc_content,
                                DocContentExt = u.LIBRARY_DATA.FirstOrDefault().doc_img_ext,
                                DocContentFileName = u.LIBRARY_DATA.FirstOrDefault().doc_img_name,
                                LibraryID = u.library_id,
                                ThumbImageContent = u.LIBRARY_DATA.FirstOrDefault().thumb_img_content,
                                ThumbImageExt = u.LIBRARY_DATA.FirstOrDefault().thumb_img_ext,
                                ThumbImageFileName = u.LIBRARY_DATA.FirstOrDefault().thumb_img_name,
                                UniqueID = u.LIBRARY_DATA.FirstOrDefault().unique_id
                            }
                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].LibraryData.DocContentText = data[idx].LibraryData.DocContent.Length > 0 ? Convert.ToBase64String(data[idx].LibraryData.DocContent) : "";
                    data[idx].LibraryData.ThumbImageContentText = data[idx].LibraryData.ThumbImageContent.Length > 0 ? Convert.ToBase64String(data[idx].LibraryData.ThumbImageContent) : "";
                }
            }

            return data;
        }

        public async Task<List<LibraryEntity>> GetAllLibraryWithoutContent(long branchID, long stdID)
        {
            var data = (from u in this.context.LIBRARY_MASTER.Include("LIBRARY_DATA")
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
                            SubjectID = u.sub_id,
                            ThumbDocName = u.thumbnail_doc,
                            ThumbImageName = u.thumbnail_img,
                            Type = u.type.Value,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value },
                            LibraryData = new LibraryDataEntity()
                            {
                                DocContentExt = u.LIBRARY_DATA.FirstOrDefault().doc_img_ext,
                                DocContentFileName = u.LIBRARY_DATA.FirstOrDefault().doc_img_name,
                                LibraryID = u.library_id,
                                ThumbImageExt = u.LIBRARY_DATA.FirstOrDefault().thumb_img_ext,
                                ThumbImageFileName = u.LIBRARY_DATA.FirstOrDefault().thumb_img_name,
                                UniqueID = u.LIBRARY_DATA.FirstOrDefault().unique_id
                            }
                        }).ToList();


            return data;
        }

        public async Task<LibraryEntity> GetLibraryByLibraryID(long library)
        {
            var data = (from u in this.context.LIBRARY_MASTER.Include("LIBRARY_DATA")
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
                            SubjectID = u.sub_id,
                            ThumbDocName = u.thumbnail_doc,
                            ThumbImageName = u.thumbnail_img,
                            Type = u.type.Value,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value },
                            LibraryData = new LibraryDataEntity()
                            {
                                DocContent = u.LIBRARY_DATA.FirstOrDefault().doc_content,
                                DocContentExt = u.LIBRARY_DATA.FirstOrDefault().doc_img_ext,
                                DocContentFileName = u.LIBRARY_DATA.FirstOrDefault().doc_img_name,
                                LibraryID = u.library_id,
                                ThumbImageContent = u.LIBRARY_DATA.FirstOrDefault().thumb_img_content,
                                ThumbImageExt = u.LIBRARY_DATA.FirstOrDefault().thumb_img_ext,
                                ThumbImageFileName = u.LIBRARY_DATA.FirstOrDefault().thumb_img_name,
                                UniqueID = u.LIBRARY_DATA.FirstOrDefault().unique_id
                            }
                        }).FirstOrDefault();

            if (data != null)
            {
                data.LibraryData.DocContentText = data.LibraryData.DocContent.Length > 0 ? Convert.ToBase64String(data.LibraryData.DocContent) : "";
                data.LibraryData.ThumbImageContentText = data.LibraryData.ThumbImageContent.Length > 0 ? Convert.ToBase64String(data.LibraryData.ThumbImageContent) : "";
            }

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
