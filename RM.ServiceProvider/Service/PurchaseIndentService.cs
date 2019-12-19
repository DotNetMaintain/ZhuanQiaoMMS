using System;
using System.Collections.Generic;
using System.Transactions;
using RM.ServiceProvider.Dao;
using RM.ServiceProvider.Enum;
using RM.ServiceProvider.Interface;
using RM.ServiceProvider.Model;

namespace RM.ServiceProvider.Service
{
    public class PurchaseIndentService : IPurchaseIndent
    {
        private static IPurchaseIndent _Instance;

        private static readonly object _Lock = new object();

        #region Sington

        /// <summary>
        ///   返回类单一实例的方法
        /// </summary>
        public static IPurchaseIndent Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_Lock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new PurchaseIndentService();
                        }
                    }
                }

                return _Instance;
            }
        }

        #endregion

        private readonly PurchaseIndentDao dao;
        private readonly PurchaseDao daoStock;

        public PurchaseIndentService()
        {
            dao = new PurchaseIndentDao();
            daoStock = new PurchaseDao();
        }

        #region IPurchaseIndent 成员

        public List<MMS_PurchaseIndentContent> GetAllInfo()
        {
            return dao.GetAllInfo();
        }

        public int InsertInfo(MMS_PurchaseIndentContent info)
        {
            string msg = dao.ValidateRepeat(info);
            if (msg == "")
            {
                return dao.InsertInfo(info);
            }
            else
            {
                throw new Exception(msg);
            }
        }

        public bool UpdateInfo(MMS_PurchaseIndentContent info)
        {
            return dao.UpdateInfo(info);
        }

        public bool DeleteInfo(int id)
        {
            return dao.DeleteInfo(id);
        }

        public MMS_PurchaseIndentContent GetInfo(int id)
        {
            return dao.GetInfo(id);
        }

        #endregion

        #region IPurchaseIndent 成员

        public TPurchaseIndent GetPurchaseIndent(int id)
        {
            return dao.GetPurchaseIndent(id);
        }

        public int SavePurchaseIndent(TPurchaseIndent obj)
        {
            if (obj.Content.ID != null && obj.Content.ID > 0)
            {
                MMS_PurchaseIndentContent content = dao.GetInfo(obj.Content.ID);
                if (content.AuditFlag == true)
                {
                    throw new Exception("该单据已经审核" + content.PurchaseBillCode);
                }
            }
            return dao.SavePurchaseIndent(obj);
        }

        public bool AuditPurchaseIndent(int id, bool isAudit, string operatorCode, string warehouse)
        {
            using (TransactionScope ts = new TransactionScope()) //开启DTC事务
            {
                TPurchaseIndent tIndent = dao.GetPurchaseIndent(id);
                if (tIndent.Content.AuditFlag == true)
                {
                    throw new Exception("该订单已生成入库,不能重复入库");
                }
                TPurchase tStock = new TPurchase();
                tStock.OprType = OperateType.otInsert;
                tStock.Content = new MMS_PurchaseContent();
                tStock.Content.AuditFlag = false;
                tStock.Content.InvoiceCode = tIndent.Content.InvoiceCode;
                tStock.Content.InvoiceType = tIndent.Content.InvoiceType;
                tStock.Content.OperateDate = DateTime.Now;
                tStock.Content.Operator = operatorCode;
                tStock.Content.PayMode = tIndent.Content.PayMode;
                tStock.Content.Provider = tIndent.Content.Provider;
                tStock.Content.PurchaseBillCode = tIndent.Content.PurchaseBillCode; //订单号和入库单号相同
                tStock.Content.PurchaseDate = DateTime.Now;
                tStock.Content.CheckMan = tIndent.Content.PurchaseMan;
                tStock.Content.WareHouse = warehouse; //tIndent.Content.WareHouse;
                foreach (TPurchaseIndentDetail tIndentDetail in tIndent.Detail)
                {
                    TPurchaseDetail TDetail = new TPurchaseDetail();
                    TDetail.OprType = OperateType.otInsert;
                    TDetail.DetDetail = new MMS_PurchaseDetail();
                    TDetail.DetDetail.Memo = tIndentDetail.DetDetail.Memo;
                    TDetail.DetDetail.Price = tIndentDetail.DetDetail.Price;
                    TDetail.DetDetail.ProductCode = tIndentDetail.DetDetail.ProductCode;
                    TDetail.DetDetail.PurchaseBillCode = tStock.Content.PurchaseBillCode;
                    TDetail.DetDetail.Quantity = tIndentDetail.DetDetail.Quantity;
                    tStock.Detail.Add(TDetail);
                }
                daoStock.SavePurchase(tStock);

                MMS_PurchaseIndentContent indentContent = tIndent.Content;
                indentContent.AuditFlag = isAudit;
                dao.UpdateInfo(indentContent);
                ts.Complete();
            }
            return true;
        }

        #endregion
    }
}