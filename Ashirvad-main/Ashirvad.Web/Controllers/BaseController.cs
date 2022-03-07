using Ashirvad.Common;
using Ashirvad.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionContext.Instance.LoginUser == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }
           //else if (SessionContext.Instance.userRightsList!=null)
           // {
           //     var list = JsonConvert.DeserializeObject<List<BranchWiseRightEntity>>(SessionContext.Instance.userRightsList);
           //     bool flag = true;
           //     if (list.Count > 0)
           //     {
           //         foreach (var item in list)
           //         {
           //             bool check = false;
           //             string PageName = item.PageInfo.Page;
           //             string Action = filterContext.ActionDescriptor.ActionName;
           //             bool View = item.PackageRightinfo.Viewstatus;
           //             switch (PageName)
           //             {
           //                 case "About Us Master":
           //                     if (Action == "AboutUsMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }

           //                     }
           //                     break;
           //                 case "Add UPI Details":
           //                     if (Action == "UPIMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Announcement Master":
           //                     if (Action == "AnnouncementMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Attendance Entry":
           //                     if (Action == "Index")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Attendance Register":
           //                     if (Action == "Index")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Banner Master":
           //                     if (Action == "BannerMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Batch Master":
           //                     if (Action == "BatchMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;

           //                 case "Category Master":
           //                     if (Action == "CategoryMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Homework":
           //                     if (Action == "HomeworkMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Library Image":
           //                     if (Action == "LibraryMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Library Video":
           //                     if (Action == "LibraryMaintenance")
           //                     {
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;



           //                 case "Live Video":
           //                     if (Action == "LiveVideoMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Manage Student Master":
           //                     if (Action == "ManageStudentMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Marks Entry":
           //                     if (Action == "MarksMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Marks Register":
           //                     if (Action == "Index")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Notification Master":
           //                     if (Action == "NotificationMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Photos":
           //                     if (Action == "PhotosMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Practice Papers":
           //                     if (Action == "PaperMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Reminder Entry":
           //                     if (Action == "ReminderMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Report":
           //                     if (Action == "GetAllReportData")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "School Master":
           //                     if (Action == "SchoolMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Standard Master":
           //                     if (Action == "StandardMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;


           //                 case "Student Fees Structure":
           //                     if (Action == "FeesMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Student Master":
           //                     if (Action == "StudentMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Student Online Payment":
           //                     if (Action == "Index")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;

           //                 case "Subject Master":
           //                     if (Action == "SubjectMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Test Paper Entry":
           //                     if (Action == "TestPaperMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "To-Do Entry":
           //                     if (Action == "ToDoMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "To-Do Register":
           //                     if (Action == "ToDoRegisterMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "User Master":
           //                     if (Action == "UserMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Video":
           //                     if (Action == "VideoMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 case "Youtube Video":
           //                     if (Action == "YoutubeMaintenance")
           //                     {
           //                         check = true;
           //                         if (View == false)
           //                         {
           //                             flag = false;
           //                         }
           //                     }
           //                     break;
           //                 default:
           //                     if (Action == "Index")
           //                     {
           //                         check = true;

           //                     }
           //                     break;
           //             }
           //             if (Action == "Index")
           //             {
           //                 flag = true;
           //             }
           //             if (check)
           //             {
           //                 break;
           //             }

           //         }
           //     }
           //     if (!flag)
           //     {
           //         SessionContext.Instance.LoginUser = null;                    
           //         SessionContext.Instance.Permission = new string[3];
           //         filterContext.Result = new RedirectResult("~/Login/Index");
           //     }
           // }
           // else
           // {
           //     SessionContext.Instance.LoginUser = null;
           //     SessionContext.Instance.Permission = new string[3];
           //     filterContext.Result = new RedirectResult("~/Login/Index");
           // }
        }

        public TransactionEntity GetTransactionData(Enums.TransactionType transType)
        {
            if (transType == Enums.TransactionType.Insert)
            {
                return new TransactionEntity()
                {
                    CreatedBy = SessionContext.Instance.LoginUser.Username,
                    CreatedDate = DateTime.Now,
                    CreatedId = SessionContext.Instance.LoginUser.UserID,
                    FinancialID= SessionContext.Instance.LoginUser.FinancialYear
                };
            }
            else
            {
                return new TransactionEntity()
                {
                    LastUpdateBy = SessionContext.Instance.LoginUser.Username,
                    LastUpdateDate = DateTime.Now,
                    LastUpdateId = SessionContext.Instance.LoginUser.UserID
                    FinancialID = SessionContext.Instance.LoginUser.FinancialYear
                };
            }
        }

        public string[] getpermission(string Page)
        {
            var list = JsonConvert.DeserializeObject<List<BranchWiseRightEntity>>(SessionContext.Instance.userRightsList);
            string[] array = new string[3];
            foreach (var item in list)
            {
                string PageName = item.PageInfo.Page;
                if (PageName == Page)
                {
                    array[0] = "false";
                    array[1] = "false";                    
                    if (item.Createstatus == true)
                    {
                        array[0] = "true";
                    }                    
                    if (item.Deletestatus == true)
                    {
                        array[2] = "true";
                    }
                }
            }
            return array;
        }
    }
}