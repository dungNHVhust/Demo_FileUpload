using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

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

        //Fix Upload
        public static string UploadFile_Fixed(FileUpload oFile, string[] lstFileType, int iMaxLength, string sFolderRoot)
        {
            string result;
            try
            {
                string ext = Path.GetExtension(oFile.FileName).ToLowerInvariant();

                // Check MIME + magic
                if (!IsImageFile(oFile.PostedFile, lstFileType))
                    throw new Exception("Upload sai định dạng tệp tin. Chỉ cho phép upload các định dạng : " + string.Join(", ", lstFileType));                
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

                string fileName = Guid.NewGuid().ToString() + ext;
                string physicalPath = HttpContext.Current.Server.MapPath(sFolderRoot);
                string savePath = Path.Combine(physicalPath, fileName);


                // Lưu file
                oFile.PostedFile.SaveAs(savePath);

                // Trả về đường dẫn tương đối
                return sFolderRoot.TrimEnd('/') + "/" + fileName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }


        public static bool IsImageFile(HttpPostedFile file, string[] allowedMimeTypes)
        {
            string mimeType = file.ContentType?.ToLowerInvariant();
            if (string.IsNullOrEmpty(mimeType) || !allowedMimeTypes.Contains(mimeType))
                return false;

            byte[] header = new byte[8];
            file.InputStream.Read(header, 0, header.Length);
            file.InputStream.Position = 0;

            switch (mimeType)
            {
                case "image/jpeg":
                    return header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF;

                case "image/png":
                    return header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47;

                case "image/gif":
                    return header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46 &&
                           header[3] == 0x38 && (header[4] == 0x37 || header[4] == 0x39) && header[5] == 0x61;

                default:
                    return false;
            }
        }

    }
}