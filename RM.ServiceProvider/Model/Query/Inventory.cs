using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.ServiceProvider.Model
{
    public class Inventory
    {
        public string material_type { get; set; }
        public string material_name { get; set; }
        public int qua { get; set; }
        public double price { get; set; }
        public int material_id { get; set; }
        public string Material_Specification { get; set; } //物料规格
        public string Material_Unit { get; set; } //物料单位
    }
}
