﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using RM.ServiceProvider.Dao;
using RM.ServiceProvider.Enum;
using RM.ServiceProvider.Interface;
using RM.ServiceProvider.Model;

namespace RM.ServiceProvider.Service
{
    public class PurchasePlanService : IPurchasePlan
    {
        private static IPurchasePlan _Instance;

        private static readonly object _Lock = new object();

        #region Sington

        /// <summary>
        ///   返回类单一实例的方法
        /// </summary>
        public static IPurchasePlan Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_Lock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new PurchasePlanService();
                        }
                    }
                }

                return _Instance;
            }
        }

        #endregion

        private readonly PurchasePlanDao dao;
        private readonly PurchaseIndentDao daoIndent;
        private readonly PurchaseDao daoPur;

        public PurchasePlanService()
        {
            dao = new PurchasePlanDao();
            daoIndent = new PurchaseIndentDao();
            daoPur = new PurchaseDao();
        }

        #region IPurchasePlan 成员

        /// <summary>
        ///   获得所有采购计划单列表
        /// </summary>
        /// <returns> </returns>
        public List<MMS_PurchasePlanContent> GetAllInfo()
        {
            return dao.GetAllInfo();
        }

        public MMS_PurchasePlanDetail GetInfoDetail(int id)
        {
            return dao.GetInfoDetail(id);
        }



        /// <summary>
        ///   插入采购计划单
        /// </summary>
        /// <param name="info"> </param>
        /// <returns> </returns>
        public int InsertInfo(MMS_PurchasePlanContent info)
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

        /// <summary>
        ///   修改采购计划单
        /// </summary>
        /// <param name="info"> </param>
        /// <returns> </returns>
        public bool UpdateInfo(MMS_PurchasePlanContent info)
        {
            return dao.UpdateInfo(info);
        }


        public bool UpdateInfoDetail(MMS_PurchasePlanDetail info)
        {
            return dao.UpdateInfoDetail(info);
        }

        /// <summary>
        ///   删除采购计划单
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public bool DeleteInfo(int id)
        {
            return dao.DeleteInfo(id);
        }

        /// <summary>
        ///   根据主键值获取采购计划单实体
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public MMS_PurchasePlanContent GetInfo(int id)
        {
            return dao.GetInfo(id);
        }

        #endregion



        public MMS_PurchasePlanContent GetPurchasePlanCode(string purchasebillcode)
        {
            return dao.GetPurchasePlanCode(purchasebillcode);
        }


        #region IPurchasePlan Members

        /// <summary>
        ///   自动生成采购计划
        /// </summary>
        /// <param name="oprCode"> </param>
        /// <returns> </returns>
        public TPurchasePlan GeneralPurchasePlan(string oprCode)
        {
            TPurchasePlan tplan = new TPurchasePlan(); //创建采购计划单
            //设置采购计划主信息操作类型为插入以在调用数据访问层做添加处理
            tplan.OprType = OperateType.otInsert;
            tplan.Content = new MMS_PurchasePlanContent(); //创建采购计划单主信息
            //生成采购计划单号
            tplan.Content.PurchaseBillCode = BatchEvaluate.GeneralCode();
            tplan.Content.PurchaseDate = DateTime.Now; //设置采购日期
            tplan.Content.Provider = ""; //采购供应商
            tplan.Content.PurchaseMan = ""; //采购经办人
            tplan.Content.AuditFlag = false; //采购计划确认标志
            //取允许采购的货品信息列表
            List<MMS_ProductInfo> productList =
                ProductInfoService.Instance.GetAllInfo().Where(itm => itm.IsStop == null || itm.IsStop == false).ToList();
            //取所有库存信息列表
            List<MMS_Store> storeList = StoreService.Instance.GetAllInfo();
            //用LINQ将库存信息按货品代码进行分组汇总
            var query = from store in storeList
                        group store by store.ProductCode
                        into g
                        orderby g.Key
                        select new
                            {
                                ProductCode = g.Key,
                                Quantity = g.Sum(itm => itm.Quantity)
                            };
            //循环遍历货品信息列表
            foreach (var product in productList)
            {
                int qty = 0;
                if (query.Count() != 0) //取该货品的库存汇总数量
                {
                    var store = query.FirstOrDefault(itm => itm.ProductCode == product.ProductCode);
                    if (store != null)
                    {
                        qty = store.Quantity;
                    }
                }
                if (qty < product.MinStore) //如果该货品的库存汇总数量小于最低库存
                {
                    //创建采购计划货品实例
                    TPurchasePlanDetail tDetail = new TPurchasePlanDetail();
                    //设置为插入操作以便数据访问层处理
                    tDetail.OprType = OperateType.otInsert;
                    tDetail.DetDetail = new MMS_PurchasePlanDetail();
                    tDetail.DetDetail.PurchaseBillCode = tplan.Content.PurchaseBillCode; //计划单号
                    tDetail.DetDetail.ProductCode = product.ProductCode; //货品代码
                    tDetail.DetDetail.Quantity = (product.MinStore ?? 0) - qty; //货品数量
                    //调用入库单服务类的GetLastPurchasePrice方法取货品单价
                    tDetail.DetDetail.Price = daoPur.GetLastPurchasePrice(tplan.Content.Provider, product.ProductCode);
                    tDetail.DetDetail.Memo = ""; //注备
                    tplan.Detail.Add(tDetail); //将采购计划货品实体添加到采购计划单中
                }
            }
            int cnt = dao.SavePurchasePlan(tplan); //调用数据访问层的保存采购计划单方法
            tplan.Content.ID = cnt; //返回新生成计划单的主键值
            return tplan;
        }

        /// <summary>
        ///   获得采购计划实体
        /// </summary>
        /// <param name="id"> 计划单据号 </param>
        /// <returns> 自定义采购计划实体 </returns>
        public TPurchasePlan GetPurchasePlan(int id)
        {
            return dao.GetPurchasePlan(id);
        }

        /// <summary>
        ///   保存采购计划
        /// </summary>
        /// <param name="obj"> 自定义采购计划实体 </param>
        /// <returns> 计划主表ID号 </returns>
        public int SavePurchasePlan(TPurchasePlan obj)
        {
            if (obj.Content.ID != null && obj.Content.ID > 0)
            {
                MMS_PurchasePlanContent content = dao.GetInfo(obj.Content.ID);
                if (content.AuditFlag == true)
                {
                    throw new Exception("该单据已经审核" + content.PurchaseBillCode);
                }
            }
            return dao.SavePurchasePlan(obj);
        }

        /// <summary>
        ///   审核采购计划
        /// </summary>
        /// <param name="id"> </param>
        /// <param name="isAudit"> </param>
        /// <returns> </returns>
        public bool AuditPurchasePlan(int id, bool isAudit, string operatorCode)
        {
            try {

                //using (TransactionScope ts = new TransactionScope()) //开启DTC事务
                //{
                    TPurchasePlan TPlan = dao.GetPurchasePlan(id); //调用数据访问层方法取采购计划单
                    if (TPlan.Content.AuditFlag == true)
                    {
                        throw new Exception("该计划已生成订单,不能重复生成订单");
                    }
                    TPlan.Content.AuditFlag = true;
                    dao.UpdateInfo(TPlan.Content);
                    //TPurchaseIndent TIndent = new TPurchaseIndent(); //创建采购订单实例
                    //TIndent.OprType = OperateType.otInsert; //设置操作类型
                    ////创建采购订单主信息
                    //TIndent.Content = new MMS_PurchaseIndentContent();
                    ////以下是将采购计划主信息赋值给采购订单主信息
                    //TIndent.Content.AuditFlag = false;
                    //TIndent.Content.InvoiceCode = TPlan.Content.InvoiceCode;
                    //TIndent.Content.InvoiceType = TPlan.Content.InvoiceType;
                    //TIndent.Content.OperateDate = DateTime.Now;
                    //TIndent.Content.Operator = operatorCode;
                    //TIndent.Content.PayMode = TPlan.Content.PayMode;
                    //TIndent.Content.Provider = TPlan.Content.Provider;
                    //TIndent.Content.PurchaseBillCode = TPlan.Content.PurchaseBillCode; //计划单号和订单号相同
                    //TIndent.Content.PurchaseDate = DateTime.Now;
                    //TIndent.Content.PurchaseMan = TPlan.Content.PurchaseMan;
                    //TIndent.Content.DeptName = TPlan.Content.DeptName;
                    ////遍历采购计划货品将其赋值给采购订单
                    //foreach (TPurchasePlanDetail TPlanDetail in TPlan.Detail)
                    //{
                    //    TPurchaseIndentDetail TIndentDetail = new TPurchaseIndentDetail();
                    //    TIndentDetail.OprType = OperateType.otInsert;
                    //    TIndentDetail.DetDetail = new MMS_PurchaseIndentDetail();
                    //    TIndentDetail.DetDetail.Memo = TPlanDetail.DetDetail.Memo;
                    //    TIndentDetail.DetDetail.Price = TPlanDetail.DetDetail.Price;
                    //    TIndentDetail.DetDetail.ProductCode = TPlanDetail.DetDetail.ProductCode;
                    //    TIndentDetail.DetDetail.PurchaseBillCode = TIndent.Content.PurchaseBillCode;
                    //    TIndentDetail.DetDetail.Quantity = TPlanDetail.DetDetail.Quantity;
                    //    TIndent.Detail.Add(TIndentDetail);
                    //}
                    //daoIndent.SavePurchaseIndent(TIndent); //调用采购订单服务类的保存采购订单方法

                    //MMS_PurchasePlanContent planContent = TPlan.Content;
                    //planContent.AuditFlag = isAudit;
                    //dao.UpdateInfo(planContent); //更新采购计划单为采购已确认
                //    ts.Complete(); //提交事务
                //}
            
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            return true;
        }

        #endregion
    }
}