using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SqlConnectionStringMagager;

namespace MyWebsiteByAEF.API
{
    /// <summary>
    /// ConnectionStringManager 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://huawublog.v5.org")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class ConnectionStringManager : System.Web.Services.WebService
    {
        /// <summary>
        /// 根据参数返回数据库连接字符串
        /// </summary>
        /// <param name="scse">数据库类型</param>
        /// <param name="DataBaseName">数据库名称</param>
        /// <param name="Server">服务器名称</param>
        /// <param name="DBPort">数据库端口</param>
        /// <param name="DBUserName">数据库用户名</param>
        /// <param name="DBUserPwd">数据库用户密码</param>
        /// <param name="Charset">数据库字符集</param>
        /// <returns></returns>
        [WebMethod]
        public Dictionary<string, string> GetConnectionString(SqlConnectionStringEnum scse, string DataBaseName, string Server = "", string DBPort = "", string DBUserName = "", string DBUserPwd = "", string Charset = "")
        {
            return GetConnectionString(scse,DataBaseName,Server,DBPort,DBUserName,DBUserPwd,Charset);
        }

        [WebMethod]
        public string GetProviderName(SqlConnectionStringEnum scse)
        {
            return GetProviderName(scse);
        }

    }
}
