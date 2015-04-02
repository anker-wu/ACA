<%@ Page Language="C#" AutoEventWireup="true" Inherits="Admin_TreeJSON_Basic" Codebehind="Basic.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
    body,td {
	    background:#ff6fb; width:100%;
        font-family: tahoma,arial,verdana,sans-serif;
        font-size: 11px;
    } 
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        All:<br/>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        Find:<br/>
        <asp:GridView ID="GridView2" runat="server">
        </asp:GridView>
    </div>
    </form>
</body>
</html>
