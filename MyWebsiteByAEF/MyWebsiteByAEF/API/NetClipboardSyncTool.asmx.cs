using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using CacheManager;
using MyWebsiteByAEF.Resource.Res;

namespace MyWebsiteByAEF.API
{
    /// <summary>
    /// NetClipboardSyncTool 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class NetClipboardSyncTool : System.Web.Services.WebService
    {
        /// <summary>
        /// 给剪切板添加文本
        /// 由于使用缓存技术，剪切板的内容可能会丢失
        /// </summary>
        /// <param name="txt">为空则返回缓存中的内容，不为空则替换缓存内容</param>
        /// <returns></returns>
        [WebMethod]
        public object AddNetClipboard(string txt)
        {
            try
            {
                CacheHelper ch = new CacheHelper();
                object obj = ch.GetCache(MyRes.剪切板);
                if (!string.IsNullOrWhiteSpace(txt))
                {
                    if (obj == null || txt != obj.ToString())
                    {
                        ch.CreateCache(MyRes.剪切板, txt);
                        obj = txt;
                    }
                }
                return obj;
            }
            catch { return ""; }
        }

        /// <summary>
        /// 清空剪切板
        /// </summary>
        [WebMethod]
        public void ClearNetClipboard()
        {
            CacheHelper ch = new CacheHelper();
            ch.DeleteCache(MyRes.剪切板);
        }

    }
}
