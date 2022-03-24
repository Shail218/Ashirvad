using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.User;
using Ashirvad.Repo.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.User
{
    public class User : ModelAccess, IUserAPI
    {
        ResponseModel responseModel = new ResponseModel();
        public async Task<long> UserMaintenance(UserEntity userInfo)
        {
            Model.USER_DEF user = new Model.USER_DEF();
            bool isUpdate = true;
            var data = (from u in this.context.USER_DEF
                        where u.user_id == userInfo.UserID
                        select u).FirstOrDefault();
            if (data == null)
            {
                user = new Model.USER_DEF();
                isUpdate = false;
            }
            else
            {
                user = data;
            }
            user.branch_id = userInfo.BranchInfo.BranchID;
            user.user_type = (int)userInfo.UserType;
            user.parent_id = userInfo.ParentID;
            user.password = userInfo.Password;
            user.row_sta_cd = userInfo.RowStatus.RowStatusId;
            user.staff_id = userInfo.StaffID;
            user.student_id = userInfo.StudentID;
            user.trans_id = this.AddTransactionData(userInfo.Transaction);
            user.username = userInfo.Username;
            string clientSecret = Security.GenerateToken(Security.CreateClientSecret(userInfo.Username, userInfo.RowStatus.RowStatusId.ToString()));
            //user.client_secret = clientSecret;
            this.context.USER_DEF.Add(user);
            if (isUpdate)
            {
                this.context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            }

            this.context.SaveChanges();
            this.AddUserRoles(userInfo);
            return user.user_id;
        }

        public async Task<long> ProfileMaintenance(UserEntity userInfo)
        {
            USER_DEF user = new USER_DEF();
            var data = (from u in this.context.USER_DEF
                        where u.user_id == userInfo.UserID
                        select u).FirstOrDefault();
            user = data;
            user.trans_id = this.AddTransactionData(userInfo.Transaction);
            user.username = userInfo.Username;
            this.context.USER_DEF.Add(user);
            this.context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            this.context.SaveChanges();
            return user.user_id;
        }

        public async Task<UserEntity> ValidateUser(string userName, string password)
        {
            var user = (from u in this.context.USER_DEF
                        .Include("BRANCH_STAFF")
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        where u.username == userName && u.password == password && (u.user_type == (int)Enums.UserType.Staff || u.user_type == (int)Enums.UserType.Admin || u.user_type == (int)Enums.UserType.SuperAdmin) && u.row_sta_cd == (int)Enums.RowStatus.Active
                        select new UserEntity()
                        {
                            //ClientSecret = u.client_secret,
                            ParentID = u.parent_id,
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            StaffID = u.staff_id,
                            StudentID = u.student_id,
                            UserID = u.user_id,
                            Username = u.username,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = b.branch_name,
                                ContactNo = b.contact_no,
                                aliasName = b.alias_name
                            },
                            StaffDetail = new StaffEntity()
                            {
                                Name = u.BRANCH_STAFF.name
                            },
                            Transaction = new TransactionEntity()
                            {
                                TransactionId = u.TRANSACTION_MASTER.trans_id
                            },
                            UserType = u.user_type == 5 ? Enums.UserType.SuperAdmin : u.user_type == 1 ? Enums.UserType.Admin : u.user_type == 2 ? Enums.UserType.Student : u.user_type == 3 ? Enums.UserType.Parent : Enums.UserType.Staff
                        }).FirstOrDefault();

            if (user != null)
            {
                user.Roles = this.GetRolesByUser(user.UserID);
                if (user.UserType == Enums.UserType.Student)
                {
                    Student.Student stu = new Student.Student();
                    var studentData = await stu.GetStudentByID(user.StudentID.Value);
                    user.StudentDetail = studentData;
                }
            }
            return user;
        }

        public async Task<UserEntity> Check_UserName(string userName)
        {
            var user = (from u in this.context.USER_DEF
                        .Include("BRANCH_STAFF")
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        where u.username == userName && (u.user_type == (int)Enums.UserType.Staff || u.user_type == (int)Enums.UserType.Admin || u.user_type == (int)Enums.UserType.SuperAdmin) && u.row_sta_cd == (int)Enums.RowStatus.Active
                        select new UserEntity()
                        {
                            Username = u.username,
                            Password = u.password,
                            ClientSecret = u.BRANCH_STAFF.name
                        }).FirstOrDefault();
            return user;
        }

        public async Task<UserEntity> ValidateStudent(string userName, string password)
        {
            var user = (from u in this.context.USER_DEF
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        where u.username == userName && u.password == password && u.user_type == (int)Enums.UserType.Student && u.row_sta_cd == (int)Enums.RowStatus.Active
                        select new UserEntity()
                        {
                            //ClientSecret = u.client_secret,
                            ParentID = u.parent_id,
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            StaffID = u.staff_id,
                            StudentID = u.student_id,
                            UserID = u.user_id,
                            Username = u.username,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = b.branch_name,
                                ContactNo = b.contact_no
                            },
                            UserType = u.user_type == 5 ? Enums.UserType.SuperAdmin : u.user_type == 1 ? Enums.UserType.Admin : u.user_type == 2 ? Enums.UserType.Student : u.user_type == 3 ? Enums.UserType.Parent : Enums.UserType.Staff
                        }).FirstOrDefault();

            if (user != null)
            {
                user.Roles = this.GetRolesByUser(user.UserID);
                if (user.UserType == Enums.UserType.Student)
                {
                    Student.Student stu = new Student.Student();
                    var studentData = await stu.GetStudentByID(user.StudentID.Value);
                    user.StudentDetail = studentData;
                }
            }
            else
            {
                user = new UserEntity();
            }
            return user;
        }

       public async Task<UserEntity> ValidateStudentData(string userName,string password)
        {
            var user = (from u in this.context.USER_DEF
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        where u.username == userName && u.password == password && u.user_type == (int)Enums.UserType.Student && u.row_sta_cd == (int)Enums.RowStatus.Active
                        select new UserEntity()
                        {
                            //ClientSecret = u.client_secret,
                            ParentID = u.parent_id,
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            StaffID = u.staff_id,
                            StudentID = u.student_id,
                            UserID = u.user_id,
                            Username = u.username,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = b.branch_name,
                                ContactNo = b.contact_no
                            },
                            UserType = u.user_type == 5 ? Enums.UserType.SuperAdmin : u.user_type == 1 ? Enums.UserType.Admin : u.user_type == 2 ? Enums.UserType.Student : u.user_type == 3 ? Enums.UserType.Parent : Enums.UserType.Staff
                        }).FirstOrDefault();

            if (user != null)
            {
                user.Roles = this.GetRolesByUser(user.UserID);
                if (user.UserType == Enums.UserType.Student)
                {
                    Student.Student stu = new Student.Student();
                    var studentData = await stu.GetStudentByID(user.StudentID.Value);
                    user.StudentDetail = studentData;
                }
                 var z = (from u in this.context.USER_DEF
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        where u.username == userName && u.user_type == (int)Enums.UserType.Student && u.row_sta_cd == (int)Enums.RowStatus.Active
                        select new UserEntity()
                        {
                            StudentID = u.student_id,
                            UserID = u.user_id,
                            Username = u.username
                        }).ToList();
                user.userEntities = z;
                if (z?.Count > 0)
                {
                    user.studentEntities = new List<StudentEntity>();
                    foreach (var item in z)
                    {
                        Student.Student stu = new Student.Student();
                        var studentData = await stu.GetStudentByID(item.StudentID.Value);
                        user.studentEntities.Add(studentData);
                    }
                }
            }
            else
            {
                user = new UserEntity();
            }
            return user;
        }

        public List<UserEntity> GetAllUsers(long branchID, List<int> userType)
        {
            bool noUserType = userType.Count == 0;
            var data = (from u in this.context.USER_DEF
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        join tdUserType in this.context.TYPE_DESC on u.user_type equals tdUserType.type
                        join stud in this.context.STUDENT_MASTER on u.student_id equals stud.student_id into tempStud
                        from student in tempStud.DefaultIfEmpty()
                        join par in this.context.STUDENT_MAINT on u.parent_id equals par.parent_id into tempPar
                        from parent in tempPar.DefaultIfEmpty()
                        join staff in this.context.BRANCH_STAFF on u.staff_id equals staff.staff_id into tempStaff
                        from stf in tempStaff.DefaultIfEmpty()
                        orderby u.user_id descending
                        where (branchID == 0 || u.branch_id == branchID) && tdUserType.@class == (int)Enums.ClassID.UserType
                        && (noUserType || userType.Contains(u.user_type))
                        select new UserEntity()
                        {
                            //ClientSecret = u.client_secret,
                            ParentID = u.parent_id,
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            StaffID = u.staff_id,
                            StudentID = u.student_id,
                            UserID = u.user_id,
                            Username = u.username,
                            UserType = u.user_type == 5 ? Enums.UserType.SuperAdmin : u.user_type == 1 ? Enums.UserType.Admin : u.user_type == 2 ? Enums.UserType.Student : u.user_type == 3 ? Enums.UserType.Parent : Enums.UserType.Staff,
                            UserTypeText = tdUserType.short_desc,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = b.branch_name
                            },
                            ParentDetail = parent != null ? new StudentMaint()
                            {
                                ParentName = parent.parent_name,
                                ContactNo = parent.contact_no,
                                FatherOccupation = parent.father_occupation,
                                MotherOccupation = parent.mother_occupation,
                                ParentID = parent.parent_id,
                                StudentID = parent.student_id
                            } : null,
                            StaffDetail = stf != null ? new StaffEntity()
                            {
                                Name = stf.name,
                                Address = stf.address,
                                ApptDT = stf.appt_dt,
                                DOB = stf.dob,
                                Education = stf.education,
                                EmailID = stf.email_id,
                                JoinDT = stf.join_dt,
                                StaffID = stf.staff_id,
                                MobileNo = stf.mobile_no,
                                LeavingDT = stf.leaving_dt
                            } : null,
                            StudentDetail = student != null ? new StudentEntity()
                            {
                                FirstName = student.first_name,
                                Address = student.address,
                                AdmissionDate = student.admission_date,
                                ContactNo = student.contact_no,
                                DOB = student.dob,
                                Grade = student.grade,
                                GrNo = student.gr_no,
                                LastName = student.last_name
                            } : null
                        }).ToList();

            if (data != null)
            {
                foreach (var item in data)
                {
                    data[data.IndexOf(item)].Roles = this.GetRolesByUser(item.UserID);
                }
            }
            return data;
        }

        public List<UserEntity> GetAllUsers(string userName, string contactNo)
        {
            var data = (from u in this.context.USER_DEF
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        join tdUserType in this.context.TYPE_DESC on u.user_type equals tdUserType.type
                        join stud in this.context.STUDENT_MASTER on u.student_id equals stud.student_id into tempStud
                        from student in tempStud.DefaultIfEmpty()
                        join par in this.context.STUDENT_MAINT on u.parent_id equals par.parent_id into tempPar
                        from parent in tempPar.DefaultIfEmpty()
                        join staff in this.context.BRANCH_STAFF on u.staff_id equals staff.staff_id into tempStaff
                        from stf in tempStaff.DefaultIfEmpty()
                        orderby u.user_id descending
                        where (string.IsNullOrEmpty(userName) || u.username == userName) && tdUserType.@class == (int)Enums.ClassID.UserType
                        && (string.IsNullOrEmpty(contactNo) || (student != null ? student.contact_no == contactNo : 1 == 1 && stf != null ? stf.mobile_no == contactNo : 1 == 1))
                        select new UserEntity()
                        {
                            //ClientSecret = u.client_secret,
                            ParentID = u.parent_id,
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            StaffID = u.staff_id,
                            StudentID = u.student_id,
                            UserID = u.user_id,
                            Username = u.username,
                            UserType = u.user_type == 5 ? Enums.UserType.SuperAdmin : u.user_type == 1 ? Enums.UserType.Admin : u.user_type == 2 ? Enums.UserType.Student : u.user_type == 3 ? Enums.UserType.Parent : Enums.UserType.Staff,
                            UserTypeText = tdUserType.short_desc,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = b.branch_name
                            },
                            ParentDetail = parent != null ? new StudentMaint()
                            {
                                ParentName = parent.parent_name,
                                ContactNo = parent.contact_no,
                                FatherOccupation = parent.father_occupation,
                                MotherOccupation = parent.mother_occupation,
                                ParentID = parent.parent_id,
                                StudentID = parent.student_id
                            } : null,
                            StaffDetail = stf != null ? new StaffEntity()
                            {
                                Name = stf.name,
                                Address = stf.address,
                                ApptDT = stf.appt_dt,
                                DOB = stf.dob,
                                Education = stf.education,
                                EmailID = stf.email_id,
                                JoinDT = stf.join_dt,
                                StaffID = stf.staff_id,
                                MobileNo = stf.mobile_no,
                                LeavingDT = stf.leaving_dt
                            } : null,
                            StudentDetail = student != null ? new StudentEntity()
                            {
                                FirstName = student.first_name,
                                Address = student.address,
                                AdmissionDate = student.admission_date,
                                ContactNo = student.contact_no,
                                DOB = student.dob,
                                Grade = student.grade,
                                GrNo = student.gr_no,
                                LastName = student.last_name
                            } : null
                        }).ToList();

            if (data != null)
            {
                foreach (var item in data)
                {
                    //var roles = (from r in this.context.USER_ROLE
                    //             where r.user_id == item.UserID
                    //             select new RolesEntity()
                    //             {
                    //                 UserID = r.user_id,
                    //                 RoleID = r.id,
                    //                 HasAccess = r.has_priv == "Y",
                    //                 Permission = r.role_id == 1 ? Common.Enums.Roles.Student :
                    //                 r.role_id == 2 ? Common.Enums.Roles.Staff :
                    //                 r.role_id == 3 ? Common.Enums.Roles.Standard :
                    //                 r.role_id == 4 ? Common.Enums.Roles.School :
                    //                 r.role_id == 5 ? Common.Enums.Roles.Subject :
                    //                 r.role_id == 6 ? Common.Enums.Roles.Announcement :
                    //                 r.role_id == 7 ? Common.Enums.Roles.Batch :
                    //                 r.role_id == 8 ? Common.Enums.Roles.OnlinePayment :
                    //                 r.role_id == 9 ? Common.Enums.Roles.AddUpiDetail :
                    //                 r.role_id == 10 ? Common.Enums.Roles.Attendance :
                    //                 r.role_id == 11 ? Common.Enums.Roles.TestSchedule :
                    //                 r.role_id == 12 ? Common.Enums.Roles.TestMarks :
                    //                 r.role_id == 13 ? Common.Enums.Roles.FeeStructure :
                    //                 r.role_id == 14 ? Common.Enums.Roles.PracticePaper :
                    //                 r.role_id == 15 ? Common.Enums.Roles.Homework :
                    //                 r.role_id == 16 ? Common.Enums.Roles.Gallery :
                    //                 r.role_id == 17 ? Common.Enums.Roles.Video :
                    //                 r.role_id == 18 ? Common.Enums.Roles.LiveVideo :
                    //                 Common.Enums.Roles.UploadTestPaper
                    //             }).ToList();
                    data[data.IndexOf(item)].Roles = this.GetRolesByUser(item.UserID);
                }
            }
            return data;
        }

        public async Task<UserEntity> GetUserByUserID(long userID)
        {
            var data = (from u in this.context.USER_DEF
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        join stud in this.context.STUDENT_MASTER on u.student_id equals stud.student_id into tempStud
                        from student in tempStud.DefaultIfEmpty()
                        where u.user_id == userID
                        select new UserEntity()
                        {
                            StudentDetail = new StudentEntity()
                            {
                                FirstName = student.first_name,
                                Address = student.address,
                                AdmissionDate = student.admission_date,
                                ContactNo = student.contact_no,
                                DOB = student.dob,
                                Grade = student.grade,
                                GrNo = student.gr_no,
                                LastName = student.last_name,
                                BatchInfo = new BatchEntity()
                                {
                                    BatchID = student.batch_time
                                },
                                FilePath = "https://mastermind.org.in" + student.file_path,
                                FileName = student.file_name
                            }
                        }).FirstOrDefault();

            return data;
        }

        public List<RolesEntity> GetRolesByUser(long userID)
        {
            var roles = (from r in this.context.USER_ROLE
                         where r.user_id == userID
                         select new RolesEntity()
                         {
                             UserID = r.user_id,
                             RoleID = r.id,
                             HasAccess = r.has_priv == "Y",
                             Permission = r.role_id == 1 ? Common.Enums.Roles.Student :
                             r.role_id == 2 ? Common.Enums.Roles.Staff :
                             r.role_id == 3 ? Common.Enums.Roles.Standard :
                             r.role_id == 4 ? Common.Enums.Roles.School :
                             r.role_id == 5 ? Common.Enums.Roles.Subject :
                             r.role_id == 6 ? Common.Enums.Roles.Announcement :
                             r.role_id == 7 ? Common.Enums.Roles.Batch :
                             r.role_id == 8 ? Common.Enums.Roles.OnlinePayment :
                             r.role_id == 9 ? Common.Enums.Roles.AddUpiDetail :
                             r.role_id == 10 ? Common.Enums.Roles.Attendance :
                             r.role_id == 11 ? Common.Enums.Roles.TestSchedule :
                             r.role_id == 12 ? Common.Enums.Roles.TestMarks :
                             r.role_id == 13 ? Common.Enums.Roles.FeeStructure :
                             r.role_id == 14 ? Common.Enums.Roles.PracticePaper :
                             r.role_id == 15 ? Common.Enums.Roles.Homework :
                             r.role_id == 16 ? Common.Enums.Roles.Gallery :
                             r.role_id == 17 ? Common.Enums.Roles.Video :
                             r.role_id == 18 ? Common.Enums.Roles.LiveVideo :
                             Common.Enums.Roles.UploadTestPaper,
                             RoleName = r.role_id == 1 ? "Student" :
                             r.role_id == 2 ? "Staff" :
                             r.role_id == 3 ? "Standard" :
                             r.role_id == 4 ? "School" :
                             r.role_id == 5 ? "Subject" :
                             r.role_id == 6 ? "Announcement" :
                             r.role_id == 7 ? "Batch" :
                             r.role_id == 8 ? "OnlinePayment" :
                             r.role_id == 9 ? "AddUpiDetail" :
                             r.role_id == 10 ? "Attendance" :
                             r.role_id == 11 ? "TestSchedule" :
                             r.role_id == 12 ? "TestMarks" :
                             r.role_id == 13 ? "FeeStructure" :
                             r.role_id == 14 ? "PracticePaper" :
                             r.role_id == 15 ? "Homework" :
                             r.role_id == 16 ? "Gallery" :
                             r.role_id == 17 ? "Video" :
                             r.role_id == 18 ? "LiveVideo" :
                             "UploadTestPaper"
                         }).ToList();
            return roles;
        }

        public bool AddUserRoles(UserEntity user)
        {
            try
            {
                if (user.Roles?.Count > 0)
                {
                    var currentRoles = (from role in this.context.USER_ROLE
                                        where role.user_id == user.UserID
                                        select role).ToList();
                    if (currentRoles?.Count > 0)
                    {
                        this.context.USER_ROLE.RemoveRange(currentRoles);
                        this.context.SaveChanges();
                    }

                    List<USER_ROLE> data = new List<USER_ROLE>();
                    foreach (var item in user.Roles)
                    {
                        //data.Add(new USER_ROLE()
                        //{
                        //    role_id = item.PermissionValue,
                        //    user_id = item.UserID,
                        //    row_sta_cd = (int)Enums.RowStatus.Active,
                        //    trans_id = this.AddTransactionData(user.Transaction),
                        //    has_priv = item.HasAccess ? "Y" : "N"
                        //});
                        USER_ROLE rl = new USER_ROLE();
                        rl.role_id = item.PermissionValue;
                        rl.user_id = item.UserID;
                        rl.row_sta_cd = (int)Enums.RowStatus.Active;
                        rl.trans_id = this.AddTransactionData(user.Transaction);
                        rl.has_priv = item.HasAccess ? "Y" : "N";
                        data.Add(rl);
                    }

                    this.context.USER_ROLE.AddRange(data);
                    this.context.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {

            }

            return false;
        }

        public async Task<bool> ChangePassword(long userID, string password, string oldPassword)
        {
            var user = this.context.USER_DEF
                .Where(x => x.user_id == userID
                && x.password == oldPassword).FirstOrDefault();
            if (user != null)
            {
                user.password = password;
                return this.context.SaveChanges() > 0;
            }

            return false;
        }

        public bool RemoveUser(long userID, string lastupdatedby)
        {
            var data = (from u in this.context.USER_DEF
                        where u.user_id == userID
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

        public List<UserEntity> GetAllUsersddl(long branchID)
        {

            var data = (from u in this.context.BRANCH_STAFF
                        join UD in this.context.USER_DEF on u.staff_id equals UD.staff_id
                        orderby u.staff_id descending
                        where u.branch_id == branchID
                        && UD.user_type == (int)Enums.UserType.Staff
                        && UD.row_sta_cd == (int)Enums.RowStatus.Active
                        && u.row_sta_cd == (int)Enums.RowStatus.Active
                        select new UserEntity()
                        {
                            UserID = u.staff_id,
                            Username = u.name,
                        }).ToList();

            if (data != null)
            {
                foreach (var item in data)
                {
                    data[data.IndexOf(item)].Roles = this.GetRolesByUser(item.UserID);
                }
            }
            return data;
        }

        public async Task<bool> CheckAgreement(long branchID)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var ToDayDate = indianTime.ToString("yyyy/MM/dd");
            DateTime dt = DateTime.ParseExact(ToDayDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var data = (from u in this.context.BRANCH_AGREEMENT
                        where u.branch_id == branchID
                        && u.to_dt > dt && u.row_sta_cd == 1
                        select new BranchAgreementEntity()
                        {
                            AgreementFromDate = u.from_dt,
                            AgreementToDate = u.to_dt
                        }).FirstOrDefault();
            if (data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdatefcmToken(UserEntity userentity, string fcm_token)
        {
            var user = this.context.USER_DEF.Where(x => x.user_id == userentity.UserID).FirstOrDefault();
            if (user != null)
            {
                user.fcm_token = fcm_token;
               return this.context.SaveChanges()>0;
            }
            return false;
        }



        public async Task<ResponseModel> StudentUserMaintenance(UserEntity userInfo)
        {
            try
            {
                Model.USER_DEF user = new Model.USER_DEF();
                bool isUpdate = true;
                var data = (from u in this.context.USER_DEF
                            where u.student_id == userInfo.StudentID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    user = data;
                    user.trans_id = this.AddTransactionData(userInfo.Transaction);
                    user.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    this.context.USER_DEF.Add(user);
                    this.context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    var res = this.context.SaveChanges();
                    if (res > 0)
                    {
                        user.branch_id = userInfo.BranchInfo.BranchID;
                        user.user_type = (int)userInfo.UserType;
                        user.parent_id = userInfo.ParentID;
                        user.password = userInfo.Password;
                        user.row_sta_cd = userInfo.RowStatus.RowStatusId;
                        user.staff_id = userInfo.StaffID;
                        user.student_id = userInfo.StudentID;
                        user.trans_id = this.AddTransactionData(userInfo.Transaction);
                        user.username = userInfo.Username;
                        this.context.USER_DEF.Add(user);
                        res = this.context.SaveChanges();
                        if (res > 0)
                        {
                            responseModel.Status = true;
                            responseModel.Message = "Student Trasnfer Successfully!!";
                        }
                        else
                        {
                            responseModel.Status = false;
                            responseModel.Message = "Failed To  Transfer Student!!";
                        }
                    }
                    else
                    {
                        responseModel.Status = false;
                        responseModel.Message = "Failed To  Transfer Student!!";
                    }
                }
            }
            catch(Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message;
            }


            return responseModel;
        }
    }
}
