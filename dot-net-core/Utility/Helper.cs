using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Aes = System.Security.Cryptography.Aes;

namespace EMenuApplication.Utility
{
    public class Helper
    {
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);

                        cs.Close();
                    }

                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);

                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string Convert_StringvalueToHexvalue(string stringvalue, Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(stringvalue);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }
        public static string Convert_HexvalueToStringvalue(string hexvalue, Encoding encoding)
        {
            int CharsLength = hexvalue.Length;
            byte[] bytesarray = new byte[CharsLength / 2];
            for (int i = 0; i < CharsLength; i += 2)
            {
                bytesarray[i / 2] = Convert.ToByte(hexvalue.Substring(i, 2), 16);
            }
            return encoding.GetString(bytesarray);
        }

        public static async Task<string> FileUploadAsync(string path, IFormFile file)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fileName = Path.GetRandomFileName().Replace(".", "") + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(path, fileName);
            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }

        public static string ConvertNumeralsToArabic(string input)
        {

            return input = input.Replace('0', '٠')

                        .Replace('1', '۱')

                        .Replace('2', '۲')

                        .Replace('3', '۳')

                        .Replace('4', '٤')

                        .Replace('5', '۵')

                        .Replace('6', '٦')

                        .Replace('7', '٧')

                        .Replace('8', '٨')

                        .Replace('9', '٩');
        }

        public static int success_code = 1;
        public static int failure_code = 0;
        public static int refernce_error_code = -3;
        public static int dateaway_error_code = -1;
        public static int commingsoon_code = 2;
        public static int onsamedate_code = -2;
        public static string lang_ar = "AR";
        public static string CommentCardResultHeader = "CRM_CommentCardResultHeader";
        public static string Nationality = "Nationality";
        public static string COR = "COR";

        public static string Active = "Active";
        public static string Inactive = "Inactive";
        public static string All = "All";
        public static decimal MaxPrice = 1000000.00m;
        public static decimal Price = 9999.99m;

        public static string Approved = "Approved";
        public static string Reject = "Reject";

        public static Byte[] GenerateQRCode(string txtQRCode)
        {
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(txtQRCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static string GetImagePath(string imageName)
        {
            return Path.Combine(AppContext.AppBaseUrl, "Image", imageName).Replace(@"\","/");
        }

        public static List<string> StringToList(string str)
        {
            return str.Split(",").ToList();
        }

        public static List<int> StringToIntList(string str)
        {
            return str.Split(",").Select(int.Parse).ToList();
        }

        public static string DecimalToString(decimal value)
        {
            var result = value - Math.Floor(value);
            return result > 0 ? value.ToString("#,##0.00") : value.ToString("#,##0");
        }

    }
    public class CommonResponse<T>
    {
        public int status { get; set; }
        public string message { get; set; }
        public T dataenum { get; set; }
    }
    enum HTMLELEMENT
    {
        TEXT = 1,
        CHECKBOXLIST = 2,
        RADIOBUTTONLIST = 3,
        DATE = 4,
        DROPDOWN = 5,
        TEXTBOX = 6,
        SLIDER = 7
    }
    enum QTYPEVALUE
    {
        type_Rating = 1,
        type_YesNo = 2,
        type_VisitFreq = 3
    }
    public enum USERINFO
    {
        FirstName = 1,
        LastName = 2,
        Email = 3,
        Mobile = 4,
        Birthday = 5,
        Gender = 6,
        Nationality = 7,
        COR = 8,
    }

    public enum VoucherSetupType
    {
        AmountType = 1,
        PercentageType = 2,
        HashOfPax = 3,
        ItemType = 4,
    }

    public enum VoucherIssuanceSource
    {
        PreDefined = 1,
        CXE = 2,
        Campaign = 3
    }
}
