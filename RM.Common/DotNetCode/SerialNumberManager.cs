using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.ServiceProvider.Dao;

namespace RM.Common.DotNetCode
{
    public class SerialNumberManager
    {

        /// <summary>
        ///   自动生成编号
        /// </summary>
        /// <param name="codeHead"> </param>
        /// <returns> </returns>
        public static string GeneralCode()
        {
            return BatchEvaluate.GeneralCode();
        }
    }
}
