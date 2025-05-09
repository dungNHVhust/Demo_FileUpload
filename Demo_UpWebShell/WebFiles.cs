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

        // Token: 0x060000AD RID: 173 RVA: 0x00004D88 File Offset: 0x00002F88
        public static string UploadFile(FileUpload oFile, string[] lstFileType, int iMaxLength, string sFolderRoot, bool bRandomFileName)
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
                    throw new Exception("ERROR_FILE_TYPE");
                }
                if (oFile.PostedFile.ContentLength > iMaxLength * 1024 * 1024)
                {
                    throw new Exception("ERROR_TOTAL");
                }
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(sFolderRoot)))
                {
                    throw new Exception("ERROR_ROOT_FOLDER_EXIST");
                }
                string str = Path.GetFileName(oFile.PostedFile.FileName);
                string extension = Path.GetExtension(oFile.PostedFile.FileName);
                if (!bRandomFileName)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath(sFolderRoot + "/" + str)))
                    {
                        throw new Exception("ERROR_DUPPLICATE");
                    }
                    oFile.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(sFolderRoot + "/" + str));
                }
                else
                {
                    string str2 = DateTime.Now.Ticks.ToString();
                    str = str2 + extension;
                    oFile.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(sFolderRoot + "/" + str));
                }
                result = sFolderRoot + "/" + str;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}