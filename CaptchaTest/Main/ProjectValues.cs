using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Windows.Forms;

namespace CaptchaTest
{
    public static partial class ProjectValues
    {
        //Base ProjectValeus
        public static bool MonitoringMode { private set; get; }
        public static int MonitoringMaxSize { private set; get; }
        public static string SuccessfulLog { private set; get; }
        public static string CryptographyKey { private set; get; }
        public static string ProfileJsonWebToken { private set; get; }
        public static string GeneralMessageFailure { private set; get; }
        public static string[] MonitoringSafeClientsList { private set; get; }
        public static string[] StoredProceduresBlockList { private set; get; }
        public static DataBaseConfigure DataBaseConfigure { private set; get; }
        //public static AccessObject[] AccessObjects { private set; get; }
        //public static LinkedList<MessageValue> ErrorMessagesList { private set; get; }
        public static ConcurrentDictionary<string, string> MessageDictionary { private set; get; }
        //public static ConcurrentDictionary<string, LinkedList<MonitorValue>> MonitoringDictionary { private set; get; }

        static ProjectValues()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var status = InitFixedValues();
                if (status == false)
                {
                    HttpRuntime.UnloadAppDomain();
                    return;
                }

                status = InitDataBase();
                if (status == false)
                {
                    HttpRuntime.UnloadAppDomain();
                    return;
                }

                //status = InitCredential();
                //if (status == false)
                //{
                //    HttpRuntime.UnloadAppDomain();
                //    return;
                //}

                //status = InitMainObjects();
                //if (status == false)
                //{
                //    HttpRuntime.UnloadAppDomain();
                //    return;
                //}
                status = InitOtherObjects();
                if (status == false)
                {
                    HttpRuntime.UnloadAppDomain();
                    return;
                }
            }
            catch (Exception ex)
            {
                HttpRuntime.UnloadAppDomain();
                return;
            }
        }
        //public static string GetProfileJsonWebToken()
        //{
        //    var identity = new Identity
        //    {
        //        Project = new Project { SecretKey = "" },
        //        Token = new AuthenticationToken { StringValue = ProfileJsonWebToken, DoNotInit = true }
        //    };
        //    var status = identity.ValidateTokenString();
        //    if (status == false)
        //        return null;

        //    if (identity.TokenStringIsValid == false)
        //        InitProfileJsonWebToken();

        //    return ProfileJsonWebToken;
        //}
        //public static string GetDataBaseMessage(string errorCategory, string errorCode)
        //{
        //    var index = $"{errorCategory }-{errorCode}";
        //    if (MessageDictionary.ContainsKey(index))
        //        return MessageDictionary[index];

        //    var dataBaseMessage = new DataBaseMessage
        //    {
        //        ErrorCode = errorCode,
        //        Category = errorCategory
        //    };
        //    var status = dataBaseMessage.GetErrorBriefDescription();
        //    if (status == false)
        //        return GeneralMessageFailure;

        //    MessageDictionary.TryAdd(index, dataBaseMessage.BriefDescription);
        //    return MessageDictionary[index];
        //}
        //private static bool InitAccessObjects()
        //{
        //    var identity = new Identity { Project = new Project { Name = ProjectName } };
        //    var status = identity.GetProjectAccessObjects();
        //    if (status == false)
        //        return false;

        //    AccessObjects = identity.Role.AccessObjects;
        //    return true;
        //}
        //private static bool InitCredential()
        //{
        //    var secretKey = new Project { Key = "secretKey" };
        //    var status = secretKey.GetProjectValues();
        //    if (status == false)
        //        return false;

        //    var userName = new Project { Key = "userName" };
        //    status = userName.GetProjectValues();
        //    if (status == false)
        //        return false;

        //    var password = new Project { Key = "password" };
        //    status = password.GetProjectValues();
        //    if (status == false)
        //        return false;

        //    Credential = new Credential
        //    {
        //        UserName = userName.Value,
        //        Password = password.Value,
        //        SecretKey = secretKey.Value
        //    };
        //    return true;
        //}
        private static bool InitDataBase()
        {
            var dataBaseConfigure = new DataBaseConfigure();
            var status = dataBaseConfigure.GetDataBaseConfigure();
            if (status == false)
                return false;

            DataBaseConfigure = dataBaseConfigure;
            return true;
        }
        private static bool InitFixedValues()
        {
            CryptographyKey = @"1234512345678976";
            //ErrorMessagesList = new LinkedList<MessageValue>();
            MessageDictionary = new ConcurrentDictionary<string, string>();
            //MonitoringDictionary = new ConcurrentDictionary<string, LinkedList<MonitorValue>>();

            //SuccessfulLog = WebConfigurationManager.AppSettings["SuccessfulLog"];
            //GeneralMessageFailure = WebConfigurationManager.AppSettings["GeneralMessageFailure"];
            //MonitoringMode = Convert.ToBoolean(WebConfigurationManager.AppSettings["MonitoringMode"]);
            //MonitoringMaxSize = Convert.ToInt32(WebConfigurationManager.AppSettings["MonitoringMaxSize"]);
            //MonitoringSafeClientsList = WebConfigurationManager.AppSettings["MonitoringSafeClientsList"].Split(',');
            //StoredProceduresBlockList = WebConfigurationManager.AppSettings["StoredProceduresBlockList"].Split(',');
            return true;
        }
        //private static bool InitMainObjects()
        //{
        //    var status = InitAccessObjects();
        //    if (status == false)
        //        return false;

        //    status = InitMonitorDictionary();
        //    if (status == false)
        //        return false;

        //    return true;
        //}
        //private static bool InitMonitorDictionary()
        //{
        //    var keys = WebConfigurationManager.AppSettings["MonitoringList"].Split(',');
        //    if (keys != null && keys.Length != 0)
        //        foreach (var key in keys)
        //            MonitoringDictionary.TryAdd(key, new LinkedList<MonitorValue>());

        //    return true;
        //}
        //private static bool InitProfileJsonWebToken()
        //{
        //    var identity = new Identity
        //    {
        //        Token = new AuthenticationToken { DoNotInit = true },
        //        Project = new Project { Name = "InfraProfile" }
        //    };
        //    var status = identity.GetProjectToken();
        //    if (status == false)
        //        return false;

        //    ProfileJsonWebToken = identity.Token.StringValue;
        //    return true;
        //}

        //ProjectValues
        public static string ProjectName { private set; get; } = "InquiryWebCrawler";

        public static string[] UserAgents { set; get; }
        public static dynamic GasWebProxy { set; get; }
        public static Random Random { private set; get; }
        public static dynamic GasBindIp { private set; get; }
        public static Queue<string> GasIpPool { private set; get; }



        public static bool InitOtherObjects()
        {
            Random = new Random();
            //new WebProxy("185.8.174.146:8080") { Credentials = new NetworkCredential("user", "1qaz2wsx") };
            //new WebProxy("79.175.133.249:8080") { Credentials = new NetworkCredential("user", "1qaz2wsx") };

            UserAgents = new string[]{
                "Mozilla/5.0 (Linux; Android 7.0; SM-G892A Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/60.0.3112.107 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.0; SM-G930VC Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/58.0.3029.83 Mobile Safari/537.36Mozilla/5.0 (Linux; Android 6.0.1; SM-G935S Build/MMB29K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/55.0.2883.91 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0.1; SM-G920V Build/MMB29K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.98 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 5.1.1; SM-G928X Build/LMY47X) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0.1; Nexus 6P Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.83 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.1.1; G8231 Build/41.2.A.0.219; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/59.0.3071.125 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0.1; E6653 Build/32.2.A.0.253) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.98 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0; HTC One X10 Build/MRA58K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/61.0.3163.98 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0; HTC One M9 Build/MRA58K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.98 Mobile Safari/537.3",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.34 (KHTML, like Gecko) Version/11.0 Mobile/15A5341f Safari/604.1",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A5370a Safari/604.1",
                "Mozilla/5.0 (iPhone9,3; U; CPU iPhone OS 10_0_1 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) Version/10.0 Mobile/14A403 Safari/602.1",
                "Mozilla/5.0 (iPhone9,4; U; CPU iPhone OS 10_0_1 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) Version/10.0 Mobile/14A403 Safari/602.1",
                "Mozilla/5.0 (Apple-iPhone7C2/1202.466; U; CPU like Mac OS X; en) AppleWebKit/420+ (KHTML, like Gecko) Version/3.0 Mobile/1A543 Safari/419.3",
                "Mozilla/5.0 (Windows Phone 10.0; Android 6.0.1; Microsoft; RM-1152) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Mobile Safari/537.36 Edge/15.15254",
                "Mozilla/5.0 (Windows Phone 10.0; Android 4.2.1; Microsoft; RM-1127_16056) AppleWebKit/537.36(KHTML, like Gecko) Chrome/42.0.2311.135 Mobile Safari/537.36 Edge/12.10536",
                "Mozilla/5.0 (Windows Phone 10.0; Android 4.2.1; Microsoft; Lumia 950) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Mobile Safari/537.36 Edge/13.1058",
                "Mozilla/5.0 (Linux; Android 7.0; Pixel C Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/52.0.2743.98 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0.1; SGP771 Build/32.2.A.0.253; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/52.0.2743.98 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0.1; SHIELD Tablet K1 Build/MRA58K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/55.0.2883.91 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.0; SM-T827R4 Build/NRD90M) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.116 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 5.0.2; SAMSUNG SM-T550 Build/LRX22G) AppleWebKit/537.36 (KHTML, like Gecko) SamsungBrowser/3.3 Chrome/38.0.2125.102 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 4.4.3; KFTHWI Build/KTU84M) AppleWebKit/537.36 (KHTML, like Gecko) Silk/47.1.79 like Chrome/47.0.2526.80 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 5.0.2; LG-V410/V41020c Build/LRX22G) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/34.0.1847.118 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246",
                "Mozilla/5.0 (X11; CrOS x86_64 8172.45.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.64 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_2) AppleWebKit/601.3.9 (KHTML, like Gecko) Version/9.0.2 Safari/601.3.9",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36",
                "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:15.0) Gecko/20100101 Firefox/15.0.1",
                "Mozilla/5.0 (CrKey armv7l 1.5.16041) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.0 Safari/537.36",
                "Roku4640X/DVP-7.70 (297.70E04154A)",
                "Mozilla/5.0 (Linux; U; Android 4.2.2; he-il; NEO-X5-116A Build/JDQ39) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Safari/534.30",
                "Mozilla/5.0 (Linux; Android 5.1; AFTS Build/LMY47O) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/41.99900.2250.0242 Safari/537.36",
                "Dalvik/2.1.0 (Linux; U; Android 6.0.1; Nexus Player Build/MMB29T)",
                "AppleTV6,2/11.1",
                "AppleTV5,3/9.1.1"
            };


            GasWebProxy = new { WebProxy = (string)null };

            GasBindIp = new { IpAddress = (string)null };

            //GasIpPool = new Queue<string>(  WebConfigurationManager.AppSettings["GasIpPool"].Split(','));
            GasIpPool = new Queue<string>();
            GasIpPool.Enqueue("130.185.76.36");

            return true;
        }

        public static string GetRandomUserAgent()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var userAgent = ProjectValues.UserAgents[Random.Next(UserAgents.Length)];

                return userAgent;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static IPAddress GetGasBindIp()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var temp = GasIpPool.Dequeue();
                GasIpPool.Enqueue(temp);
                var bindIpAddress = IPAddress.Parse(temp);

                return bindIpAddress;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}