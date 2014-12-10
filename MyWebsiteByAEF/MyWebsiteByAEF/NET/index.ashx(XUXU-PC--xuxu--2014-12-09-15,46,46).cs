using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;
using System.IO;
using Shell32;
using System.Net;
using Common;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using CacheManager;
using MyWebsiteByAEF.API.ForDB;
using MyWebsiteByAEF.DBModel;
using Newtonsoft.Json.Linq;
using MyWebsiteByAEF.API;
using JsonNETManager;

namespace Website.NET
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler
    {
        string errorMsg = "{\"success\":false,\"info\":\"程序内部异常！\"}";
        public void ProcessRequest(HttpContext context)
        {
            String methodName = context.Request["method"];
            if (String.IsNullOrEmpty(methodName)) return;
            Type type = this.GetType();
            MethodInfo method = type.GetMethod(methodName);
            if (method != null)
            {
                method.Invoke(this, new object[] { context });
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void GetContent(HttpContext hc)
        {
            /*默认查询方法
             根据页码返回指定值
             * 返回：
             * 方法状态，是否正常
             * 值:总个数，根据页码和数量的数据
             */
            if (!AssistClass.CheckFormParams(hc.Request.Form))
            {
                hc.Response.Write(errorMsg);
                return;
            }
            try
            { 
            int pageNumber = Convert.ToInt32(hc.Request["pageNumber"]);//页码
            int pageSize = Convert.ToInt32(hc.Request["pageSize"]); //每页总数据量
            string order = hc.Request["order"];

            //暂时由3种:default,time,hit
            QuestionAndAnswerTable qaat = new QuestionAndAnswerTable();
            questionandanswer[] qaaArr = null;
            IOrderedQueryable<questionandanswer> q = null;
            switch (order)
            {
                case"time":
                    q = qaat.SelectManyWithQueryable().OrderByDescending(d => d.CreateTime);
                    break;
                case "hit":
                    q = qaat.SelectManyWithQueryable().OrderByDescending(d => d.Hits);
                    break;
                default:
                    q = qaat.SelectManyWithQueryable().OrderByDescending(d => d.CreateTime);
                    break;
            }
                //分页
                qaaArr=q.Skip(AssistClass.FYSkip(pageNumber, pageSize)).Take(pageSize).ToArray();
                hc.Response.Write("{\"success\":true,\"info\":\"\",\"total\":" + qaat.SelectManyWithQueryable().Count() + ",\"rows\":" + JsonNetHelper.SerializeObject(qaaArr) + "}");
            }
            catch {
                hc.Response.Write(errorMsg);
            }
        }

        public void GetContentXq(HttpContext hc)
        {
            if (!AssistClass.CheckFormParams(hc.Request.Form))
            {
                hc.Response.Write(errorMsg);
                return;
            }
            var id=hc.Request["id"];
            QuestionAndAnswerTable qaat = new QuestionAndAnswerTable();
            var list=qaat.Select(id);
            hc.Response.Write("{\"success\":true,\"info\":\"\",\"total\":1,\"rows\":"+JsonNetHelper.SerializeObject(list)+"}");

            //string id = hc.Request["id"].Trim();
            //if (string.IsNullOrWhiteSpace(id)) { return; }
            //try
            //{
            //    QuestionAndAnswerTable idm = new QuestionAndAnswerTable();
            //    questionandanswer qaa = idm.Select(id);
            //    qaa.Hits += 1;
            //    idm.Update(qaa);
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("<div><h3 class='contentTile'>" + qaa.Question + "</h3><p class='content'>" + qaa.Answer + "</p></div>");
            //    hc.Response.Write(sb.ToString());
            //}
            //catch { hc.Response.Write("error"); }
        }

        public void add(HttpContext hc)
        {
            string title = hc.Request["title"].Trim();
            string content = hc.Request["content"].Trim();//.Replace("\n", "<br/>");
            string keyword = hc.Request["keyword"].Trim();
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content)) { return; }
            try
            {
                QuestionAndAnswerTable idm = new QuestionAndAnswerTable();
                questionandanswer qaa = new questionandanswer();
                qaa.Id = Guid.NewGuid().ToString();
                qaa.Question = title;
                qaa.Answer = content;
                qaa.Keyword = keyword;
                qaa.Hits = 0;
                qaa.CreateTime = DateTime.Now;
                idm.Add(qaa);
                hc.Response.Write("xx1");
            }
            catch { hc.Response.Write("error"); }

        }

        public void GetSearch(HttpContext hc)
        {
            //关键字，会在【标题】【关键字】【正文】里搜索，排序按照点击率，或者标题、关键字、正文关联度
            string keyword = hc.Request["key"].Trim();
            if (string.IsNullOrWhiteSpace(keyword)) { return; }
            string[] key = null;
            keyword = keyword.Replace('，', ',').Replace(' ', ',').Replace('-', ',').Replace('|', ',');
            key = keyword.Split(',');
            try
            {
                    StringBuilder sb = new StringBuilder();
                using (MyDBEntities db = new MyWebsiteByAEF.DBModel.MyDBEntities())
                {
                    var q = (from a in db.questionandanswer
                             where a.Question.Contains(key[0])
                             || a.Answer.Contains(key[0])
                             || a.Keyword.Contains(key[0])
                             select a).ToList();
                    foreach (var item in q)
                    {
                        sb.Append("<div>" + @"
                        <a class='contentTile' target='_blank' href='HTML/ShowContent.htm?id=" +
                            item.Id + "'>" + (item.Question.ToString().Length > 25 ? item.Question.ToString().Substring(0, 25) + "..."
                            : item.Question) + "</a>" + 
                            "<div class='content'>" + AssistClass.ReplaceHtmlTag(item.Answer.ToString(), 236).Replace(key[0],
                                               "<b style='color:red'>"+key[0]+"</b>") + "</div>" + 
                                               "<div class='row'><div class='col-md-6'><small><b>关键字：</b>" + item.Keyword +
                             "</small></div><div class='col-md-3'><small><b>更新于：</b>" + item.CreateTime +
                             "</small></div></div><hr /></div>");
                    }
                }
                hc.Response.Write(sb.ToString());
            }
            catch
            {
                hc.Response.Write("error");
            }
        }

        public void GetWeather(HttpContext hc)
        {
            try
            {
                ////根据网络得到地区，
                //string url = "http://api.map.baidu.com/location/ip?ak=7729f96c58346633dad40b4f1e62746b&coor=bd09ll";
                //url = AssistClass.HttpWebRequest2Json(url);
                ////改成json.net的取值方法
                //JObject rss = JObject.Parse(url);
                //url = rss["content"]["address"].ToString();

                //根据地区查询天气
                string url = "上海";
                JObject rss = null;
                url = "http://api.map.baidu.com/telematics/v3/weather?location=" + url + "&output=json&ak=7729f96c58346633dad40b4f1e62746b";
                url = AssistClass.HttpWebRequest2Json(url);

                rss = JObject.Parse(url);
                url = "<b>" + rss["results"][0]["currentCity"].ToString() + "</b>：" + rss["results"][0]["weather_data"][0]["date"].ToString() + "<br/>" + rss["results"][0]["weather_data"][0]["weather"].ToString() + "&nbsp;" + rss["results"][0]["weather_data"][0]["wind"].ToString() + "<br/>温度：" + rss["results"][0]["weather_data"][0]["temperature"].ToString();
                hc.Response.Write(url);
            }
            catch (Exception ex){ 
                hc.Response.Write("天气加载失败！");
                WriteErrorLog(ex);
            }
        }

        public void GetMusicList(HttpContext hc)
        {
            //读取指定目录下的文件
            string mp3Path = "Resource/MP3/";
            string ret = "";
            //得到文件基本信息
            ShellClass sc = new ShellClass();
            foreach (string fileName in Directory.GetFiles(hc.Request.PhysicalApplicationPath+mp3Path))
            {
                Folder dir = sc.NameSpace(Path.GetDirectoryName(fileName));
                FolderItem item = dir.ParseName(Path.GetFileName(fileName));
                string str = dir.GetDetailsOf(item, 27);
                ret += "<li data-artist='{0}' data-title='{1}' data-album='{2}' data-info='' data-image='' data-duration='0'><div class='amazingaudioplayer-source' data-src='{3}' data-type='audio/mpeg' /></li>";
                ret = string.Format(ret, dir.GetDetailsOf(item, 13), string.IsNullOrWhiteSpace(dir.GetDetailsOf(item, 21))?"未知名称":dir.GetDetailsOf(item, 21), dir.GetDetailsOf(item, 14), mp3Path+fileName.Substring(fileName.LastIndexOf("/") + 1));
            }
            //组装html返回
            hc.Response.Write(ret);
        }

        #region 翻译

        public void getJson(HttpContext hc)
        {
            try
            {
                string para = hc.Request["from1"].Replace(" ", "%20");
                string url = "http://openapi.baidu.com/public/2.0/bmt/translate?client_id=Zxf03CQXtslHT1TfptaGBWOn&q='" + para + "'&from=auto&to=auto";
                url = AssistClass.HttpWebRequest2Json(url);
                para = "";
                //解析json
                JObject rss = JObject.Parse(url);
                var ur = from p in rss["trans_result"]
                         select (string)p["dst"];
                foreach (var item in ur)
                {
                    para += item + "\n";
                }
                para = para.Remove(para.Length - 1);
                hc.Response.Write(para);
            }
            catch { hc.Response.Write("翻译失败！"); }
        }
        #endregion
        #region 书签工具
        public void GetBookMark(HttpContext hc)
        {
            BookMarkManager bm = new BookMarkManager();
                hc.Response.Write(bm.GetBookMarks());
        }

        public void AddBookMark(HttpContext hc)
        {
            string txt = hc.Request["txt"].Trim();
            if (string.IsNullOrWhiteSpace(txt)) { return; }
            //格式：名称-链接
            string[] Arr = txt.Split('\n');
            string[] IArr = null;
            string title,url;
            foreach (string item in Arr)
            {
                IArr = item.Split(new string[] { "][" }, StringSplitOptions.None);
                title=IArr[0].Replace("[", "").Replace("]", "");
                url = IArr[1].Replace("[", "").Replace("]", "");
                //如果书签存在则不添加
                List<string> bkList = AssistClass.DeserializeObject<List<BKList>>(AssistClass.HttpWebRequest2Json("http://devapi.saved.io/v1/bookmarks?token=1264cd317847ecc2e8d3b78e863e3e10")).Select(n => n.title).ToList();
                if (bkList.Contains(title))
                {
                    continue;
                }

                AddJsonData ajd = AssistClass.DeserializeObject<AddJsonData>(AssistClass.PostWebRequest("http://devapi.saved.io/v1/create", "token=1264cd317847ecc2e8d3b78e863e3e10&url=" + url + "&title=" + title, Encoding.GetEncoding("GB2312")));
                if (!ajd.message.ToLower().Equals("success")) { hc.Response.Write("error"); return; }
            }
            hc.Response.Write("success");
        }

        public void DeleteBookMark(HttpContext hc)
        { 
            string id=hc.Request["id"].Trim();
            if(string.IsNullOrWhiteSpace(id)){return;}

            BookMarkManager bm = new BookMarkManager();
            //{"is_error":false,"data":{"rows_deleted":1},"message":"Success"}
             if (!bm.DeleteBookMark(Convert.ToInt32(id))) 
             { 
                 hc.Response.Write("error"); return; 
             }
        }
        #endregion

        #region 错误信息
        public void GetErrorLog(HttpContext hc)
        {
            using (MyDBEntities db=new MyDBEntities())
            {
            StringBuilder sb = new StringBuilder();
                List<errorlog> elList = db.errorlog.OrderByDescending(d=>d.CreateTime).ToList();
                foreach (var item in elList)
                {
                    sb.Append("<tr><td>" + item.Id + "</td><td><b>错误信息：</b>" + item.ErrorMsg + "<br/><b>错误发生时间：</b>" + item.CreateTime + "<br/><b>错误备注：</b>" + item.Comment + "</td></tr>");
                }
                hc.Response.Write(sb.ToString());
            }
        }

        public void WriteErrorLog(Exception ex)
        {

        }
        #endregion

        #region 剪切板
        public void SyncJQB(HttpContext hc)
        {

            string txt = hc.Request["txt"];
            NetClipboardSyncTool nc = new NetClipboardSyncTool();
            hc.Response.Write(nc.AddNetClipboard(txt));
        }
        #endregion

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