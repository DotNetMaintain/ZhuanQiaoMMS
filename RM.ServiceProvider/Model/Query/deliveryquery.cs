using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.ServiceProvider.Model
{
    public class deliveryquery
    {
        public string PurchaseBillCode { get; set; }
        public int id { get; set; }
        public int checkquantity { get; set; }
        public string DeptName { get; set; }
        public string PurchaseDate { get; set; }
        public string OperatorDate { get; set; }
        public int productcode { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double amount { get; set; }
        public string auditflag { get; set; }
        public string material_type { get; set; }
        public string material_name { get; set; }
        public string Material_Specification { get; set; }
        public string Material_Unit { get; set; }
        public string username { get; set; }
        public string Material_Supplier { get; set; }
        public string Material_Attr01 { get; set; }
        public string midtype { get; set; }
        public int qua { get; set; }
    }
}
