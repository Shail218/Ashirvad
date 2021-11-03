using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Faculty;
using Ashirvad.Repo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Faculty
{
    public class Faculty : ModelAccess, IFacultyAPI
    {
        public async Task<long> FacultyMaintenance(FacultyEntity facultyInfo)
        {
            Model.FACULTY_MASTER facultymaster = new Model.FACULTY_MASTER();
            bool isUpdate = true;
            var data = (from facul in this.context.FACULTY_MASTER
                        where facul.faculty_id == facultyInfo.FacultyID
                        select facul).FirstOrDefault();
            if (data == null)
            {
                data = new Model.FACULTY_MASTER();
                isUpdate = false;
            }
            else
            {
                facultymaster = data;
                isUpdate = true;
                facultyInfo.Transaction.TransactionId = data.trans_id;

            }
            facultymaster.staff_id = facultyInfo.staff.StaffID;
            facultymaster.board_type = (int)facultyInfo.board;
            facultymaster.subject_id = facultyInfo.subject.SubjectID;
            facultymaster.std_id = facultyInfo.standard.StandardID;
            facultymaster.description = facultyInfo.Descripation;
            facultymaster.file_name = facultyInfo.FacultyContentFileName;
            facultymaster.file_path = facultyInfo.FilePath;
            facultymaster.row_sta_cd = facultyInfo.RowStatus.RowStatusId;
            facultymaster.trans_id = this.AddTransactionData(facultyInfo.Transaction);
            facultymaster.branch_id = facultyInfo.BranchInfo.BranchID;
            if (!isUpdate)
            {
                this.context.FACULTY_MASTER.Add(facultymaster);
            }

            this.context.SaveChanges();
            return 0;

        }



        public async Task<List<FacultyEntity>> GetAllFaculty(long branchID)
        {
            var data = (from u in this.context.FACULTY_MASTER
                          .Include("STD_MASTER")
                          .Include("SUBJECT_MASTER")
                          .Include("BRANCH_MASTER")
                          .Include("BRANCH_STAFF")
                        where branchID == 0 || u.branch_id == branchID && u.row_sta_cd == 1
                        select new FacultyEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            standard = new StandardEntity()
                            {
                                StandardID = u.STD_MASTER.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            staff = new StaffEntity()
                            {
                                StaffID = u.BRANCH_STAFF.staff_id,
                                Name = u.BRANCH_STAFF.name
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity()
                            {
                                TransactionId = u.trans_id
                            },
                            board = u.board_type == 1 ? Enums.BoardType.GujaratBoard : u.board_type == 2 ? Enums.BoardType.CBSC : Enums.BoardType.Both,
                            FacultyID = u.faculty_id,
                        }).ToList();

            return data;
        }

        public async Task<List<FacultyEntity>> GetAllFaculty(long branchID, int typeID)
        {
            var data = (from u in this.context.FACULTY_MASTER
                         .Include("STD_MASTER")
                         .Include("SUBJECT_MASTER")
                         .Include("BRANCH_MASTER")
                         .Include("BRANCH_STAFF")
                        where branchID == 0 || u.branch_id == branchID && u.row_sta_cd==1
                        select new FacultyEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            standard = new StandardEntity()
                            {
                                StandardID = u.STD_MASTER.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            staff = new StaffEntity()
                            {
                                StaffID = u.BRANCH_STAFF.staff_id,
                                Name = u.BRANCH_STAFF.name
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity()
                            {
                                TransactionId = u.trans_id
                            },
                            board = u.board_type == 1 ? Enums.BoardType.GujaratBoard : u.board_type == 2 ? Enums.BoardType.CBSC : Enums.BoardType.Both,
                            FacultyID=u.faculty_id,
                            HeaderImageText=u.file_name,
                            FilePath=u.file_path,
                        }).ToList();

            return data;
        }

        public async Task<FacultyEntity> GetFacultyByFacultyID(long facultyID)
        {
            var data = (from u in this.context.FACULTY_MASTER
                        .Include("STD_MASTER")
                         .Include("SUBJECT_MASTER")
                         .Include("BRANCH_MASTER")
                         .Include("BRANCH_STAFF")
                        where u.faculty_id == facultyID
                        select new FacultyEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            FacultyID = u.faculty_id,
                            Descripation = u.description,
                            FacultyContentFileName=u.file_name,
                            FilePath=u.file_path,
                            standard = new StandardEntity()
                            {
                                StandardID = u.STD_MASTER.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            staff = new StaffEntity()
                            {
                                StaffID = u.BRANCH_STAFF.staff_id,
                                Name = u.BRANCH_STAFF.name
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity()
                            {
                                TransactionId = u.trans_id
                            },
                            board = u.board_type == 1 ? Enums.BoardType.GujaratBoard : u.board_type == 2 ? Enums.BoardType.CBSC : Enums.BoardType.Both,

                        }).FirstOrDefault();


            return data;
        }

        public bool RemoveFaculty(long faculID, string lastupdatedby)
        {
            var data = (from u in this.context.FACULTY_MASTER
                        where u.faculty_id == faculID
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
