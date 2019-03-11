using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CaptchaTest
{
    public partial class DataBaseConfigure
    {
        public int CommandTimeout { set; get; }
        public string ConnectionString { set; get; }


        public bool GetDataBaseConfigure()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                //var encryptedText = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dataBase.config"));
                var encryptedText = @"0SuaF2y2ST4sGEjTpOPXSdkdIhvef1LieL3wmsXuHBcsJLAmrF/899+7qTTKIGJaBfrm3X5ukAz1sXC+9R6d0EhUYcrBNRjJ62T6DOY6fvHdMil2I5F5Xt4F3BLzt1JcBYxzTMdMJc6wh2d5ERgWhpuk8ZisV1TEisQaAAXkQ6R8HyCo6o3hs5thjxdHt8Z4EfHlCSwqVThG+Q/cIttnIzkmHy3QlICx4Dj+7CgUbP+OWJPE4GBT/yJryoJTWy2DE3VamIABgKM8Au2hqTZphQK81wXVteFVYoI4RAKfCUxJPQkEJVa+rCO7L3n6B/xCcCM2YaMptgLjTLivxAXljXo3BgG4SQBgWhQYvc3aszI5/4RJrLexfh5rz5UziUxgFSiHGdlYKow3USIiY8ElVFViBSXBoejCvki6UM64s7IfYMuWfxUwGQ4jQaPvfFHTPqbNjRYDkwRBs9p5oJAz4TNO4scaNbvPwR6HW4ayIsyx01R+JPXdoEXQ5RX+6GLhYvVQ5jss2v1oW8yL4R5rwElYM88klCkb6/yakI9oLc34Acs34SeWyruH8SdR5MngM+pZrJXs/1DA+qQywzsg6g==";

             var decryptedText = Helper.Decrypt(encryptedText);

                var configure = XDocument.Parse(decryptedText);
                ConnectionString = configure.Root.XPathSelectElement("/Configure/ConnectionString").Value;
                CommandTimeout = Convert.ToInt32(configure.Root.XPathSelectElement("/Configure/CommandTimeout").Value);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}