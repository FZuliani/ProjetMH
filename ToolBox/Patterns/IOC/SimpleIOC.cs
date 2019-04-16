using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Patterns.IOC
{
    public class SimpleIOC : ISimpleIOC
    {
        private IDictionary<Type, object> _Instances;
        private IDictionary<Type, CtorInfo> _Infos;

        public SimpleIOC()
        {
            _Instances = new Dictionary<Type, object>();
            _Infos = new Dictionary<Type, CtorInfo>();
        }

        public TResource GetInstance<TResource>()
        {
            Type resourceType = typeof(TResource);

            if (!_Instances.ContainsKey(resourceType))
                throw new InvalidOperationException("Please register your resource before use it!");

            if (_Instances[resourceType] == null)
                _Instances[resourceType] = Activator.CreateInstance(_Infos[resourceType].TResource, _Infos[resourceType].Parameters);

            return (TResource)_Instances[resourceType];
        }

        public void Register<TResource>()
        {
            Register<TResource>(null);
        }

        public void Register<TResource>(params object[] parameters)
        {
            Type resourceType = typeof(TResource);
            _Instances.Add(resourceType, null);
            _Infos.Add(resourceType, new CtorInfo(resourceType, parameters));
        }

        public void Register<TInferface, TResource>()
            where TResource : class, TInferface
        {
            Register<TInferface, TResource>(null);
        }

        public void Register<TInferface, TResource>(params object[] parameters)
            where TResource : class, TInferface
        {
            Type interfaceType = typeof(TInferface);

            if (!interfaceType.IsInterface)
                throw new InvalidOperationException("TInterface must be an interface!");

            Type resourceType = typeof(TResource);
            _Instances.Add(interfaceType, null);
            _Infos.Add(interfaceType, new CtorInfo(resourceType, parameters));
        }

        private class CtorInfo
        {
            internal Type TResource { get; private set; }
            internal object[] Parameters { get; private set; }

            public CtorInfo(Type resourceType, object[] parameters)
            {
                TResource = resourceType;
                Parameters = parameters;
            }
        }
    }
}
