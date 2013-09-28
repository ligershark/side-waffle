using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace $rootnamespace$
{
    public static class $safeitemname$Config
    {
        public static void Setup()
        {
            ControllerBuilder.Current.SetControllerFactory(
                new $safeitemname$());
        }
    }

    public class $safeitemname$ : DefaultControllerFactory
    {
        private IKernel kernel = new StandardKernel(new AplicationIocServices());

        protected override IController GetControllerInstance(
            RequestContext requestContext,
            Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }
            return (IController)kernel.Get(controllerType);
        }

        private class AplicationIocServices : NinjectModule
        {
            public override void Load()
            {
                /*
                 * set up your services in this area
                 * Bind<ISampleService>().To<SampleService>();
                */
            }
        }
    }
}