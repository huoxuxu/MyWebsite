using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MyWebsiteByAEF.API
{
    /// <summary>
    /// GetServerInfo 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class GetServerInfo : System.Web.Services.WebService
    {
        /// <summary>
        /// 返回服务器位宽【32、64】
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public bool ServerBitWidth()
        {
            return Environment.Is64BitOperatingSystem;
        }

        /// <summary>
        /// 返回网站根目录绝对路径
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string WebsiteAbsolutelyPath()
        {
            return Server.MapPath("~/");
        }
    }
}
