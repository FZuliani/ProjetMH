using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Patterns.Mediator
{
    public class Messenger<TMessage>
    {
        private static Messenger<TMessage> _Instance;

        public static Messenger<TMessage> Instance
        {
            get
            {
                return _Instance ?? (_Instance = new Messenger<TMessage>());
            }
        }

        private Dictionary<string,Action<TMessage>> _Broadcast;

        protected Messenger()
        {
            _Broadcast = new Dictionary<string, Action<TMessage>>();
        }

        public void Register(string topic, Action<TMessage> action)
        {
            if(_Broadcast.ContainsKey(topic))
                _Broadcast[topic] += action;
            _Broadcast[topic] = action;
        }

        public void Send(string topic, TMessage message)
        {
            if(_Broadcast.ContainsKey(topic))
                _Broadcast[topic]?.Invoke(message);
        }
    }
}
