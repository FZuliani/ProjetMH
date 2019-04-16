using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Patterns.IOC;

namespace ToolBox.Patterns.Locator
{
    public abstract class LocatorBase
    {
        public ISimpleIOC Container { get; protected set; }

        protected LocatorBase() : this(new SimpleIOC())
        {

        }

        protected LocatorBase(ISimpleIOC container)
        {
            Container = container;
        }
    }
}
