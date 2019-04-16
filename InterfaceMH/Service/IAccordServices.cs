using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary;

namespace InterfaceMH.Service
{
    public interface IAccordServices
    {
        Task<string> Get(double  frequence);
       
      
    }
}