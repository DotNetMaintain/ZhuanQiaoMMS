using System.Collections.Generic;
using System.Linq;
using RM.ServiceProvider.Model;
using System;
using System.Data.Common;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;

namespace RM.ServiceProvider.Dao
{
    /// <summary>
    ///   货品信息数据访问类
    /// </summary>
    internal class MaterialInfoDao
    {
        private readonly RMDataContext dc;
        
        /// <summary>
        ///   构造函数，创建LINQ to SQL数据上下文类的实例
        /// </summary>
        public MaterialInfoDao()
        {
            dc = new RMDataContext(ConnectionManager.ConnectionString);
        }

        /// <summary>
        ///   获得所有的货品信息列表
        /// </summary>
        /// <returns> 货品信息列表 </returns>
        public List<MMS_MaterialInfo> GetAllInfo()
        {
            List<MMS_MaterialInfo> lst = new List<MMS_MaterialInfo> ();
            using (RMDataContext dc2 = new RMDataContext(ConnectionManager.ConnectionString)) //创建一个新的上下文
            {
                lst = dc2.MMS_MaterialInfo.ToList();
                //lst = dc2.MMS_MaterialInfo.Select(itm => itm).ToList();
            }
          
             return lst;
        }

        /// <summary>
        ///   插入货品信息
        /// </summary>
        /// <param name="info"> 货品信息实体 </param>
        /// <returns> 返回插入记录的自增主键值 </returns>
        public int InsertInfo(MMS_MaterialInfo info)
        {
            dc.MMS_MaterialInfo.InsertOnSubmit(info);
            dc.SubmitChanges();
            return info.Material_ID;
        }

        /// <summary>
        ///   更新货品信息
        /// </summary>
        /// <param name="info"> 货品信息实体 </param>
        /// <returns> 更新成功返回true </returns>
        public bool UpdateInfo(MMS_MaterialInfo info)
        {
            var query = from item in dc.MMS_MaterialInfo
                        where item.Material_ID == info.Material_ID
                        select item;

            BatchEvaluate.Eval(info, query.First());
            dc.SubmitChanges();
            return true;
        }

        /// <summary>
        ///   删除货品信息
        /// </summary>
        /// <param name="id"> 货品信息主键 </param>
        /// <returns> 删除成功返回true </returns>
        public bool DeleteInfo(int id)
        {
            var query = from item in dc.MMS_MaterialInfo
                        where item.Material_ID  == id
                        select item;
            if (query.Count() > 0)
            {
                dc.MMS_MaterialInfo.DeleteOnSubmit(query.First());
                dc.SubmitChanges();
            }
            return true;
        }

        /// <summary>
        ///   根据主键返回货品信息实体
        /// </summary>
        /// <param name="id"> 货品信息主键 </param>
        /// <returns> 货品信息实体 </returns>
        public MMS_MaterialInfo GetInfo(int id)
        {
            return dc.MMS_MaterialInfo.Where(itm => itm.Material_ID == id).FirstOrDefault();
        }

        /// <summary>
        ///   验证记录中是否有重复值
        /// </summary>
        /// <param name="info"> 实体 </param>
        /// <returns> 如果不重复返回"" </returns>
        public string ValidateRepeat(MMS_MaterialInfo info)
        {
            //查询货品信息表中货品代码与新添加或修改的货品代码相同且主键不同(新添加的货品ID为空)的记录数
            int cnt1 = dc.MMS_MaterialInfo.Where(itm => itm.Material_Name == info.Material_Name && itm.Material_ID != info.Material_ID).Count();
            if (cnt1 > 0) //如果记录数大于0表示货品代码重复
            {
                return "代码重复";
            }
            //查询货品信息表中货品简称与新添加或修改的货品简称相同且主键不同的记录数
            int cnt2 = dc.MMS_MaterialInfo.Where(itm => itm.Material_CommonlyName == info.Material_CommonlyName && itm.Material_ID != info.Material_ID).Count();
            if (cnt2 > 0) //如果记录数大于0表示货品简称重复
            {
                return "简称重复";
            }
            return "";
        }

        /// <summary>
        ///   根据货品代码获得货品信息实体
        /// </summary>
        /// <param name="productCode"> 货品代码 </param>
        /// <returns> 货品信息实体 </returns>
        public MMS_MaterialInfo GetProductInfoByCode(string MaterialCode)
        {
            if (string.IsNullOrEmpty(MaterialCode))
            {
                return dc.MMS_MaterialInfo.FirstOrDefault();
            }
            else
            {
                return dc.MMS_MaterialInfo.Where(itm => itm.Material_ID == Convert.ToInt32(MaterialCode)).FirstOrDefault();
            }
        }

        /// <summary>
        ///   根据货品代码获得货品信息实体
        /// </summary>
        /// <param name="productCode"> 货品代码 </param>
        /// <returns> 货品信息实体 </returns>
        public string GettypeById(System.Nullable<int> id)
        {
            string MaterialType_Name = dc.MMS_MaterialType.Where(itm=>itm.MaterialType_ID==id).FirstOrDefault().MaterialType_Name;
            return MaterialType_Name;
        }
        /// <summary>
        ///   根据货品助记码获得所有货品信息当前页的信息
        /// </summary>
        /// <param name="helpCode"> 货品助记码 </param>
        /// <param name="CurrentPageIndex"> 当前页索引 </param>
        /// <param name="PageSize"> 页尺寸 </param>
        /// <returns> 当前页信息 </returns>
        public List<MMS_MaterialInfo> GetAllInfo(string MaterialCommonlyName, int CurrentPageIndex, int PageSize)
        {


            string userremark = RequestSession.GetSessionUser().UserRemark.ToString();
            List<MMS_MaterialInfo> lst = new List<MMS_MaterialInfo>();
            using (RMDataContext dc2 = new RMDataContext(ConnectionManager.ConnectionString)) //创建一个新的上下文
            {


                if (CurrentPageIndex > 0) //如果当前页索引大于0
                {
                    if (string.IsNullOrEmpty(MaterialCommonlyName)) //货品信息助记码为空
                    {
                        if (string.IsNullOrEmpty(userremark))
                        {
                            lst = dc2.MMS_MaterialInfo.Where(itm => itm.DeleteMark == 1).Skip((CurrentPageIndex - 1) * PageSize).Take(PageSize).ToList();

                        }
                        else
                        {
                            lst = dc2.MMS_MaterialInfo.Where(itm => itm.DeleteMark == 1 && itm.Material_Attr01 == userremark).Skip((CurrentPageIndex - 1) * PageSize).Take(PageSize).ToList();

                        }
                        return lst;

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(userremark))
                        {

                            lst = dc2.MMS_MaterialInfo.Where(itm => itm.Material_CommonlyName.Contains(MaterialCommonlyName) && itm.DeleteMark == 1).Skip((CurrentPageIndex - 1) *
                                                                                              PageSize).Take(PageSize).
                                ToList();
                        }
                        else
                        {

                            lst = dc2.MMS_MaterialInfo.Where(itm => itm.Material_CommonlyName.Contains(MaterialCommonlyName) && itm.DeleteMark == 1 && itm.Material_Attr01 == userremark).Skip((CurrentPageIndex - 1) *
                                                                                              PageSize).Take(PageSize).
                                ToList();
                        }
                        return lst;

                    }
                }
                else
                {
                    int cnt = 0;
                    if (string.IsNullOrEmpty(MaterialCommonlyName)) //货品信息助记码为空
                    {
                        if (string.IsNullOrEmpty(userremark))
                        {
                            cnt = dc2.MMS_MaterialInfo.Select(itm => itm).Count(); //统计所有货品信息的记录数
                        }
                        else
                        {
                            cnt = dc2.MMS_MaterialInfo.Select(itm => itm.Material_Attr01 == userremark).Count(); //统计所有货品信息的记录数
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(userremark))
                        {
                            cnt = dc2.MMS_MaterialInfo.Where(itm => itm.Material_CommonlyName.Contains(MaterialCommonlyName) && itm.DeleteMark == 1).Count();
                        }
                        else
                        {
                            //统计指定助记码货品信息的记录数
                            cnt = dc2.MMS_MaterialInfo.Where(itm => itm.Material_CommonlyName.Contains(MaterialCommonlyName) && itm.DeleteMark == 1 && itm.Material_Attr01 == userremark).Count();
                        }
                    }
                    //返回货品信息列表(只有一行且ID成员存放统计的记录数)
                    return new List<MMS_MaterialInfo> { new MMS_MaterialInfo { Material_ID = cnt } };
                }
            }
        }
    }
}
