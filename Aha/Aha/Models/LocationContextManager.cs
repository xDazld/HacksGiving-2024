using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aha.Models
{
    internal class LocationContextManager
    {
        Dictionary<string, LocationContext> URIToLocationContextMap = new Dictionary<string, LocationContext>();
        List<LocationContext> LocationHistory = new List<LocationContext>();

        public static LocationContextManager LocationContextManagerInstance { get; } = new LocationContextManager();
        private LocationContextManager()
        {
            //Read CSV For location Contexts
            using(StreamReader reader = new StreamReader("LocationContexts.csv"))
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(',');
                    URIToLocationContextMap.Add(line[0], new LocationContext(line[1], line[2]));
                }
            }
        }
    }
}