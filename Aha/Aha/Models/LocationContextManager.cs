using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aha.Models
{
    public class LocationContextManager
    {
        public Dictionary<string, string> BLEDeviceNameToLocationContext = new Dictionary<string, string>();
        //List<LocationContext> LocationHistory = new List<LocationContext>(); //For the future if it becomes necessary

        public static LocationContextManager LocationContextManagerInstance { get; } = new LocationContextManager();
        private LocationContextManager()
        {
            //Read CSV For location Contexts
            using(StreamReader reader = new StreamReader(FileSystem.AppDataDirectory+"/LocationContexts.csv"))
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(',');
                    BLEDeviceNameToLocationContext.Add(line[0], line[1]);
                }
            }
        }
    }
}