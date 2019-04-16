using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace API_MH.Models
{
    public class Corde : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;

        public string corde { get; set; }
        
        private string _frette;

        public string frette
        {
            get { return _frette; }
            set {

                    this._frette = value;
                    this.RaisePropertyChanged("frette");
                
            }
        }


        public void RaisePropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}