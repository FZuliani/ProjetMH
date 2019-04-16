using DataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Connections.Databases;
using ToolBox.Patterns.Locator;

namespace InterfaceMH.Service
{
    public class AccordServices : IAccordServices
    {

        public readonly string _Enpoint;

        public AccordServices()
        {
            _Enpoint = "http://localhost:55767/api";
        }
        
        public async Task<string> Get(double frequence)
        {
            
            using(HttpClient client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync($"{_Enpoint}/Accord/{frequence}/");
                if (message.IsSuccessStatusCode)
                {
                    HttpContent content = message.Content;
                    string Accord = await content.ReadAsStringAsync();
                    Accord = Accord.Replace("(", "");
                    Accord = Accord.Replace(")", "");
                    
                    return Accord;
                }
            }
            return null;

        }

    }
}
