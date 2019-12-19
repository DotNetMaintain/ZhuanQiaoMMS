using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.DAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using RM.Common.DotNetEncrypt;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace RM.Web.RMBase.SysBaseInfo
{
    public partial class MaterialStorage_Form : PageBase
    {
        private string _key;
        private string _price;
        private MMS_MaterialInfo_Dal material_idao = new MMS_MaterialInfo_Dal();

        protected void Page_Load(object sender, EventArgs e)
        {
            this._key = base.Request["key"];
            this._price = base.Request["price"].ToString().Trim();
            DataBindGrid();

        }

        private void DataBindGrid()
        {
           
            int count = 0;
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            if (!string.IsNullOrEmpty(_key))
            {
                SqlWhere.Append(" and Material_ID" + " = @obj  and price=" + "@objprice");
                IList_param.Add(new SqlParam("@obj", this._key));
                IList_param.Add(new SqlParam("@objprice", Convert.ToDouble(this._price)));
            }




            DataTable dt = this.material_idao.GetStorageMaterialInfo(SqlWhere, IList_param);
            Hashtable ht = GetCustHashtableById(dt);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
              
            }
            
           
            
        }




        public Hashtable GetCustHashtableById(DataTable dt)
        {
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string key = dt.Columns[i].ColumnName;
                    ht[key.ToUpper()] = dr[key];
                }
            }
            return ht;
        }

        protected void Save_Click(object sender, EventArgs e)
        {
           
        }
    }
}