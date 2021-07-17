using Ashirvad.Repo.DataAcceessAPI.Area.Batch;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using Ashirvad.Repo.DataAcceessAPI.Area.School;
using Ashirvad.Repo.DataAcceessAPI.Area.Staff;
using Ashirvad.Repo.DataAcceessAPI.Area.Standard;
using Ashirvad.Repo.DataAcceessAPI.Area.Student;
using Ashirvad.Repo.DataAcceessAPI.Area.Subject;
using Ashirvad.Repo.DataAcceessAPI.Area.User;
using Ashirvad.Repo.Services.Area.Batch;
using Ashirvad.Repo.Services.Area.Branch;
using Ashirvad.Repo.Services.Area.School;
using Ashirvad.Repo.Services.Area.Staff;
using Ashirvad.Repo.Services.Area.Standard;
using Ashirvad.Repo.Services.Area.Student;
using Ashirvad.Repo.Services.Area.Subject;
using Ashirvad.Repo.Services.Area.User;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Batch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.School;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Staff;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Standard;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Subject;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using Ashirvad.ServiceAPI.Services.Area.Batch;
using Ashirvad.ServiceAPI.Services.Area.Branch;
using Ashirvad.ServiceAPI.Services.Area.School;
using Ashirvad.ServiceAPI.Services.Area.Staff;
using Ashirvad.ServiceAPI.Services.Area.Standard;
using Ashirvad.ServiceAPI.Services.Area.Student;
using Ashirvad.ServiceAPI.Services.Area.Subject;
using Ashirvad.ServiceAPI.Services.Area.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Unity;
using Unity.WebApi;

namespace Ashirvad.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents();
            //UnityInjection();
        }

        private void UnityInjection()
        {
            
        }
    }
}
