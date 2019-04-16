using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Connections.Databases;

namespace ToolBox.Patterns.Locator
{
    public class ResourceLocator : LocatorBase
    {


        private static ResourceLocator instance;

        public static ResourceLocator Instance
        {
            get { return instance ?? (instance = new ResourceLocator()); }
        }

        private ResourceLocator()
        {
            Container.Register<Connection>(Resource.connectionASPMVC);          
        }

        public IConnection iconnection
        {
            get { return Container.GetInstance<Connection>(); }
        }

        public Connection connection
        {
            get { return Container.GetInstance<Connection>(); }
        }
    }
}
