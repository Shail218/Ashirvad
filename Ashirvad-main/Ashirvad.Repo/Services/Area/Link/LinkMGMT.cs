using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Link
{
    public class LinkMGMT: ModelAccess, ILinkAPI
    {
        public async Task<long> LinkMaintenance(LinkEntity linkInfo)
        {
            Model.LINK_MASTER linkMaster = new Model.LINK_MASTER();
            bool isUpdate = true;
            var data = (from link in this.context.LINK_MASTER
                        where link.unique_id == linkInfo.UniqueID
                        select link).FirstOrDefault();
            if (data == null)
            {
                data = new Model.LINK_MASTER();
                isUpdate = false;
            }
            else
            {
                linkMaster = data;
                linkInfo.Transaction.TransactionId = data.trans_id;
            }

            linkMaster.row_sta_cd = linkInfo.RowStatus.RowStatusId;
            linkMaster.trans_id = this.AddTransactionData(linkInfo.Transaction);
            linkMaster.branch_id = linkInfo.Branch.BranchID;
            linkMaster.type = linkInfo.LinkType;
            linkMaster.title = linkInfo.Title;
            linkMaster.vid_desc = linkInfo.LinkDesc;
            linkMaster.vid_url = linkInfo.LinkURL;
            linkMaster.course_dtl_id = linkInfo.BranchCourse.course_dtl_id;
            linkMaster.class_dtl_id = linkInfo.BranchClass.Class_dtl_id;
            if (!isUpdate)
            {
                this.context.LINK_MASTER.Add(linkMaster);
            }

            var linkID = this.context.SaveChanges() > 0 ? linkMaster.unique_id : 0;
            return linkID;
        }

        public async Task<List<LinkEntity>> GetAllLink(int type, long branchID, long stdID)
        {
            var data = (from u in this.context.LINK_MASTER
                        .Include("BRANCH_MASTER")
                        where u.type == type
                        &&  u.branch_id == branchID
                        && (0 == stdID || u.class_dtl_id == stdID) && u.row_sta_cd == 1
                        select new LinkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UniqueID = u.unique_id,
                            Branch = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            LinkDesc = u.vid_desc,
                            LinkURL = u.vid_url,
                            LinkType =u.type,
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            Title = u.title
                        }).ToList();

            return data;
        }

        public async Task<List<LinkEntity>> GetAllLinkBySTD(int type, long branchID,long courseid, long stdID)
        {
            var data = (from u in this.context.LINK_MASTER
                        .Include("BRANCH_MASTER") orderby u.unique_id descending
                        where u.type == type
                        && u.branch_id == branchID
                        && (0 == stdID || u.class_dtl_id == stdID) && u.row_sta_cd == 1 && u.course_dtl_id == courseid
                        select new LinkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UniqueID = u.unique_id,
                            Branch = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            LinkDesc = u.vid_desc,
                            LinkURL = u.vid_url,
                            LinkType = u.type,
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            Title = u.title
                        }).ToList();

            return data;
        }

        public async Task<List<LinkEntity>> GetAllCustomLiveVideo(DataTableAjaxPostModel model, long branchID,int type)
        {
            var Result = new List<LinkEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.LINK_MASTER orderby u.unique_id descending
                          where u.type == type && u.branch_id == branchID && u.row_sta_cd == 1
                          select new LinkEntity()
                          {
                              UniqueID = u.unique_id
                          }).Distinct().Count();
            var data = (from u in this.context.LINK_MASTER
                        where u.type == type && u.branch_id == branchID && u.row_sta_cd == 1
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.title.ToLower().Contains(model.search.value)
                        || u.vid_desc.ToLower().Contains(model.search.value)
                        || u.vid_url.ToLower().Contains(model.search.value))
                        orderby u.unique_id descending
                        select new LinkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UniqueID = u.unique_id,
                            Branch = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            LinkDesc = u.vid_desc,
                            LinkURL = u.vid_url,
                            LinkType = u.type,
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            Count = count,
                            Title = u.title
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }

        public async Task<LinkEntity> GetLinkByUniqueID(long uniqueID)
        {
            var data = (from u in this.context.LINK_MASTER
                        .Include("BRANCH_MASTER")
                        where u.unique_id == uniqueID
                        select new LinkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UniqueID = u.unique_id,
                            Branch = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            LinkDesc = u.vid_desc,
                            LinkURL = u.vid_url,
                            LinkType = u.type,
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            Title = u.title
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveLink(long uniqueID, string lastupdatedby)
        {
            var data = (from u in this.context.LINK_MASTER
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
