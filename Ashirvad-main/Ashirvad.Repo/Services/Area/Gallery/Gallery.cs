using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Gallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Gallery
{
    public class Gallery : ModelAccess, IGalleryAPI
    {
        public async Task<long> GalleryMaintenance(GalleryEntity galleryInfo)
        {
            Model.GALLERY_MASTER galleryMaster = new Model.GALLERY_MASTER();
            bool isUpdate = true;
            var data = (from gallery in this.context.GALLERY_MASTER
                        where gallery.unique_id == galleryInfo.UniqueID
                        select gallery).FirstOrDefault();
            if (data == null)
            {
                data = new Model.GALLERY_MASTER();
                isUpdate = false;
            }
            else
            {
                galleryMaster = data;
                galleryInfo.Transaction.TransactionId = data.trans_id;
            }

            if (galleryInfo.FileInfo?.Length > 0)
            {
                galleryMaster.file_info = galleryInfo.FileInfo;
            }
            else if (!string.IsNullOrEmpty(galleryInfo.FileEncoded))
            {
                galleryMaster.file_info = Convert.FromBase64String(galleryInfo.FileEncoded);
            }
            else
            {
                galleryMaster.file_info = null;
            }


            galleryMaster.row_sta_cd = galleryInfo.RowStatus.RowStatusId;
            galleryMaster.trans_id = this.AddTransactionData(galleryInfo.Transaction);
            galleryMaster.branch_id = galleryInfo.Branch.BranchID;
            galleryMaster.uplaod_type = galleryInfo.GalleryType;
            galleryMaster.remarks = galleryInfo.Remarks;
            if (!isUpdate)
            {
                this.context.GALLERY_MASTER.Add(galleryMaster);
            }

            var uniqueID = this.context.SaveChanges() > 0 ? galleryMaster.unique_id : 0;
            return uniqueID;
        }
        
        public async Task<List<GalleryEntity>> GetAllGallery(int type, long branchID)
        {
            var data = (from u in this.context.GALLERY_MASTER
                        .Include("BRANCH_MASTER")
                        where u.uplaod_type == type
                        && (0 == branchID || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new GalleryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UniqueID = u.unique_id,
                            FileInfo = u.file_info,
                            Branch = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            GalleryType = u.uplaod_type,
                            Remarks = u.remarks
                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].FileEncoded = data[idx].FileInfo.Length > 0 ? Convert.ToBase64String(data[idx].FileInfo) : "";
                }
            }

            return data;
        }

        public async Task<List<GalleryEntity>> GetAllGalleryWithoutContent(int type, long branchID)
        {
            var data = (from u in this.context.GALLERY_MASTER
                        .Include("BRANCH_MASTER")
                        where u.uplaod_type == type
                        && (0 == branchID || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new GalleryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UniqueID = u.unique_id,
                            Branch = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            GalleryType = u.uplaod_type,
                            Remarks = u.remarks
                        }).ToList();

            return data;
        }

        public async Task<GalleryEntity> GetGalleryByUniqueID(long uniqueID)
        {
            var data = (from u in this.context.GALLERY_MASTER
                        .Include("BRANCH_MASTER")
                        where u.unique_id == uniqueID
                        select new GalleryEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UniqueID = u.unique_id,
                            FileInfo = u.file_info,
                            Branch = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            GalleryType = u.uplaod_type,
                            Remarks = u.remarks
                        }).FirstOrDefault();

            if (data != null)
            {
                data.FileEncoded = data.FileInfo.Length > 0 ? Convert.ToBase64String(data.FileInfo) : "";
            }

            return data;
        }

        public bool RemoveGallery(long uniqueID, string lastupdatedby)
        {
            var data = (from u in this.context.GALLERY_MASTER
                        where u.unique_id == uniqueID
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
