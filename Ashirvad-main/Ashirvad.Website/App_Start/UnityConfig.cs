using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.Services.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.Services.Area;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Ashirvad.Website
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ILibrary1API, Library1>();
            container.RegisterType<ILibrary1Service, Library1Service>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}