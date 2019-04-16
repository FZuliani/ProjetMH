using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Patterns.Locator;

namespace InterfaceMH.Service
{
    public class ServiceLocator : LocatorBase
    {


        public ServiceLocator()
        {
            Container.Register<IAccordServices, AccordServices>();
            Container.Register<IServiceStorage, ServiceStorage>();
        }

    }
}
