using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;

namespace $rootnamespace$
{
    public class $safeitemname$ : PhoneBootstrapper
    {
        PhoneContainer container;

        protected override void Configure()
        {
            container = new PhoneContainer();

            container.RegisterPhoneServices(RootFrame);

            //add viewmodels registration here   

            AddCustomConventions();
        }

        static void AddCustomConventions()
        {
           //add custom conventions here (e.g: bindable appbar)
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }           
    }
}
