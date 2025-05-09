using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Demo_UpWebShell
{
    public partial class Default : System.Web.UI.Page
    {
        private string sViewFolder = "Uploads";  //private string sViewFolder = "appdata/system/weblayouts";
        private string sImagePreview = "imgImagePreview";
        private string sImageLink = "imgImageLink";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.sImagePreview = base.Request.QueryString["c1"].ToString();
                this.sImageLink = base.Request.QueryString["c2"].ToString();
            }
            catch
            {
            }
            if (!base.IsPostBack)
            {
                this.BindListImage();
            }
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string commandArgument = ((LinkButton)sender).CommandArgument;
                string text = base.Server.MapPath("~/" + commandArgument);
                FileInfo fileInfo = new FileInfo(text);
                string fullName = fileInfo.FullName;
                if (fileInfo != null)
                {
                    fileInfo.Delete();
                    if (!File.Exists(text))
                    {
                        this.lblMessage.Attributes.CssStyle.Add("color", "#090");
                        this.lblMessage.Text = "Đã xóa thành công ảnh";
                    }
                    else
                    {
                        this.lblMessage.Attributes.CssStyle.Add("color", "#f00");
                        this.lblMessage.Text = "Có lỗi khi xóa ảnh. Bạn vui lòng thao tác lại";
                    }
                }
                else
                {
                    this.lblMessage.Attributes.CssStyle.Add("color", "#f00");
                    this.lblMessage.Text = "Không có hình ảnh";
                }
                this.BindListImage();
            }
            catch
            {
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.FileUpload1.HasFile)
                {
                    string str = WebFiles.UploadFile(this.FileUpload1, new string[]
                    {
                        "image/gif",
                        "image/jpeg",
                        "image/png"
                    }, 2, "~/" + this.sViewFolder);
                    this.BindListImage();
                    this.lblMessage.Text = "File " + str + " đã được upload.";
                    this.lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#090");
                }
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }
        }

        protected void rptrList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DataRowView dataRowView = (DataRowView)e.Item.DataItem;
                Label label = e.Item.FindControl("lblImage") as Label;
                if (label != null)
                {
                    label.Attributes.CssStyle.Add("cursor", "pointer");
                    label.Attributes.Add("onmouseover", "document.getElementById('imgImagePreview').src='" + base.ResolveClientUrl("~/" + dataRowView["PATH"]) + "';this.style.color='#960';");
                    label.Attributes.Add("onmouseout", "this.style.color='#000';");
                    label.Attributes.Add("onclick", string.Concat(new string[]
                    {
                        "parent.document.getElementById('",
                        this.sImagePreview,
                        "').src = document.getElementById('imgImagePreview').src;parent.document.getElementById('",
                        this.sImageLink,
                        "').value = document.getElementById('imgImagePreview').src;alert('Ảnh đã được chọn');"
                    }));
                }
            }
        }

        private void BindListImage()
        {
            try
            {
                DataTable listImageFromFolder = this.GetListImageFromFolder(this.sViewFolder);
                this.rptrList.DataSource = listImageFromFolder;
                this.rptrList.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        private DataTable GetListImageFromFolder(string sFolder)
        {
            DataTable result;
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(new DataColumn("NAME", typeof(string)));
                dataTable.Columns.Add(new DataColumn("PATH", typeof(string)));
                string[] files = Directory.GetFiles(base.Server.MapPath("~/" + sFolder));
                foreach (string fileName in files)
                {
                    FileInfo fileInfo = new FileInfo(fileName);
                    try
                    {
                        Bitmap bitmap = new Bitmap(fileInfo.FullName);
                        bitmap.Dispose();
                        string value = sFolder + "/" + fileInfo.Name;
                        DataRow dataRow = dataTable.NewRow();
                        dataRow["NAME"] = fileInfo.Name;
                        dataRow["PATH"] = value;
                        dataTable.Rows.Add(dataRow);
                    }
                    catch
                    {
                    }
                }
                result = dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}