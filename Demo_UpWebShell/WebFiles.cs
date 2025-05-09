using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Demo_UpWebShell
{
    public class WebFiles
    {
        // Token: 0x060000AC RID: 172 RVA: 0x00004BF0 File Offset: 0x00002DF0
        public static string UploadFile(FileUpload oFile, string[] lstFileType, int iMaxLength, string sFolderRoot)
        {
            string result;
            try
            {
                bool flag = false;
                foreach (string value in lstFileType)
                {
                    flag |= oFile.PostedFile.ContentType.ToString().Equals(value, StringComparison.OrdinalIgnoreCase);
                }
                if (!flag)
                {
                    throw new Exception("Upload sai định dạng tệp tin. Chỉ cho phép upload các định dạng : " + lstFileType.ToString());
                }
                if (oFile.PostedFile.ContentLength > iMaxLength * 1024 * 1024)
                {
                    throw new Exception("Tệp tin quá dung lượng cho phép. Tối đa chỉ được upload " + iMaxLength + "MB.");
                }
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(sFolderRoot)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(sFolderRoot));
                }
                if (File.Exists(HttpContext.Current.Server.MapPath(sFolderRoot + "/" + Path.GetFileName(oFile.PostedFile.FileName))))
                {
                    throw new Exception("Đã có Tệp tin tồn tại. Đường dẫn file : " + sFolderRoot + "/" + Path.GetFileName(oFile.PostedFile.FileName));
                }
                oFile.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(sFolderRoot + "/" + Path.GetFileName(oFile.PostedFile.FileName)));
                result = sFolderRoot + "/" + Path.GetFileName(oFile.PostedFile.FileName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}