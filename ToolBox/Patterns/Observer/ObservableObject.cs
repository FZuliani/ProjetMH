using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Patterns.Observer
{
    public class ObservableObject : IObservableObject
    {
        public event Action<IObservableObject> PropertyChanged;

        protected void RaisePropertyChanged()
        {
            PropertyChanged?.Invoke(this);
        }
    }
}
