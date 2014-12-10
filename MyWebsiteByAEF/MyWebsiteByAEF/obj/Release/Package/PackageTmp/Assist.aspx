<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Assist.aspx.cs" Inherits="MyWebsiteByAEF.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        问题：<asp:TextBox ID="TextBox1" runat="server" Text="我是测试点额数据"></asp:TextBox><br />
        答案：<asp:TextBox ID="TextBox2" runat="server" Text="我是测试点额数据"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="Save" onclick="Button1_Click" />
        <br /><br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Question" HeaderText="Question" />
                <asp:BoundField DataField="Answer" HeaderText="Answer" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
