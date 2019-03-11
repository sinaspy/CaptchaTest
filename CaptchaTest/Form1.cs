using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;

using HtmlAgilityPack;
using System.Web;

namespace CaptchaTest
{
    public partial class Form1 : Form
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
        public HtmlAgilityPack.HtmlDocument HtmlDocument { set; get; }
        public GasBillInquirySearchFields SearchFields { set; get; }
        public string ViewState { get; private set; }
        public string EventValidation { get; private set; }
        public Form1()
        {
            InitializeComponent();
            var v = ProjectValues.UserAgents;
        }

        private void btnGetImage_Click(object sender, EventArgs e)
        {
            bool Send1()
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
                        HtmlDocument = new HtmlAgilityPack.HtmlDocument();
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
            bool Send2()
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

            Send1();
            txtCaptchaLink.Text = CaptchaUrl;
            Send2();
            picOriginal.Image = CaptchaImage;
        }
    }
}
