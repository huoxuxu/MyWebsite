using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using EncryptManager;

namespace MyWebsiteByAEF.API
{
    /// <summary>
    /// De_EncryptManager 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class De_EncryptManager : System.Web.Services.WebService
    {
        /// <summary>
        /// 加密-密钥包含在加密后的字符串内
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns></returns>
        [WebMethod]
        public string Encrypt(string str)
        {
            AESEncryptHelper aes = new AESEncryptHelper();
            return aes.Encrypt(str);
        }

        /// <summary>
        /// 解密，解密密钥包含在加密后的字符串内
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <returns></returns>
        [WebMethod]
        public string Decrypt(string str)
        {
            AESEncryptHelper aes = new AESEncryptHelper();
            return aes.Decrypt(str);
        }
    }
}
