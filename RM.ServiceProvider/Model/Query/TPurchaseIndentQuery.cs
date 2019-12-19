using System;

namespace RM.ServiceProvider.Model
{
    public class TPurchaseIndentQuery
    {
        /// <summary>
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///   订单号
        /// </summary>
        public string PurchaseBillCode { get; set; }

        /// <summary>
        ///   采购人
        /// </summary>
        public string PurchaseMan { get; set; }

        /// <summary>
        ///   采购日期
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        ///   供应商
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        ///   货品代码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        ///   货品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///   数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///   单价
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        ///   金额
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        ///   订单转入库
        /// </summary>
        public bool AuditFlag { get; set; }
    }
}