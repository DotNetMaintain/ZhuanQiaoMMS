using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.ServiceProvider.Model
{
    public class storedetail
    {
        public string invoicecode { get; set; } //发票号
        public string PurchaseBillCode { get; set; } //入库单号
        public string midtype { get; set; } //物料大类
        public string material_type { get; set; } //物料类型
        public string material_name { get; set; } //物料名称
        public string Material_Specification { get; set; } //物料规格
        public string Material_Unit { get; set; } //物料单位
        public string Material_Supplier { get; set; } //供应商
        public string lot { get; set; } //批号
        public float price { get; set; }  //价格
        public float quantity { get; set; } //数量
        public float amount { get; set; } //金额
        public string PurchaseDate { get; set; }
        public int id { get; set; }
    }
}
