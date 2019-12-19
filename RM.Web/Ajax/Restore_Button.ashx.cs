﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using RM.ServiceProvider;
using RM.ServiceProvider.Dao;
using RM.ServiceProvider.Service;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Busines;
using System.Text;

namespace RM.Web.Ajax
{
    /// <summary>
    /// Delivery_Button 的摘要说明
    /// </summary>
    public class Restore_Button : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1.0);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";

            var text = context.Request["data"];

            try
            {

                if (text != null)
                {
                    string[] str_data = text.Split(',');
                    foreach (string str in str_data)
                    {

                        string str_replace = str.Replace('\"', ' ');
                        str_replace = str_replace.Replace('[', ' ');
                        str_replace = str_replace.Replace(']', ' ');
                        str_replace = str_replace.Trim();
                        DateTime datetime = (DateTime)DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("select getdate()"));

                        //领料单明细详情
                        MMS_PurchasePlanDetail purPlanDetail = PurchasePlanService.Instance.GetInfoDetail(Convert.ToInt32(str_replace));

                        //首先查看是否已经发料


                        List<MMS_Delivery_Detail> dlist = DeliveryInfoService.Instance.GetReturnDeliveryInfo(purPlanDetail.PurchaseBillCode, purPlanDetail.ProductCode, purPlanDetail.Price);
                        //List<MMS_Delivery_Detail> dlist= deliver.Where(item => item.AuditFlag != 1) as List<MMS_Delivery_Detail>;

                        //没有发料直接执行退料将领料表中的标识修改掉

                        if (dlist == null || dlist.Count == 0)
                        {

                            purPlanDetail.AuditFlag = "0";  //退货处理
                            purPlanDetail.OperatorDate = datetime;
                            purPlanDetail.CheckQuantity = 0;
                            PurchasePlanService.Instance.UpdateInfoDetail(purPlanDetail);
                            foreach (MMS_Delivery_Detail dd in dlist)
                            {
                                if (dd.AuditFlag != 1)
                                {
                                    //将发料表中的标识修改掉
                                    dd.AuditFlag = 1;  //退货处理
                                    DeliveryInfoService.Instance.UpdateInfo(dd);
                                }
                            }

                        }
                        else
                        {

                            //已经发料的状况下看是否已经当月结算，如何已经结算则不允许退料
                            //没有结算执行退料
                            //将领料表中的标识修改掉
                            purPlanDetail.AuditFlag = "0";  //退货处理
                            purPlanDetail.OperatorDate = datetime;
                            purPlanDetail.CheckQuantity = 0;
                            //purPlanDetail.
                            PurchasePlanService.Instance.UpdateInfoDetail(purPlanDetail);

                            foreach (MMS_Delivery_Detail dd in dlist)
                            {
                                if (dd.AuditFlag != 1)
                                {
                                    //将发料表中的标识修改掉
                                    dd.AuditFlag = 1;  //退货处理
                                    DeliveryInfoService.Instance.UpdateInfo(dd);

                                    //将入库表中的已用数量扣掉
                                    MMS_PurchaseDetail purDetail = PurchaseService.Instance.GetInfoDetail(Convert.ToInt32(dd.Memo));
                                    if (purDetail != null)
                                    {
                                        purDetail.UseQuantity = Convert.ToInt32(purDetail.UseQuantity) - Convert.ToInt32(dd.Quantity);
                                        PurchaseService.Instance.UpdateInfoDetail(purDetail);
                                    }
                                }
                            }



                        }
                    }


                }


                else
                {

                    context.Session.Abandon();
                    context.Session.Clear();
                    context.Response.Write(1);
                    context.Response.End();

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }





        }





        public static string GetJSON<T>(object obj)
        {
            string result = String.Empty;
            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    serializer.WriteObject(ms, obj);
                    result = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}