using Ashirvad.Repo.DataAcceessAPI.Area.Attendance;
using Ashirvad.Repo.DataAcceessAPI.Area.Banner;
using Ashirvad.Repo.DataAcceessAPI.Area.Batch;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using Ashirvad.Repo.DataAcceessAPI.Area.Gallery;
using Ashirvad.Repo.DataAcceessAPI.Area.Homework;
using Ashirvad.Repo.DataAcceessAPI.Area.Library;
using Ashirvad.Repo.DataAcceessAPI.Area.Link;
using Ashirvad.Repo.DataAcceessAPI.Area.Notification;
using Ashirvad.Repo.DataAcceessAPI.Area.Paper;
using Ashirvad.Repo.DataAcceessAPI.Area.Reminder;
using Ashirvad.Repo.DataAcceessAPI.Area.School;
using Ashirvad.Repo.DataAcceessAPI.Area.Staff;
using Ashirvad.Repo.DataAcceessAPI.Area.Standard;
using Ashirvad.Repo.DataAcceessAPI.Area.Student;
using Ashirvad.Repo.DataAcceessAPI.Area.Subject;
using Ashirvad.Repo.DataAcceessAPI.Area.Test;
using Ashirvad.Repo.DataAcceessAPI.Area.ToDo;
using Ashirvad.Repo.DataAcceessAPI.Area.AboutUs;
using Ashirvad.Repo.DataAcceessAPI.Area.User;
using Ashirvad.Repo.Services.Area.AboutUs;
using Ashirvad.Repo.Services.Area.Attendance;
using Ashirvad.Repo.Services.Area.Banner;
using Ashirvad.Repo.Services.Area.Batch;
using Ashirvad.Repo.Services.Area.Branch;
using Ashirvad.Repo.Services.Area.Gallery;
using Ashirvad.Repo.Services.Area.Homework;
using Ashirvad.Repo.Services.Area.Library;
using Ashirvad.Repo.Services.Area.Link;
using Ashirvad.Repo.Services.Area.Notification;
using Ashirvad.Repo.Services.Area.Paper;
using Ashirvad.Repo.Services.Area.Reminder;
using Ashirvad.Repo.Services.Area.School;
using Ashirvad.Repo.Services.Area.Staff;
using Ashirvad.Repo.Services.Area.Standard;
using Ashirvad.Repo.Services.Area.Student;
using Ashirvad.Repo.Services.Area.Subject;
using Ashirvad.Repo.Services.Area.Test;
using Ashirvad.Repo.Services.Area.ToDo;
using Ashirvad.Repo.Services.Area.User;
using Ashirvad.ServiceAPI.ServiceAPI.Area.AboutUs;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Banner;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Batch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Gallery;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Homework;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Link;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Notification;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Paper;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Reminder;
using Ashirvad.ServiceAPI.ServiceAPI.Area.School;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Staff;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Standard;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Subject;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Test;
using Ashirvad.ServiceAPI.ServiceAPI.Area.ToDo;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using Ashirvad.ServiceAPI.Services.Area.AboutUs;
using Ashirvad.ServiceAPI.Services.Area.Attendance;
using Ashirvad.ServiceAPI.Services.Area.Banner;
using Ashirvad.ServiceAPI.Services.Area.Batch;
using Ashirvad.ServiceAPI.Services.Area.Branch;
using Ashirvad.ServiceAPI.Services.Area.Homework;
using Ashirvad.ServiceAPI.Services.Area.Library;
using Ashirvad.ServiceAPI.Services.Area.Notification;
using Ashirvad.ServiceAPI.Services.Area.Paper;
using Ashirvad.ServiceAPI.Services.Area.Reminder;
using Ashirvad.ServiceAPI.Services.Area.School;
using Ashirvad.ServiceAPI.Services.Area.Staff;
using Ashirvad.ServiceAPI.Services.Area.Standard;
using Ashirvad.ServiceAPI.Services.Area.Student;
using Ashirvad.ServiceAPI.Services.Area.Subject;
using Ashirvad.ServiceAPI.Services.Area.Test;
using Ashirvad.ServiceAPI.Services.Area.ToDo;
using Ashirvad.ServiceAPI.Services.Area.User;
using Ashirvad.ServiceAPI.Services.Gallery;
using Ashirvad.ServiceAPI.Services.Link;
using System.Web.Http;
using Unity;
using Unity.WebApi;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.Services.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Fees;
using Ashirvad.Repo.Services.Area;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Page;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Page;
using Ashirvad.Repo.DataAcceessAPI.Area.Package;
using Ashirvad.Repo.Services.Area.Package;
using Ashirvad.ServiceAPI.Services.Area.Page;
using Ashirvad.Repo.Services.Area.Page;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Package;
using Ashirvad.ServiceAPI.Services.Area.Package;
using Ashirvad.Repo.DataAcceessAPI.Area.Faculty;
using Ashirvad.Repo.Services.Area.Faculty;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Faculty;
using Ashirvad.ServiceAPI.Services.Area.Faculty;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Announcement;
using Ashirvad.ServiceAPI.Services.Area.Announcement;
using Ashirvad.Repo.DataAcceessAPI.Area.Announcement;
using Ashirvad.Repo.Services.Area.Announcement;
using Ashirvad.Repo.DataAcceessAPI.Area.Circular;
using Ashirvad.Repo.Services.Area.Circular;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Circular;
using Ashirvad.ServiceAPI.Services.Area.Circular;
using Ashirvad.Repo.DataAcceessAPI.Area.PaymentRegister;
using Ashirvad.Repo.Services.Area.PaymentRegister;
using Ashirvad.ServiceAPI.ServiceAPI.Area.PaymentRegister;
using Ashirvad.ServiceAPI.Services.Area.PaymentRegister;
using Ashirvad.Repo.DataAcceessAPI.Area.Role;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Role;
using Ashirvad.ServiceAPI.Services.Area.Role;
using Ashirvad.Repo.DataAcceessAPI.Area.RoleRights;
using Ashirvad.Repo.Services.Area.RoleRights;
using Ashirvad.ServiceAPI.Services.Area.RoleRights;
using Ashirvad.ServiceAPI.ServiceAPI.Area.RoleRights;
using Ashirvad.Repo.DataAcceessAPI.Area.UserRights;
using Ashirvad.ServiceAPI.ServiceAPI.Area.UserRights;
using Ashirvad.ServiceAPI.Services.Area.UserRights;
using Ashirvad.Repo.Services.Area.UserRights;
using Ashirvad.Repo.DataAcceessAPI.Area.Competition;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton;
using Ashirvad.ServiceAPI.Services.Area.Competition;
using Ashirvad.Repo.Services.Area.Competiton;

namespace Ashirvad.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IUserAPI, User>();

            container.RegisterType<IBranchService, BranchService>();
            container.RegisterType<IBranchAPI, Branch>();

            container.RegisterType<IStaffService, StaffService>();
            container.RegisterType<IStaffAPI, Staff>();

            container.RegisterType<IStudentService, StudentService>();
            container.RegisterType<IStudentAPI, Student>();

            container.RegisterType<ISchoolService, SchoolService>();
            container.RegisterType<ISchoolAPI, School>();

            container.RegisterType<IStandardService, StandardService>();
            container.RegisterType<IStandardAPI, Standard>();

            container.RegisterType<IAttendanceService, AttendanceService>();
            container.RegisterType<IAttendanceAPI, Attendance>();

            container.RegisterType<IBatchService, BatchService>();
            container.RegisterType<IBatchAPI, Batch>();
               
            container.RegisterType<ISubjectService, SubjectService>();
            container.RegisterType<ISubjectAPI, Subject>();

            container.RegisterType<IGalleryService, GalleryService>();
            container.RegisterType<IGalleryAPI, Gallery>();

            container.RegisterType<ILinkService, LinkService>();
            container.RegisterType<ILinkAPI, LinkMGMT>();

            container.RegisterType<IBannerService, BannerService>();
            container.RegisterType<IBannerAPI, Banner>();

            container.RegisterType<INotificationService, NotificationService>();
            container.RegisterType<INotificationAPI, Notification>();

            container.RegisterType<IBannerService, BannerService>();
            container.RegisterType<IBannerAPI, Banner>();

            container.RegisterType<IPaperService, PaperService>();
            container.RegisterType<IPaperAPI, Paper>();

            container.RegisterType<ILibraryService, LibraryService>();
            container.RegisterType<ILibraryAPI, Library>();

            container.RegisterType<IReminderService, ReminderService>();
            container.RegisterType<IReminderAPI, Reminder>();

            container.RegisterType<IToDoService, ToDoService>();
            container.RegisterType<IToDoAPI, ToDo>();

            container.RegisterType<IHomeworkService, HomeworkService>();
            container.RegisterType<IHomeworkAPI, Homework>();

            container.RegisterType<ITestService, TestService>();
            container.RegisterType<ITestAPI, Test>();

            container.RegisterType<IAboutUsService, AboutUsService>();
            container.RegisterType<IAboutUs, AboutUs>();

            container.RegisterType<IFeesService, FeesService>();
            container.RegisterType<IFeesAPI, Fees>();

            container.RegisterType<IHomeworkDetailService, HomeworkDetailService>();
            container.RegisterType<IHomeworkDetailsAPI, HomeworkDetails>();

            container.RegisterType<IMarksAPI, Marks>();
            container.RegisterType<IMarksService, MarksService>();

            container.RegisterType<ICategoryAPI, Category>();
            container.RegisterType<ICategoryService, CategoryService>();

            container.RegisterType<ILibrary1API, Library1>();
            container.RegisterType<ILibrary1Service, Library1Service>();

            container.RegisterType<IPageAPI, Pages>();
            container.RegisterType<IPageService, PageService>();

            container.RegisterType<IPackageAPI, Package>();
            container.RegisterType<IPackageService, PackageService>();

            container.RegisterType<IPackageRightsAPI, PackageRights>();
            container.RegisterType<IPackageRightsService, PackageRightsService>();

            container.RegisterType<IBranchRightsAPI, BranchRights>();
            container.RegisterType<IBranchRightsService, BranchRightsService>();

            container.RegisterType<IFacultyAPI, Faculty>();
            container.RegisterType<IFacultyService, FacultyService>();

            container.RegisterType<IBranchCourseAPI, BranchCourse>();
            container.RegisterType<IBranchCourseService, BranchCourseService>();

            container.RegisterType<IBranchClassAPI, BranchClass>();
            container.RegisterType<IBranchClassService, BranchClassService>();

            container.RegisterType<IBranchSubjectAPI, BranchSubject>();
            container.RegisterType<IBranchSubjectService, BranchSubjectService>();

            container.RegisterType<IAnnouncementService, AnnouncementService>();
            container.RegisterType<IAnnouncementAPI, Announcement>();

            container.RegisterType<ICircularAPI, Circular>();
            container.RegisterType<ICircularService, CircularService>();

            container.RegisterType<IPaymentRegisterAPI, PaymentRegister>();
            container.RegisterType<IPaymentRegisterService, PaymentRegisterService>();

            container.RegisterType<IRoleAPI, RoleAPI>();
            container.RegisterType<IRoleService, RoleService>();

            container.RegisterType<IRoleRightsAPI, RoleRightsAPI>();
            container.RegisterType<IRoleRightsService, RoleRightsService>();

            container.RegisterType<IUserRightsAPI, UserRightsAPI>();
            container.RegisterType<IUserRightsService, UserRightsService>();

            container.RegisterType<ICompetitonAPI, Competition>();
            container.RegisterType<ICompetitonService, CompetitionService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

        }
    }
}