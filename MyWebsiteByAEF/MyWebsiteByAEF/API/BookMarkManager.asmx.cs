using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using Common;

namespace MyWebsiteByAEF.API
{
    /// <summary>
    /// BookMarkManager 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://huawublog.v5.org")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class BookMarkManager : System.Web.Services.WebService
    {
        /// <summary>
        /// 得到所有书签
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetBookMarks()
        {
            try
            {
                List<BKList> bkList = AssistClass.DeserializeObject<List<BKList>>(AssistClass.HttpWebRequest2Json("http://devapi.saved.io/v1/bookmarks?token=1264cd317847ecc2e8d3b78e863e3e10"));
                StringBuilder sb = new StringBuilder();
                foreach (BKList item in bkList)
                {
                    sb.Append("<tr><td><a target='_blank' href='" + item.url + "'>" + item.title + "</a></td><td><button id='" + item.bk_id + "' onclick='DeleteBK(this)' type='button' class='btn btn-danger'>删除</button></td></tr>");
                }
                return sb.ToString();
            }

            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 批量添加书签：格式：
        /// [ABC][http://abc.com]
        /// [DEF][http://def.com]
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        [WebMethod]
        public bool AddBookMark(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt)) { return false; }
            //格式：名称-链接
            string[] Arr = txt.Split('\n');
            string[] IArr = null;
            string title, url;
            foreach (string item in Arr)
            {
                IArr = item.Split(new string[] { "][" }, StringSplitOptions.None);
                title = IArr[0].Replace("[", "").Replace("]", "");
                url = IArr[1].Replace("[", "").Replace("]", "");
                //如果书签存在则不添加
                List<string> bkList = AssistClass.DeserializeObject<List<BKList>>(AssistClass.HttpWebRequest2Json("http://devapi.saved.io/v1/bookmarks?token=1264cd317847ecc2e8d3b78e863e3e10")).Select(n => n.title).ToList();
                if (bkList.Contains(title))
                {
                    continue;
                }
                AddJsonData ajd = AssistClass.DeserializeObject<AddJsonData>(AssistClass.PostWebRequest("http://devapi.saved.io/v1/create", 
                    "token=1264cd317847ecc2e8d3b78e863e3e10&url=" + url + "&title=" + title,
                    Encoding.GetEncoding("GB2312")));
                if (!ajd.message.ToLower().Equals("success")) { return false; }
            }
            return true;
        }

        [WebMethod]
        public bool DeleteBookMark(int id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString())) { return false; }

            //{"is_error":false,"data":{"rows_deleted":1},"message":"Success"}
            AddJsonData ajd = AssistClass.DeserializeObject<AddJsonData>(AssistClass.PostWebRequest("http://devapi.saved.io/v1/delete", 
                "token=1264cd317847ecc2e8d3b78e863e3e10&bk_id=" + id, 
                Encoding.GetEncoding("GB2312")));
            if (!ajd.message.ToLower().Equals("success")) {return false; }
            return true;
        }
    }
    #region 书签工具类
    public class AddJsonData
    {
        public string is_error { get; set; }
        public dataClass data { get; set; }
        public string message { get; set; }
    }
    public class dataClass
    {
        public int id { get; set; }
        public string title { get; set; }
        public string list { get; set; }
        public string url { get; set; }
    }
    public class BKList
    {
        public string bk_id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string list { get; set; }
        public string list_name { get; set; }
        public string creation_date { get; set; }
    }
    #endregion
}
