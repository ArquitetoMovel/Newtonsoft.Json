using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.PerformanceTest
{
    [Serializable]
    public class Feira
    {
        public DateTime Dia { get; set; }

        
        public List<Item> FrutasOuLegumes { get; set; }
    }
}
