using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.ServiceProvider.Model
{
    public class Inventorydetail
    {
        public string PurchaseBillCode { get; set; } //入库单号
        public string ValueName { get; set; } //
        public string material_name { get; set; } //物料名称
        public int productcode { get; set; }
        public string lot { get; set; } //批号
        public double price { get; set; }  //价格
        public double amount { get; set; }  //金额
        public int quantity { get; set; } //库存
        public string invoicecode { get; set; } //发票号
        public string user_name { get; set; }
        public string operatedate { get; set; }
        public string Material_Specification { get; set; }
        public string Material_Unit { get; set; }
        public string validdate { get; set; }
        public string Material_Type { get; set; }
        public string midtype { get; set; }
    }
}
