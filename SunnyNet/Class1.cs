using System;
using System.Text;
using System.Net;
using System.IO;

namespace SunnyNet
{
    public class SunnyInterface
    {
        private CookieContainer _cookies = new CookieContainer();
        private const string HOST = "https://www.sunnyportal.com";
        private const string LOGIN = HOST + "/Templates/Start.aspx";
        private const string INVERTER = HOST + "/Templates/UserProfile.aspx";
        private const string DATE = HOST + "/FixedPages/InverterSelection.aspx";
        private const string VALUES = HOST + "/Templates/DownloadDiagram.aspx?down=diag";
        private const string DASH = HOST + "/Dashboard?t=";


        public void ClearCookies()
        {
            _cookies = new CookieContainer();
        }
        public void Connect(string user, string pass)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(LOGIN);

            var postData = "ctl00$ContentPlaceHolder1$Logincontrol1$txtUserName=" + user;
            postData += "&ctl00$ContentPlaceHolder1$Logincontrol1$txtPassword=" + pass;
            postData += "&__EVENTTARGET=" + "ctl00$ContentPlaceHolder1$Logincontrol1$LoginBtn";

            var data = Encoding.ASCII.GetBytes(postData);
            request.CookieContainer = _cookies;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.AllowAutoRedirect = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        public void openInverter()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(INVERTER);

            var postData = "__EVENTTARGET=" + "ctl00$NavigationLeftMenuControl$0_6";

            var data = Encoding.ASCII.GetBytes(postData);
            request.CookieContainer = _cookies;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            request.AllowAutoRedirect = true;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        public string[] requestValues()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(VALUES);
            request.Method = "GET";
            request.AllowAutoRedirect = true;
            request.CookieContainer = _cookies;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();

            /*byte[] buffer = new byte[1024];
            int bytesRead = 0;

            FileStream fileStream = File.Create("Data.csv");
            while((bytesRead = stream.Read(buffer, 0, 1024)) != 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();*/
            int lineNum = 0;

            var read = new StreamReader(stream);
            /* double[] vs = new double[96];
             int lineNum = 0;
             while(!read.EndOfStream)
             {
                 lineNum++;
                 string line = read.ReadLine();
                 string[] values = line.Split(';');
                 if(lineNum >1)
                 {
                     if (values[1] == "")
                         values[1] = "0";

                     vs[lineNum - 2] = Convert.ToDouble(values[1]);

                 }
             }
             return vs;*/
            string[] vs = new string[96];
            while (!read.EndOfStream)
            {
                lineNum++;
                string line = read.ReadLine();
                if (lineNum > 1)
                {

                    vs[lineNum - 2] = line.Split(';')[1];
                }
            }
            Array.Resize(ref vs, lineNum - 1);
            return vs;



        }

        public void requestValuesFile(string name)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(VALUES);
            request.Method = "GET";
            request.AllowAutoRedirect = true;
            request.CookieContainer = _cookies;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();

            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            string sep = "sep=;" + Environment.NewLine;

            FileStream fileStream = File.Create(name + ".csv");

            fileStream.Write(Encoding.ASCII.GetBytes(sep), 0, Encoding.ASCII.GetByteCount(sep));
            while ((bytesRead = stream.Read(buffer, 0, 1024)) != 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

        }
        public int getWatts()
        {
            Int32 unixTT = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DASH + unixTT);
            request.Method = "GET";
            request.AllowAutoRedirect = true;
            request.CookieContainer = _cookies;
            request.Timeout = 5000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";



            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamer = response.GetResponseStream();

            var reader = new StreamReader(response.GetResponseStream());


            string objText = reader.ReadToEnd();
            objText = objText.Replace("\"", "");
            objText = objText.Replace("\\", "");
            string[] info = objText.Split(',');
            string[,] data = new string[info.Length, 2];
            for (int i = 0; i < 5; i++)
            {
                data[i, 0] = info[i + 4].Split(':')[0];
                data[i, 1] = info[i + 4].Split(':')[1];
            }

            return Convert.ToInt32(data[0, 1]);

        }
        public void setDateTotal()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DATE);

            var postData = "__EVENTTARGET=" + "ctl00$ContentPlaceHolder1$UserControlShowInverterSelection1$LinkButton_TabBack3";
            postData += "&ctl00$ContentPlaceHolder1$UserControlShowInverterSelection1$_datePicker$textBox=" + "01/01/2016";
            postData += "&ctl00$HiddenPlantOID" + "4cb822d1-dea7-40c3-890f-c9be09c6d490";

            var data = Encoding.ASCII.GetBytes(postData);
            request.CookieContainer = _cookies;
            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        public void setDateYear(string year)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DATE);

            var postData = "__EVENTTARGET=" + "ctl00$ContentPlaceHolder1$UserControlShowInverterSelection1$LinkButton_TabBack2";
            postData += "&ctl00$ContentPlaceHolder1$UserControlShowInverterSelection1$_datePicker$textBox=" + "01/01/" + year;
            postData += "&ctl00$HiddenPlantOID" + "4cb822d1-dea7-40c3-890f-c9be09c6d490";

            var data = Encoding.ASCII.GetBytes(postData);
            request.CookieContainer = _cookies;
            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        public void setDateMonth(string month, string year)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DATE);

            var postData = "__EVENTTARGET=" + "ctl00$ContentPlaceHolder1$UserControlShowInverterSelection1$LinkButton_TabBack1";
            postData += "&ctl00$ContentPlaceHolder1$UserControlShowInverterSelection1$_datePicker$textBox=" + month + "/01/" + year;
            postData += "&ctl00$HiddenPlantOID" + "4cb822d1-dea7-40c3-890f-c9be09c6d490";

            var data = Encoding.ASCII.GetBytes(postData);
            request.CookieContainer = _cookies;
            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public void setDate(string day, string month, string year)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DATE);

            var postData = "__EVENTTARGET=" + "ctl00$ContentPlaceHolder1$UserControlShowInverterSelection1$LinkButton_TabFront3";
            postData += "&ctl00$ContentPlaceHolder1$UserControlShowInverterSelection1$_datePicker$textBox=" + month + "/" + day + "/" + year;
            postData += "&ctl00$HiddenPlantOID" + "4cb822d1-dea7-40c3-890f-c9be09c6d490";

            var data = Encoding.ASCII.GetBytes(postData);
            request.CookieContainer = _cookies;
            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}

