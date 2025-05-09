# Web Shell Upload Lab

## Mô tả:

Lab này mô phỏng một ứng dụng web ASP.NET có một số lỗ hổng bảo mật nghiêm trọng, cho phép kẻ tấn công upload và thực thi **web shell** thông qua các endpoint `/Default` và `/RandomFile`. Những lỗ hổng này liên quan đến việc kiểm tra file upload không chặt chẽ, giúp kẻ tấn công có thể tải lên các file độc hại như **web shell**.

## Các endpoint chính:

### 1. `/Default`:

Endpoint này cho phép người dùng upload ảnh và lưu tên của ảnh theo tên gốc. Điều này tạo ra nguy cơ nếu file không được kiểm tra kỹ.

### 2. `/RandomFile`:

Endpoint này cho phép người dùng upload các file lên server, nhưng nó lại đặt tên file bằng **một giá trị thời gian**. Cụ thể, tên file sẽ được đặt theo giá trị **ticks** của thời gian hiện tại (ticks là số lượng đơn vị thời gian nhỏ nhất tính từ năm 0001, được sử dụng trong .NET). Điều này có thể gây ra vấn đề nếu không kiểm soát được loại file upload.

## Phần code gây lỗi:

Lỗi chính xảy ra trong quá trình **upload file**. Ứng dụng kiểm tra tên file dựa trên **ContentType** của file, nhưng không kiểm tra kỹ loại file được upload. Điều này khiến cho kẻ tấn công có thể upload các loại file không phải ảnh (chẳng hạn như web shell).

Dưới đây là đoạn mã có lỗi:

```csharp
bool flag = false;
foreach (string value in lstFileType)
{
    flag |= oFile.PostedFile.ContentType.ToString().Equals(value, StringComparison.OrdinalIgnoreCase);
}
if (!flag)
{
   ....
}
...
oFile.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(sFolderRoot + "/" + Path.GetFileName(oFile.PostedFile.FileName)));
result = sFolderRoot + "/" + Path.GetFileName(oFile.PostedFile.FileName);
```
