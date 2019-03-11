using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CaptchaTest
{
    class GasBillInquiry
    {
        public string UserAgent { set; get; }
        public WebProxy Proxy { set; get; }
        public IPAddress IPAddress { set; get; }
        public string CaptchaUrl { set; get; }
        public Image CaptchaImage { set; get; }
        public string CaptchaText { set; get; }
        public string RequestString { set; get; }
        public string ResponseString { set; get; }
        public long FailedCounter { set; get; }
        public GasBill Bill { set; get; }
        public HtmlDocument HtmlDocument { set; get; }
        public GasBillInquirySearchFields SearchFields { set; get; }
        public string ViewState { get; private set; }
        public string EventValidation { get; private set; }

        public bool Get()
        {
            UserAgent = ProjectValues.GetRandomUserAgent();
            Proxy = new WebProxy("185.8.174.146:8080") { Credentials = new NetworkCredential("user", "1qaz2wsx") };// new WebProxy("127.0.0.1:8888"); ProjectValues.GetGasWebProxy();
            IPAddress = ProjectValues.GetGasBindIp();

        start:
            var status = Send1();
            if (status == false)
            {
                return false;
            }

            status = Send2();
            if (status == false)
            {
                return false;
            }

            CaptchaText = Helper.SolveCaptchaWithAntiCaptcha(out int execTime, CaptchaImage);//Works

            var t = 5000 - execTime;
            if (t > 0)
                Thread.Sleep(t);

            status = Send3();
            if (status == false)
            {
                if (FailedCounter < 3)
                {
                    goto start;
                }

                return false;
            }

            status = Extract();
            if (status == false)
            {
                return false;
            }

            return true;
        }
        private bool Send1()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://billing.nigc.ir/billing/gasAll.aspx");
                request.Method = "Get";
                request.Timeout = 60 * 1000;
                request.UserAgent = UserAgent;
                request.Accept = "*/*";
                request.Proxy = Proxy;

                var responseString = (string)null;
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    responseString = reader.ReadToEnd();
                    HtmlDocument = new HtmlDocument();
                    HtmlDocument.LoadHtml(responseString);
                }

                var inputs = HtmlDocument.DocumentNode.SelectNodes(@"//input").ToList();
                foreach (var input in inputs)
                {
                    if (string.Equals(input.Attributes["name"]?.Value, "__VIEWSTATE", StringComparison.OrdinalIgnoreCase))
                    {
                        ViewState = HttpUtility.UrlEncode(input.Attributes["value"].Value);
                        continue;
                    }

                    if (string.Equals(input.Attributes["name"]?.Value, "__EVENTVALIDATION", StringComparison.OrdinalIgnoreCase))
                    {
                        EventValidation = HttpUtility.UrlEncode(input.Attributes["value"].Value);
                        continue;
                    }
                }

                var images = HtmlDocument.DocumentNode.SelectNodes(@"//img").ToList();
                foreach (var image in images)
                {
                    if (image.Attributes["src"]?.Value.Contains("CaptchaImage.ashx") == true)
                    {
                        CaptchaUrl = HttpUtility.UrlDecode(image.Attributes["src"].Value);
                        continue;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.ReportGasBlockedProxy();
                return false;
            }
        }
        private bool Send2()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create($"http://billing.nigc.ir/billing/{CaptchaUrl}");
                request.Method = "Get";
                request.Timeout = 60 * 1000;
                request.UserAgent = UserAgent;
                request.Accept = "*/*";
                request.Proxy = Proxy;

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                {
                    CaptchaImage = Image.FromStream(responseStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool Send3()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var body =
                    $"__EVENTTARGET=" +
                    $"&__EVENTARGUMENT=" +
                    $"&__VIEWSTATE={ViewState}" +
                    $"&__VIEWSTATEGENERATOR=19D0DAE0" +
                    $"&__EVENTVALIDATION={EventValidation}" +
                    $"&txKey={SearchFields.GasParticipateCode}" +
                    $"&CaptchaControl1={CaptchaText}" +
                    $"&btnSearch=%D9%85%D8%B4%D8%A7%D9%87%D8%AF%D9%87";
                var contentBytes = Encoding.UTF8.GetBytes(body);
                var request = (HttpWebRequest)WebRequest.Create($"http://billing.nigc.ir/billing/gasAll.aspx?c=1&k={SearchFields.GasParticipateCode}");
                request.Method = "POST";
                request.Timeout = 30 * 1000;
                request.ContentLength = contentBytes.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
                request.UserAgent = UserAgent;
                request.Referer = "http://billing.nigc.ir/billing/gasAll.aspx?c=1&k={SearchFields.GasParticipateCode}";
                request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                request.Proxy = Proxy;
                request.KeepAlive = true;
                request.ServicePoint.Expect100Continue = false;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (var requestWriter = request.GetRequestStream())
                    requestWriter.Write(contentBytes, 0, (int)request.ContentLength);

                var responseString = (string)null;
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    responseString = reader.ReadToEnd();
                    HtmlDocument = new HtmlDocument();
                    HtmlDocument.LoadHtml(responseString);
                }

                if (responseString.Contains("كد امنيتي وارد شده صحيح نمي باشد."))
                {
                    FailedCounter++;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                FailedCounter = 9999;
                return false;
            }
        }
        private bool Extract()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var allDetails = HtmlDocument.DocumentNode.Descendants().Where(x => (x.Name == "table" && x.Attributes["border"].Value == "1" && x.Attributes["bordercolor"].Value == "#000099" && x.Attributes["cellpadding"] != null)).ToList();
                if (allDetails.Count == 0)
                {
                    return false;
                }

                var detailsInSide = allDetails[0].Descendants("td").ToList();
                var consumptionNodeInSide = allDetails[1].Descendants("td").ToList();
                var billNodeInSide = allDetails[2].Descendants("td").ToList();
                var amount = HttpUtility.HtmlDecode(billNodeInSide[4].InnerText)?.Replace("٫", "").ToEnglishNumber();
                var paymentId = HttpUtility.HtmlDecode(billNodeInSide[6].InnerText).ToEnglishNumber();
                var billId = HttpUtility.HtmlDecode(billNodeInSide[7].InnerText).ToEnglishNumber();
                var tempBillId = $"{SearchFields.GasParticipateCode.Remove(0, SearchFields.GasParticipateCode.Length - 7)}3".ToEnglishNumber();

                Bill = new GasBill
                {
                    FullName = HttpUtility.HtmlDecode(detailsInSide[2].InnerText),
                    Address = HttpUtility.HtmlDecode(detailsInSide[4].InnerText),
                    PaymentDate = Helper.ShamsiDDMMYYToGregorianDateTime(HttpUtility.HtmlDecode(billNodeInSide[5].InnerText).ToEnglishNumber().Replace(" ", "")),
                    CurrentDate = Helper.ShamsiDDMMYYToGregorianDateTime(HttpUtility.HtmlDecode(consumptionNodeInSide[7].InnerText).ToEnglishNumber().Replace(" ", "")),
                    PreviousDate = Helper.ShamsiDDMMYYToGregorianDateTime(HttpUtility.HtmlDecode(consumptionNodeInSide[6].InnerText).ToEnglishNumber().Replace(" ", "")),
                    Amount = long.TryParse(amount, out var parseAmount) ? parseAmount : 0,
                    PaymentID = string.Equals(paymentId, "0000000000000") ? "000000000001" : paymentId,
                    BillID = string.IsNullOrEmpty(billId) ? Helper.AppendControlNumberToBillId(tempBillId) : billId,
                };

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
