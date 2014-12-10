//标题
var mainBiaoTi = "<div style=\"margin-left:20px; margin-bottom:20px;\"><table style=\" cursor:pointer;\">\
<tr><td rowspan=\"2\"><span class=\"title\">花无</span></td><td><span class=\"subtitle\">做一个正直的人,有益于社会的人！</span></td></tr>\
<tr><td><span class=\"subtitle\">huawublog.v5.org</span></td></tr></table></div>";
//主页显示模板
var mainTemp = "{{if(it.success){}}\
            {{if(it.rows.length>0){}}\
            <table width='100%'>\
            {{for(var ro in it.rows){}}\
            <tr><td colspan='2'>\
            <a class='contentTile' href='#' onclick='ToXQ(\"{{=it.rows[ro].Id}}\")'>\
            {{=it.rows[ro].Question}}</a>\
<div class='content'>\
{{if(it.rows[ro].Answer.length>300){}}\
{{=it.rows[ro].Answer.substr(0,300)}}\
{{}else{}}\
{{=it.rows[ro].Answer}}\
{{}}}\
</div>\
</td></tr><tr><td><small>\
<b>关键字：</b>\
{{=it.rows[ro].Keyword}}\
</small></td><td><small>\
<b>更新于：</b>\
{{=it.rows[ro].CreateTime}}\
</small></td></tr><tr><td colspan='2'><hr/></td></tr>\
            {{}}}\
            </table>\
            {{}else{}}<p>NoData！</p>{{}}}\
            {{}else{}}<p>{{=it.info}}</p>{{}}}";
//主页详情模板
var XQTemplate = "{{if(it.success){}}\
            {{if(it.total>0){}}\
            <h3 class='contentTile'>{{=it.rows.Question}}</h3>\
            {{=it.rows.Answer}}\
            {{}else{}}<p>NoData！</p>{{}}}\
            {{}else{}}<p>{{=it.info}}</p>{{}}}";