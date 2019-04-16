using InterfaceMH.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace InterfaceMH
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        public static ServiceLocator serviceLocator { get; set; }

        public App()
        {
            serviceLocator = new ServiceLocator();
        }

    }
}
