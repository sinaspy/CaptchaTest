using Newtonsoft.Json;
using Persia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

//using AForge.Imaging.Filters;
//using Emgu.CV;
//using Emgu.CV.Structure;



//using HtmlAgilityPack;

//using ImageProcessor;
//using ImageProcessor.Imaging.Formats;

//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;

//using Newtonsoft.Json;

//using Persia;

//using Tesseract;

namespace CaptchaTest
{
    public static partial class Helper
    {
        //base helper
        public static string ToEnglishNumber(this string number)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var arabicDigits = new char[10] { '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩' };
                var persianDigits = new char[10] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
                var englishDigits = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                if (!string.IsNullOrEmpty(number))
                    for (int i = 0; i < persianDigits.Length; i++)
                        number = number.Replace(persianDigits[i], englishDigits[i]).Replace(arabicDigits[i], englishDigits[i]);

                var englishNumber = number;
                //Log.Trace(ProjectValues.SuccessfulLog, sw.Elapsed.TotalMilliseconds);
                return englishNumber;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string Decrypt(string cipherText)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var decryptedString = string.Empty;
                var vectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");
                var cipherTextBytes = Convert.FromBase64String(cipherText);
                using (var password = new PasswordDeriveBytes(ProjectValues.CryptographyKey, null))
                {
                    var keyBytes = password.GetBytes(32);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, vectorBytes))
                        {
                            using (var memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[cipherTextBytes.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    decryptedString = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }

                //Log.Trace(ProjectValues.SuccessfulLog, sw.Elapsed.TotalMilliseconds);
                return decryptedString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //helper
        public static void ReportGasBlockedProxy()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                lock (ProjectValues.GasWebProxy)
                {
                    var temp = new WebProxy((string)ProjectValues.GasWebProxy.WebProxy);
                    File.AppendAllText(@"C:\AyanTech\Inquiry\WebCrawler\Files\Proxy\GasBlockList.txt", $"{temp.Address.Host}:{temp.Address.Port}{Environment.NewLine}");
                    ProjectValues.GasWebProxy = new { WebProxy = (string)null };
                }

                //Log.Trace("successfully completed.", sw.ElapsedMilliseconds);
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public static string SolveCaptchaWithAntiCaptcha(out int executionTime, Image captchaImage, bool numeric = false, string languagePool = "en", bool @case = false, bool phrase = false)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var taskId = 0;
                var captchaText = (string)null;
                using (var client = new HttpClient())
                {
                    var body = new
                    {
                        clientKey = "7accc226bdd28887a68449d247e42827",
                        task = new
                        {
                            type = "ImageToTextTask",
                            body = Convert.ToBase64String((byte[])new ImageConverter().ConvertTo(captchaImage, typeof(byte[]))),
                            numeric,
                            languagePool,
                            @case,
                            phrase,
                            math = 0,
                            minLength = 0,
                            maxLength = 0
                        }
                    };
                    var content = new StringContent(JsonSerializer(body));
                    var result = client.PostAsync("http://api.anti-captcha.com/createTask", content).Result;

                    var responseString = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var response = JsonDeserializer<dynamic>(responseString);

                    if (response.errorId != 0)
                    {
                        executionTime = Convert.ToInt32(sw.Elapsed.TotalMilliseconds);
                        return null;
                    }

                    taskId = response.taskId;
                }

                Thread.Sleep(3000);
                for (int i = 0; i < 40; i++)
                {
                    using (var client = new HttpClient())
                    {
                        var body = new
                        {
                            clientKey = "7accc226bdd28887a68449d247e42827",
                            taskId
                        };
                        var content = new StringContent(JsonSerializer(body));
                        var result = client.PostAsync("http://api.anti-captcha.com/getTaskResult", content).Result;

                        var responseString = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        var response = JsonDeserializer<dynamic>(responseString);

                        if (response.errorId != 0)
                        {
                            executionTime = Convert.ToInt32(sw.Elapsed.TotalMilliseconds);
                            return null;
                        }

                        if (!string.Equals((string)response.status, "ready", StringComparison.OrdinalIgnoreCase))
                        {
                            Thread.Sleep(1000);
                            continue;
                        }

                        captchaText = response.solution.text;
                        break;
                    }
                }

                //Log.Trace("successfully completed.", sw.ElapsedMilliseconds);
                executionTime = Convert.ToInt32(sw.Elapsed.TotalMilliseconds);
                return captchaText;
            }
            catch (Exception ex)
            {
                executionTime = Convert.ToInt32(sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static T JsonDeserializer<T>(string jsonString)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var instance = JsonConvert.DeserializeObject<T>(jsonString);

                //Log.Trace("successfully completed.", sw.ElapsedMilliseconds);
                return instance;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
        public static string JsonSerializer<T>(T jsonObject)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var jsonString = JsonConvert.SerializeObject(jsonObject);

                //Log.Trace("successfully completed.", sw.ElapsedMilliseconds);
                return jsonString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DateTime? ShamsiDDMMYYToGregorianDateTime(string date)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var dateParts = date.Split('/');
                var gregorianDateTime = Calendar.ConvertToGregorian(Convert.ToInt32("13" + dateParts[2]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[0]), DateType.Persian);

                //Log.Trace("successfully completed.", sw.ElapsedMilliseconds);
                return gregorianDateTime;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DateTime? ShamsiDDMMYYYYToGregorianDateTime(string date)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var dateParts = date.Split('/');
                var gregorianDateTime = Calendar.ConvertToGregorian(Convert.ToInt32(dateParts[2]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[0]), DateType.Persian);

                //Log.Trace("successfully completed.", sw.ElapsedMilliseconds);
                return gregorianDateTime;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string AppendControlNumberToBillId(string billId)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var c = 2;
                var total = 0;
                string controlNumber;
                var charArray = billId.ToCharArray();
                for (var i = charArray.Length - 1; i > -1; i--)
                {
                    if (c > 7) c = 2;
                    total = (Convert.ToInt32(charArray[i].ToString()) * c) + total;
                    c++;
                }

                var mod = total % 11;
                if (mod == 0 || mod == 1)
                    controlNumber = "0";
                else
                    controlNumber = (11 - mod).ToString();

                return $"{billId}{controlNumber}";
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


    }
}