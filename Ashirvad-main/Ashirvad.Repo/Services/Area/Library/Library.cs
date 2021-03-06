using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Library;
using Ashirvad.Repo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

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

            libraryMaster.thumbnail_img = libraryInfo.ThumbnailFileName;
            libraryMaster.thumbnail_path = libraryInfo.ThumbnailFilePath;
            libraryMaster.library_image = libraryInfo.DocFileName;
            libraryMaster.library_path = libraryInfo.DocFilePath;
            libraryMaster.category_id = libraryInfo.CategoryInfo.CategoryID;
            libraryMaster.library_type = libraryInfo.Library_Type;
            libraryMaster.type = libraryInfo.Type;
            libraryMaster.library_title = libraryInfo.LibraryTitle;
            libraryMaster.video_link = libraryInfo.VideoLink;
            libraryMaster.createby_branch = libraryInfo.CreatebyBranch;
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
            libraryInfo.LibraryID = libraryMaster.library_id;
            LibraryStandardMaintenance(libraryInfo);


            return libraryMaster.library_id;
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
            var data = (from u in this.context.LIBRARY_MASTER orderby u.library_id descending
                        where (0 == u.branch_id || u.branch_id == branchID)
                        // (0 == stdID || u.std_id == stdID)
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
                            //  StandardID = u.std_id,
                            //SubjectID = u.subject_id,
                            ThumbnailFileName = u.thumbnail_img,
                            ThumbnailFilePath = "https://mastermind.org.in" + u.thumbnail_path,
                            DocFileName = u.library_image,
                            DocFilePath = "https://mastermind.org.in" + u.library_path,
                            CategoryInfo = new CategoryEntity()
                            {
                                CategoryID = u.category_id.HasValue ? u.category_id.Value : 0
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
                        orderby u.library_id descending
                        from branch in tempBranch.DefaultIfEmpty()
                        where (0 == branchID || u.branch_id == null || u.branch_id == 0 || u.branch_id == branchID) && u.row_sta_cd == 1
                        //  (0 == stdID || u.std_id == stdID || u.std_id == null || u.std_id == 0) && u.row_sta_cd == 1//
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

                            //SubjectID = u.subject_id,
                            ThumbnailFileName = u.thumbnail_img,
                            ThumbnailFilePath = "https://mastermind.org.in" + u.thumbnail_path,
                            DocFileName = u.library_image,
                            DocFilePath = "https://mastermind.org.in" + u.library_path,
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
                            //SubjectID = u.subject_id,
                            ThumbnailFileName = u.thumbnail_img,
                            ThumbnailFilePath = u.thumbnail_path,
                            DocFileName = u.library_image,
                            DocFilePath = u.library_path,
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
            if (data != null)
            {
                data.Subjectlist = new List<SubjectEntity>();
                data.Standardlist = new List<StandardEntity>();
                var Standard = (from u in this.context.LIBRARY_STD_MASTER
                                where u.library_id == library
                                select new StandardEntity()
                                {
                                    Standard = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                    StandardID = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0
                                }).Distinct().ToList();

                var Subject = (from u in this.context.LIBRARY_STD_MASTER
                               where u.library_id == library
                               select new SubjectEntity()
                               {
                                   Subject = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                   SubjectID = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0
                               }).Distinct().ToList();
                data.BranchCourse = (from u in this.context.LIBRARY_STD_MASTER
                                     where u.library_id == library
                                     select new BranchCourseEntity()
                                     {
                                         course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0
                                     }).FirstOrDefault();
                data.BranchSubject = (from u in this.context.LIBRARY_STD_MASTER
                                     where u.library_id == library
                                     select new BranchSubjectEntity()
                                     {
                                         Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                         Subject = new SuperAdminSubjectEntity()
                                         {
                                             SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                                         }
                                     }).FirstOrDefault();

                data.Subjectlist.AddRange(Subject);
                data.Standardlist.AddRange(Standard);
            }
            return data;
        }

        public async Task<List<LibraryEntity>> GetAllLibrary(int Type, long BranchID)
        {
            var data = (from u in this.context.LIBRARY_MASTER
                        where u.row_sta_cd == 1 && u.library_type == Type && (u.branch_id == BranchID || BranchID == 0)
                        select new LibraryEntity()
                        {
                            LibraryID = u.library_id,
                            BranchID = u.branch_id,
                            VideoLink = u.video_link,
                            LibraryTitle = u.library_title,
                            ThumbnailFileName = u.thumbnail_img,
                            ThumbnailFilePath = "https://mastermind.org.in" + u.thumbnail_path,
                            Type = u.type.Value,
                            Description = u.doc_desc,
                            DocFileName = u.library_image,
                            DocFilePath = "https://mastermind.org.in" + u.library_path,
                            CategoryInfo = new CategoryEntity()
                            {
                                CategoryID = u.category_id.HasValue ? u.category_id.Value : 0,
                                Category = u.CATEGORY_MASTER.category_name
                            },

                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value }
                        }).Distinct().OrderByDescending(a => a.LibraryID).ToList();
            if (data.Count > 0)
            {
                data[0].libraryEntities = new List<LibraryEntity>();
                foreach (var item in data)
                {
                    item.libraryEntities = (from u in this.context.LIBRARY_STD_MASTER
                                            where item.RowStatus.RowStatusId == 1 && item.LibraryID == u.library_id
                                            select new LibraryEntity()
                                            {
                                                //LibraryID = item.LibraryID,
                                                standard = new StandardEntity()
                                                {
                                                    //StandardID = u.std_id.HasValue ? u.std_id.Value : 0,
                                                    Standard = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                                }
                                            }).Distinct().ToList();
                }
            }

            return data;
        }

        public async Task<List<LibraryEntity>> GetAllLibrarybybranch(int Type, long BranchID)
        {
            var data = (from u in this.context.LIBRARY_MASTER orderby u.library_id descending
                        where u.row_sta_cd == 1 && u.library_type == Type && (u.createby_branch == BranchID || BranchID == 0)
                        select new LibraryEntity()
                        {
                            LibraryID = u.library_id,
                            BranchID = u.branch_id,
                            VideoLink = u.video_link,
                            LibraryTitle = u.library_title,
                            ThumbnailFileName = u.thumbnail_img,
                            ThumbnailFilePath = "https://mastermind.org.in" + u.thumbnail_path,
                            Type = u.type.Value,
                            Description = u.doc_desc,
                            DocFileName = u.library_image,
                            DocFilePath = "https://mastermind.org.in" + u.library_path,
                            CategoryInfo = new CategoryEntity()
                            {
                                CategoryID = u.category_id.HasValue ? u.category_id.Value : 0,
                                Category = u.CATEGORY_MASTER.category_name
                            },

                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value }
                        }).Distinct().OrderByDescending(a => a.LibraryID).ToList();
            if (data.Count > 0)
            {
                data[0].libraryEntities = new List<LibraryEntity>();
                foreach (var item in data)
                {
                    item.libraryEntities = (from u in this.context.LIBRARY_STD_MASTER
                                            where item.RowStatus.RowStatusId == 1 && item.LibraryID == u.library_id
                                            select new LibraryEntity()
                                            {
                                                //LibraryID = item.LibraryID,
                                                standard = new StandardEntity()
                                                {
                                                    //StandardID = u.std_id.HasValue ? u.std_id.Value : 0,
                                                    Standard = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                                }
                                            }).Distinct().ToList();
                }
            }

            return data;
        }

        public async Task<List<LibraryEntity>> GetAllMobileLibrary(int Type, long BranchID)
        {
            var data = (from u in this.context.LIBRARY_MASTER
                        orderby u.library_id descending
                        where u.row_sta_cd == 1 && u.library_type == Type && (u.createby_branch == BranchID || BranchID == 0)
                        select new LibraryEntity()
                        {
                            LibraryID = u.library_id,
                            BranchID = u.branch_id,
                            VideoLink = u.video_link,
                            LibraryTitle = u.library_title,
                            ThumbnailFileName = u.thumbnail_img,
                            ThumbnailFilePath = "https://mastermind.org.in" + u.thumbnail_path,
                            Type = u.type.Value,
                            Library_Type = (int)u.library_type,
                            Description = u.doc_desc,
                            DocFileName = u.library_image,
                            DocFilePath = "https://mastermind.org.in" + u.library_path,
                            CategoryInfo = new CategoryEntity()
                            {
                                CategoryID = u.category_id.HasValue ? u.category_id.Value : 0,
                                Category = u.CATEGORY_MASTER.category_name
                            },
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            list = (from b in this.context.LIBRARY_STD_MASTER
                                    where u.row_sta_cd == 1 && b.library_id == u.library_id
                                    select new LibraryStandardEntity()
                                    {
                                        library_id = b.library_id,
                                        BranchCourse = new BranchCourseEntity()
                                        {
                                            course_dtl_id = b.course_dtl_id.HasValue ? b.course_dtl_id.Value : 0,
                                            course = new CourseEntity()
                                            {
                                                CourseID = b.COURSE_DTL_MASTER.COURSE_MASTER.course_id,
                                                CourseName = b.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                            }
                                        },
                                        std_id = b.class_dtl_id.HasValue? b.class_dtl_id.Value:0,
                                        standard = b.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                        subject = b.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                                    }).Distinct().ToList(),
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value }
                        }).ToList();
            //if (data?.Count > 0)
            //{
            //    foreach (var item in data)
            //    {
            //        int idx = data.IndexOf(item);
            //        data[idx].subject = this.context.LIBRARY_STD_MASTER.Where(z => z.library_id == item.LibraryID && z.course_dtl_id == item.list[idx].BranchCourse.course_dtl_id && z.LIBRARY_MASTER.row_sta_cd == 1).Select(y => new SubjectEntity() { SubjectID = y.subject_dtl_id.HasValue ? y.subject_dtl_id.Value : 0, Subject = y.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name }).FirstOrDefault();                   
            //    }
            //}         
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
            //bool Isvalid = CheckHistory(libraryID);
            //if (Isvalid)
            //{
               
            //}
            return false;
        }

        public async Task<bool> LibraryStandardMaintenance(LibraryEntity libraryInfo)
        {
            bool isSuccess = false;

            if (libraryInfo.Type == 1)
            {
                var data = (from lib in this.context.LIBRARY_STD_MASTER
                            where lib.library_id == libraryInfo.LibraryID
                            select lib).ToList();
                if (data?.Count > 0)
                {
                    this.context.LIBRARY_STD_MASTER.RemoveRange(data);
                    this.context.SaveChanges();
                }


            }
            else
            {
                var data = (from lib in this.context.LIBRARY_STD_MASTER
                            where lib.library_id == libraryInfo.LibraryID
                            select lib).ToList();
                if (data?.Count > 0)
                {
                    this.context.LIBRARY_STD_MASTER.RemoveRange(data);
                }
                foreach (var item in libraryInfo.Standardlist)
                {
                    if (item != null)
                    {
                        LIBRARY_STD_MASTER library = null;
                        List<LIBRARY_STD_MASTER> libraryList = new List<LIBRARY_STD_MASTER>();
                        //long Std = Convert.ToInt64(item);
                        foreach (var item1 in libraryInfo.Subjectlist)
                        {
                            library = new LIBRARY_STD_MASTER()
                            {
                                library_id = libraryInfo.LibraryID,
                                course_dtl_id = libraryInfo.BranchCourse.course_dtl_id,
                                class_dtl_id = item.StandardID,
                                subject_dtl_id = item1.SubjectID
                            };
                            libraryList.Add(library);
                        }
                        this.context.LIBRARY_STD_MASTER.AddRange(libraryList);
                        this.context.SaveChanges();
                        isSuccess = true;
                    }
                }

            }
            return isSuccess;
        }

        public async Task<long> LibraryApprovalMaintenance(ApprovalEntity approvalEntity)
        {
            Model.APPROVAL_MASTER approvalMaster = new Model.APPROVAL_MASTER();
            bool isUpdate = true;
            var data = (from aprv in this.context.APPROVAL_MASTER
                        where aprv.library_id == approvalEntity.library.LibraryID && aprv.branch_id == approvalEntity.Branch_id
                        select new
                        {
                            approvalMaster = aprv
                        }).FirstOrDefault();
            if (data == null)
            {
                approvalMaster = new Model.APPROVAL_MASTER();
                isUpdate = false;
            }
            else
            {
                approvalMaster = data.approvalMaster;
                approvalEntity.TransactionInfo.TransactionId = data.approvalMaster.trans_id;
            }

            approvalMaster.row_sta_cd = approvalEntity.RowStatus.RowStatusId;
            approvalMaster.trans_id = this.AddTransactionData(approvalEntity.TransactionInfo);
            approvalMaster.branch_id = approvalEntity.Branch_id;
            approvalMaster.library_id = approvalEntity.library.LibraryID;
            approvalMaster.library_status = (int)approvalEntity.Library_Status;
            this.context.APPROVAL_MASTER.Add(approvalMaster);
            if (isUpdate)
            {
                this.context.Entry(approvalMaster).State = System.Data.Entity.EntityState.Modified;
            }
            return this.context.SaveChanges() > 0 ? approvalMaster.approval_id : 0;
        }

        public async Task<List<LibraryEntity>> GetAllLibraryApproval(long BranchId)
        {
            List<LibraryEntity> libraryEntities = new List<LibraryEntity>();
            libraryEntities = (from u in this.context.LIBRARY_MASTER
                               join li in this.context.LIBRARY_STD_MASTER on u.library_id equals li.library_id into ps
                               from li in ps.DefaultIfEmpty()
                               orderby u.library_id descending
                               where u.row_sta_cd == 1
                               && u.branch_id == 0
                               && u.createby_branch != BranchId
                               select new LibraryEntity()
                               {
                                   LibraryID = u.library_id,
                                   BranchID = u.branch_id,
                                   VideoLink = u.video_link,
                                   LibraryTitle = u.library_title,
                                   ThumbnailFileName = u.thumbnail_img,
                                   ThumbnailFilePath = u.thumbnail_path == null || u.thumbnail_path == "" ? "" : "https://mastermind.org.in" + u.thumbnail_path,
                                   Type = u.type.Value,
                                   Library_Type = (int)u.library_type,
                                   Description = u.doc_desc,
                                   DocFileName = u.library_image,
                                   CreatebyBranch = u.createby_branch.Value,
                                   DocFilePath = u.library_path == null || u.library_path == "" ? "" : "https://mastermind.org.in" + u.library_path,
                                   CategoryInfo = new CategoryEntity()
                                   {
                                       CategoryID = u.category_id.HasValue ? u.category_id.Value : 0,
                                       Category = u.CATEGORY_MASTER.category_name
                                   },
                                   RowStatus = new RowStatusEntity()
                                   {
                                       RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                       RowStatusId = (int)u.row_sta_cd
                                   },
                                   Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value },
                                   subject = new SubjectEntity()
                                   {
                                       Subject = li != null ? li.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name : ""
                                   },
                                   approval = new ApprovalEntity()
                                   {
                                       Approval_id = 0,
                                       Library_Status_text = "Pending",
                                   }
                               }).Distinct().ToList();
            if (libraryEntities?.Count > 0)
            {
                //foreach (var item in data)
                //{
                //    int idx = data.IndexOf(item);
                //    data[idx].Subjectlist = this.context.LIBRARY_STD_MASTER.Where(z => z.library_id == item.LibraryID).Select(y => new SubjectEntity()
                //    { Subject = y.SUBJECT_MASTER.subject }).ToList();

                //}
                foreach (var item in libraryEntities)
                {
                    var res = (from u in this.context.APPROVAL_MASTER
                                     .Include("LIBRARY_MASTER")
                               where u.row_sta_cd == 1
                               && u.branch_id == BranchId
                               && u.library_id == item.LibraryID
                               select new ApprovalEntity()
                               {
                                   Approval_id = u.approval_id,
                                   Library_Status_text = u.library_status == 1 ? "Pending" : u.library_status == 2 ? "Approve" : u.library_status == 3 ? "Reject" : "Pending",
                               }).FirstOrDefault();
                    if (res != null)
                    {
                        item.approval = res;
                    }
                }
            }
            return libraryEntities;

        }

        public async Task<List<LibraryEntity>> GetLibraryApprovalByBranch(long BranchId)
        {
            List<LibraryEntity> libraryEntities = new List<LibraryEntity>();
            libraryEntities = (from li in this.context.LIBRARY_MASTER
                               join u in this.context.APPROVAL_MASTER on li.library_id equals u.library_id into ps
                               from u in ps.DefaultIfEmpty()
                               orderby u.library_id descending
                               where u.row_sta_cd == 1
                               && u.branch_id == BranchId
                               && u.library_status == 2
                               select new LibraryEntity()
                               {
                                   LibraryID = li.library_id,
                                   BranchID = li.branch_id,
                                   VideoLink = li.video_link,
                                   LibraryTitle = li.library_title,
                                   ThumbnailFileName = li.thumbnail_img,
                                   ThumbnailFilePath = li.thumbnail_path == null || li.thumbnail_path == "" ? "" : "https://mastermind.org.in" + li.thumbnail_path,
                                   Type = li.type.Value,
                                   Library_Type = (int)li.library_type,
                                   Description = li.doc_desc,
                                   DocFileName = li.library_image,
                                   CreatebyBranch = li.createby_branch.Value,
                                   DocFilePath = li.library_path == null || li.library_path == "" ? "" : "https://mastermind.org.in" + li.library_path,
                                   CategoryInfo = new CategoryEntity()
                                   {
                                       CategoryID = li.category_id.HasValue ? li.category_id.Value : 0,
                                       Category = li.CATEGORY_MASTER.category_name
                                   },
                                   RowStatus = new RowStatusEntity()
                                   {
                                       RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                       RowStatusId = (int)u.row_sta_cd
                                   },
                                   Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                               }).ToList();

            var library = (from li in this.context.LIBRARY_MASTER
                           where li.createby_branch == BranchId && li.row_sta_cd == 1
                           select new LibraryEntity()
                           {
                               LibraryID = li.library_id,
                               BranchID = li.branch_id,
                               VideoLink = li.video_link,
                               LibraryTitle = li.library_title,
                               ThumbnailFileName = li.thumbnail_img,
                               ThumbnailFilePath = li.thumbnail_path == null || li.thumbnail_path == "" ? "" : "https://mastermind.org.in" + li.thumbnail_path,
                               Type = li.type.Value,
                               Library_Type = (int)li.library_type,
                               Description = li.doc_desc,
                               DocFileName = li.library_image,
                               CreatebyBranch = li.createby_branch.Value,
                               DocFilePath = li.library_path == null || li.library_path == "" ? "" : "https://mastermind.org.in" + li.library_path,
                               CategoryInfo = new CategoryEntity()
                               {
                                   CategoryID = li.category_id.HasValue ? li.category_id.Value : 0,
                                   Category = li.CATEGORY_MASTER.category_name
                               },
                               RowStatus = new RowStatusEntity()
                               {
                                   RowStatus = li.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                   RowStatusId = (int)li.row_sta_cd
                               },
                               Transaction = new TransactionEntity() { TransactionId = (long)li.trans_id },
                           }).ToList();

            libraryEntities.AddRange(library);
            return libraryEntities;
        }

        public async Task<List<LibraryEntity>> GetLibraryApprovalByBranchStd(long standardID, long BranchId, string standard)
        {
            List<LibraryEntity> libraryEntities = new List<LibraryEntity>();
            libraryEntities = (from li in this.context.LIBRARY_MASTER
                               join u in this.context.APPROVAL_MASTER on li.library_id equals u.library_id
                               join ls in this.context.LIBRARY_STD_MASTER on li.library_id equals ls.library_id
                               orderby u.approval_id descending
                               where u.row_sta_cd == 1
                               && u.branch_id == BranchId
                               && u.library_status == 2
                               && ls.CLASS_DTL_MASTER.CLASS_MASTER.class_name == standard
                               //&& ls.class_dtl_id == standardID
                               select new LibraryEntity()
                               {
                                   LibraryID = u.library_id,
                                   BranchID = u.branch_id,
                                   VideoLink = li.video_link,
                                   LibraryTitle = li.library_title,
                                   ThumbnailFileName = li.thumbnail_img,
                                   ThumbnailFilePath = li.thumbnail_path == null || li.thumbnail_path == "" ? "" : "https://mastermind.org.in" + li.thumbnail_path,
                                   Type = li.type.Value,
                                   Library_Type = (int)li.library_type,
                                   Description = li.doc_desc,
                                   DocFileName = li.library_image,
                                   CreatebyBranch = li.createby_branch.Value,
                                   DocFilePath = li.library_path == null || li.library_path == "" ? "" : "https://mastermind.org.in" + li.library_path,
                                   CategoryInfo = new CategoryEntity()
                                   {
                                       CategoryID = li.category_id.HasValue ? li.category_id.Value : 0,
                                       Category = li.CATEGORY_MASTER.category_name
                                   },
                                   RowStatus = new RowStatusEntity()
                                   {
                                       RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                       RowStatusId = (int)u.row_sta_cd
                                   },
                                   Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                               }).Distinct().ToList();

            var library = (from li in this.context.LIBRARY_MASTER
                           join ls in this.context.LIBRARY_STD_MASTER on li.library_id equals ls.library_id

                           where li.row_sta_cd == 1
                           && li.createby_branch == BranchId
                           && li.type == 2
                            && ls.CLASS_DTL_MASTER.CLASS_MASTER.class_name == standard
                               && ls.class_dtl_id == standardID
                           group li by new
                           {
                               li.library_id,
                               li.library_title,
                               li.thumbnail_img,
                               li.thumbnail_path,
                               li.library_type,
                               li.doc_desc,
                                li.library_image,
                               li.library_path,
                               li.video_link,
                               li.type
                           } into gcs
                           select new LibraryEntity()
                           {
                               LibraryID = gcs.Key.library_id,
                              
                               VideoLink = gcs.Key.video_link,
                               LibraryTitle = gcs.Key.library_title,
                               ThumbnailFileName = gcs.Key.thumbnail_img,
                               ThumbnailFilePath = gcs.Key.thumbnail_path == null || gcs.Key.thumbnail_path == "" ? "" : "https://mastermind.org.in" + gcs.Key.thumbnail_path,
                               Type = gcs.Key.type.Value,
                               Library_Type = (int)gcs.Key.library_type,
                               Description = gcs.Key.doc_desc,
                               DocFileName = gcs.Key.library_image,
                               
                               DocFilePath = gcs.Key.library_path == null || gcs.Key.library_path == "" ? "" : "https://mastermind.org.in" + gcs.Key.library_path,
                               
                           }).ToList();

            libraryEntities.AddRange(library);
            return libraryEntities;
        }

        public bool CheckHistory(long libraryID)
        {
            bool Issuccess = true;
            Issuccess = this.context.APPROVAL_MASTER.Where(s => s.library_id == libraryID).FirstOrDefault() != null;
            if (Issuccess)
            {
                return false;
            }
            return true;
        }


        public async Task<List<LibraryEntity>> GetAllCustomLibrary(DataTableAjaxPostModel model,int Type, long BranchID)
        {
            List<LibraryEntity> libraryEntities = new List<LibraryEntity>();
            try
            {
                bool Isasc = model.order[0].dir == "desc" ? false : true;
                long count = (from u in this.context.LIBRARY_MASTER
                              orderby u.library_id descending
                              where u.row_sta_cd == 1 && u.library_type == Type
                              && (u.createby_branch == BranchID || BranchID == 0)
                              select new LibraryEntity()
                              {
                                  
                                  LibraryID = u.library_id,
                                  BranchID = u.branch_id,
                                  VideoLink = u.video_link,
                                  LibraryTitle = u.library_title,
                                  ThumbnailFileName = u.thumbnail_img,
                                  ThumbnailFilePath = "https://mastermind.org.in" + u.thumbnail_path,
                                  Type = u.type.Value,
                                  Description = u.doc_desc,
                                  DocFileName = u.library_image,
                                  DocFilePath = "https://mastermind.org.in" + u.library_path,
                                  CategoryInfo = new CategoryEntity()
                                  {
                                      CategoryID = u.category_id.HasValue ? u.category_id.Value : 0,
                                      Category = u.CATEGORY_MASTER.category_name
                                  },                                  
                                  RowStatus = new RowStatusEntity()
                                  {
                                      RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                      RowStatusId = (int)u.row_sta_cd
                                  },
                                  Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value }
                              }).Distinct().Count();

                libraryEntities = (from u in this.context.LIBRARY_MASTER
                            orderby u.library_id descending
                            where u.row_sta_cd == 1 && u.library_type == Type && (u.createby_branch == BranchID || BranchID == 0)
                            select new LibraryEntity()
                            {
                                LibraryID = u.library_id,
                                BranchID = u.branch_id,
                                VideoLink = u.video_link,
                                LibraryTitle = u.library_title,
                                ThumbnailFileName = u.thumbnail_img,
                                ThumbnailFilePath = "https://mastermind.org.in" + u.thumbnail_path,
                                Type = u.type.Value,
                                Description = u.doc_desc,
                                DocFileName = u.library_image,
                                DocFilePath = "https://mastermind.org.in" + u.library_path,
                                CategoryInfo = new CategoryEntity()
                                {
                                    CategoryID = u.category_id.HasValue ? u.category_id.Value : 0,
                                    Category = u.CATEGORY_MASTER.category_name
                                },
                                Count=count,
                                RowStatus = new RowStatusEntity()
                                {
                                    RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                    RowStatusId = (int)u.row_sta_cd
                                },
                                Transaction = new TransactionEntity() { TransactionId = u.trans_id.Value }
                            })
                       .OrderByDescending(u => u.LibraryID)
                       .Skip(model.start)
                       .Take(model.length)
                       .ToList();
                if (libraryEntities.Count > 0)
                {

                    foreach (var item in libraryEntities)
                    {
                        item.libraryEntities = (from u in this.context.LIBRARY_STD_MASTER
                                                where item.RowStatus.RowStatusId == 1 && item.LibraryID == u.library_id
                                                select new LibraryEntity()
                                                {
                                                    //LibraryID = item.LibraryID,
                                                    standard = new StandardEntity()
                                                    {
                                                        //StandardID = u.std_id.HasValue ? u.std_id.Value : 0,
                                                        Standard = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                                    }
                                                }).Distinct().ToList();
                        item.subject = (from u in this.context.LIBRARY_STD_MASTER
                                        where item.RowStatus.RowStatusId == 1 && item.LibraryID == u.library_id
                                        select new SubjectEntity()
                                        {
                                            Subject = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name

                                        }).FirstOrDefault();
                        item.BranchCourse = (from u in this.context.LIBRARY_STD_MASTER
                                        where item.RowStatus.RowStatusId == 1 && item.LibraryID == u.library_id
                                        select new BranchCourseEntity()
                                        {
                                            course = new CourseEntity()
                                            {
                                                CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                            }

                                        }).FirstOrDefault();
                    }
                }
            }
            catch(Exception ex)
            {

            }
           
           

            return libraryEntities;
        }
    }
}
