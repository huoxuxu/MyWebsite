using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MyWebsiteByAEF.DBModel;
using Common;

namespace MyWebsiteByAEF.API.ForDB
{
    /// <summary>
    /// QuestionAndAnswerTable 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class QuestionAndAnswerTable : System.Web.Services.WebService
    {
        [WebMethod]
        public bool Add(questionandanswer t)
        {
            try
            {
                using (MyDBEntities db = new MyDBEntities())
                {
                    db.questionandanswer.AddObject(t);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        [WebMethod]
        public bool Delete(string id)
        {
            try
            {
                
                using (MyDBEntities db = new MyDBEntities())
                {
                    questionandanswer qaa = db.questionandanswer.FirstOrDefault(n => n.Id == id);
                    if (qaa != null)
                    {
                        db.questionandanswer.DeleteObject(qaa);
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch { return false; }
        }

        [WebMethod]
        public bool Update(questionandanswer t)
        {
            try
            {
                
                using (MyDBEntities db = new MyDBEntities())
                {
                    questionandanswer qaa = db.questionandanswer.FirstOrDefault(n => n.Id == t.Id);
                    if (qaa != null)
                    {
                        qaa.Question = t.Question;
                        qaa.Answer = t.Answer;
                        qaa.Keyword = t.Keyword;
                        qaa.Hits = t.Hits;
                        qaa.CreateTime = t.CreateTime;
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch { return false; }
        }

        [WebMethod]
        public questionandanswer Select(string id)
        {
            try
            {
                
                using (MyDBEntities db = new MyDBEntities())
                {
                    return db.questionandanswer.FirstOrDefault(n => n.Id == id);
                }
            }
            catch { return new questionandanswer(); }
        }

        /// <summary>
        /// 返回IQueryabled
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public IQueryable<questionandanswer> SelectManyWithQueryable()
        {
            try
            {
                return new MyDBEntities().questionandanswer.AsQueryable();
            }
            catch { return null; }
        }
    }
}
