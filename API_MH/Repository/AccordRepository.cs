using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToolBox.Connections.Databases;
using ToolBox.Patterns.Locator;

namespace API_MH.Repository
{
    public class AccordRepository
    {

        public string Get(double frequence)
        {
            Command command = new Command($"Select Accord From Accord Where [Note] = @frequence");
            command.AddParameter("frequence", frequence);

            string accord = (string)ResourceLocator.Instance.connection.ExecuteScalar(command);
            if(!(accord is null))
            {
                accord = accord.Replace("(", "");
                accord = accord.Replace(")", "");
            }

            return accord;

        }


    }
}