using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Patterns.IOC
{
    public interface ISimpleIOC
    {
        TResource GetInstance<TResource>();
        void Register<TResource>();
        void Register<TResource>(params object[] parameters);

        void Register<TInferface, TResource>()
            where TResource : class, TInferface;
        void Register<TInferface, TResource>(params object[] parameters)
            where TResource : class, TInferface;
    }
}
