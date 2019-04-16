using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceMH.Service
{
    public class ServiceStorage : IServiceStorage
    {      
        public string Offset
        {
            get
            {               
                if (!File.Exists("storage.json"))
                {
                    return null;
                }
                using (FileStream stream = File.OpenRead("storage.json"))
                {
                    
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line;
                        if((line = reader.ReadLine()) != null)
                        {
                            return line;
                        }
                        return null;
                    }                    
                }                
            }
            set
            {                
                using (FileStream stream = File.OpenWrite("storage.json"))
                {
                    
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(value.ToString());
                    }
                }                
            }
        }
    }
}
