using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aha.Models
{
    internal struct LocationContext(String LocationName, String LocationDescription)
    {
        String LocationName { get; set; }
        String LocationDescription { get; set; }
    }
}
