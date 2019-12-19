using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.ServiceProvider.Model
{
    public class materialInfo
    {
        public int material_id { get; set; }
        public string material_type { get; set; }
        public string material_name { get; set; }
        public string Material_CommonlyName { get; set; }
        public string Material_Specification { get; set; }
        public string Material_Unit { get; set; }
        public string Material_Supplier { get; set; }
        public int Material_SafetyStock { get; set; }
        public double price { get; set; }
        public int qua { get; set; }
        public string Material_Comm { get; set; }
    }
}
