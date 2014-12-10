using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWebsiteByAEF.DBModel;

namespace MyWebsiteByAEF
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GVBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (MyDBEntities db = new MyDBEntities())
            {
                questionandanswer qaa = new questionandanswer();
                qaa.Id = Guid.NewGuid().ToString();
                qaa.Question = TextBox1.Text;
                qaa.Answer = TextBox2.Text;
                qaa.Keyword = TextBox1.Text;
                qaa.Hits = 0;
                qaa.CreateTime = DateTime.Now;
                db.questionandanswer.AddObject(qaa);
                db.SaveChanges();
                GVBind();
            }
        }
        public void GVBind()
        {
            using (MyDBEntities db=new MyDBEntities ())
            {
                GridView1.DataSource = db.questionandanswer.OrderByDescending(d=>d.CreateTime).ToList();
                GridView1.DataBind();

            }
        }
    }
}