using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevDaysSpeakers.Model
{
    public class Speaker
    {
        //Add speaker attributes here


        //Azure information for version
        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }
    }
}
