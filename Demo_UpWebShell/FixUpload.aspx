<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FixUpload.aspx.cs" Inherits="Demo_UpWebShell.FixUpload" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload Ảnh</title>
    <style type="text/css">
        body {
            font-family: 'Segoe UI', Arial, sans-serif;
            margin: 0;
            padding: 0;
            background: #f0f2f5;
        }
        .main-container {
            max-width: 900px;
            margin: 40px auto;
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 4px 24px rgba(0,0,0,0.08);
            padding: 32px 24px 24px 24px;
        }
        #CPopupImageUpload {
            display: flex;
            gap: 32px;
        }
        #LeftBar {
            width: 25%;
        }
        #LeftBar .listfolder {
            list-style: none;
            padding: 0;
            margin: 0 0 24px 0;
        }
        #LeftBar ul li h1 {
            font-size: 18px;
            font-weight: 600;
            color: #333;
            margin: 0 0 12px 0;
        }
        #RightBar {
            width: 75%;
        }
        #FileBox {
            display: flex;
            gap: 24px;
        }
        #ListFile {
            width: 55%;
            max-height: 340px;
            overflow-y: auto;
            background: #fafbfc;
            border-radius: 8px;
            border: 1px solid #e0e0e0;
            padding: 16px;
        }
        #ListFile ul {
            padding: 0;
            margin: 0;
        }
        #ListFile ul li {
            margin-bottom: 12px;
            display: flex;
            align-items: center;
        }
        #ListFile .btn-delete {
            background: #ff4d4f;
            color: #fff;
            border: none;
            border-radius: 4px;
            padding: 4px 12px;
            margin-right: 12px;
            cursor: pointer;
            transition: background 0.2s;
        }
        #ListFile .btn-delete:hover {
            background: #d9363e;
        }
        #ListFile .file-label {
            cursor: pointer;
            color: #007bff;
            transition: color 0.2s;
        }
        #ListFile .file-label:hover {
            color: #0056b3;
        }
        #ImagePreview {
            width: 45%;
            min-width: 180px;
            display: flex;
            align-items: center;
            justify-content: center;
            background: #f8f8f8;
            border-radius: 8px;
            border: 1px solid #e0e0e0;
            min-height: 220px;
        }
        #ImagePreview img {
            max-width: 90%;
            max-height: 200px;
            border-radius: 6px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.07);
        }
        #UploadBar {
            margin-top: 24px;
            padding: 16px;
            background: #f6f8fa;
            border-radius: 8px;
            border: 1px solid #e0e0e0;
            display: flex;
            align-items: center;
            gap: 16px;
        }
        #UploadBar .aspNet-FileUpload {
            margin-right: 12px;
        }
        #UploadBar .aspNet-Button {
            background: #1890ff;
            color: #fff;
            border: none;
            border-radius: 4px;
            padding: 6px 18px;
            cursor: pointer;
            font-weight: 500;
            transition: background 0.2s;
        }
        #UploadBar .aspNet-Button:hover {
            background: #096dd9;
        }
        #UploadBar .aspNet-Label {
            margin-left: 12px;
            color: #faad14;
            font-weight: 500;
        }
    </style>
</head>
<body>
    <div class="main-container">
        <form id="form1" runat="server">
            <div id="CPopupImageUpload">
                <div id="LeftBar">
                    <ul class="listfolder">
                        <li>
                            <h1><asp:HyperLink ID="HyperLink2" runat="server">Images</asp:HyperLink></h1>
                        </li>
                    </ul>
                </div>
                <div id="RightBar">
                    <div id="FileBox">
                        <div id="ListFile">
                            <asp:Repeater ID="rptrList" runat="server" OnItemDataBound="rptrList_ItemDataBound_FixUpload">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <asp:LinkButton ID="lbtnDelete" runat="server" CssClass="btn-delete" CommandArgument='<%# Eval("PATH") %>' OnClick="lbtnDelete_Click_FixUpload">Xóa</asp:LinkButton>
                                        <asp:Label ID="lblImage" EnableViewState="false" runat="server" CssClass="file-label" Text='<%# Eval("NAME") %>'></asp:Label>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate> 
                            </asp:Repeater>
                        </div>
                        <div id="ImagePreview">
                            <img id="imgImagePreview" src="" alt="Preview" />
                        </div>
                    </div>
                    <div id="UploadBar">
                        Tệp tin: <asp:FileUpload ID="FileUpload_FixUpload" Width="120px" runat="server" CssClass="aspNet-FileUpload" />
                        <span style="color:#888; font-size:13px;">(*.gif; *.jpg; *.png; )</span>
                        <asp:Button ID="btnUpload" runat="server" Text="Tải lên" OnClick="btnUpload_Click_FixUpload" CssClass="aspNet-Button" />
                        <asp:Label ID="lblMessage" EnableViewState="false" Text="" runat="server" CssClass="aspNet-Label"></asp:Label>
                    </div>
                </div>
            </div>
        </form>
    </div>
</body>
</html>

