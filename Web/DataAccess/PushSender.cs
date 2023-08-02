using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Net.Security;
using System.Collections.Specialized;
using System.Net.Sockets;
using System.Web;

namespace DataAccess
{
    public class PushSender
    {
        public const int DEVICE_ANDROID = 0;
        public const int DEVICE_IOS = 1;

        private string mAndroidUrl = "";
        private string mAndroidAppID = "";
        private string mIOSHostName = "";
        private int mIOSPort = 0;
        private string mIOSCertPath = "";
        private string mIOSCertPassword = "";

        public PushSender()
        {
        }
        public PushSender(string strAndroidUrl, string strAndroidAppID, string strIOSHostName, int iIOSPort, string strIOSCertPath, string strIOSCertPassword)
        {
            Init(strAndroidUrl, strAndroidAppID, strIOSHostName, iIOSPort, strIOSCertPath, strIOSCertPassword);
        }
        public void Init(string strAndroidUrl, string strAndroidAppID, string strIOSHostName, int iIOSPort, string strIOSCertPath, string strIOSCertPassword)
        {
            mAndroidUrl = strAndroidUrl;
            mAndroidAppID = strAndroidAppID;
            mIOSHostName = strIOSHostName;
            mIOSPort = iIOSPort;
            mIOSCertPath = strIOSCertPath;
            mIOSCertPassword = strIOSCertPassword;
        }

        public bool Send(int iType, string strDeviceID, string strMsg)
        {
            if ((iType != DEVICE_ANDROID && iType != DEVICE_IOS) ||
                string.IsNullOrEmpty(strDeviceID) ||
                string.IsNullOrEmpty(strMsg))
                return false;

            if (iType == DEVICE_ANDROID)
                return SendToAndroid(strDeviceID, System.Uri.EscapeUriString(strMsg));
            else if (iType == DEVICE_IOS)
                return SendToIOS(strDeviceID, strMsg);

            return false;
        }

        private bool SendToAndroid(string strDeviceID, string strMsg)
        {
            try
            {
                //WriteLog(strDeviceID + "----------------------------" +strMsg);

                WebRequest tRequest = WebRequest.Create(mAndroidUrl);
                tRequest.Method = "POST";
                tRequest.ContentType = " application/x-www-form-urlencoded;charset=utf-8";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", mAndroidAppID));

                string strCollapseKey = Guid.NewGuid().ToString("n");
                string strPostData = string.Format("registration_id={0}&data.payload={1}&collapse_key={2}", strDeviceID, strMsg, strCollapseKey);

                //string strPostData = string.Format("registration_id={0}&data.payload={1}", strDeviceID, strMsg);

                Byte[] byteArray = Encoding.UTF8.GetBytes(strPostData);
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                String sResponseFromServer = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }
        protected bool SendToIOS(string strDeviceID, string strMsg)
        {
            try
            {
                //X509Certificate2 clientCertificate = new X509Certificate2(mIOSCertPath, mIOSCertPassword);
                X509Certificate2 clientCertificate = new X509Certificate2(mIOSCertPath, mIOSCertPassword, X509KeyStorageFlags.MachineKeySet);
                X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);
                TcpClient client = new TcpClient(mIOSHostName, mIOSPort);
                SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                try
                {
                    sslStream.AuthenticateAsClient(mIOSHostName, certificatesCollection, SslProtocols.Tls, true);
                }
                catch (Exception e)
                {
                    client.Close();
                    throw (e);
                }
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);
                writer.Write((byte)0);  //The command
                writer.Write((byte)0);  //The first byte of the deviceId length (big-endian first byte)
                writer.Write((byte)32); //The deviceId length (big-endian second byte)
                writer.Write(HexStringToByteArray(strDeviceID.ToUpper()));
                String strPayLoad = "{\"aps\":{\"alert\":\"" + strMsg + "\",\"badge\":0,\"sound\":\"default\"}}";

                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(strPayLoad);
                byte[] payloadSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(b1.Length)));
                writer.Write(payloadSize);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Close();
            }
            catch(Exception e)
            {
                throw (e);
                //return false;
            }
            return true;
        }

        public bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public byte[] HexStringToByteArray(String s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }
            return buffer;
        }

        public void WriteLog(string msg)
        {
            // web.congif 에 TEST_LOG의 값이 있음
            // <add key="TEST_LOG" value="D:\경로\경로\폴더\"/>
            string slogFilePath = "E:\\work\\colorAR\\web\\logs\\";
            string sDateTime = System.DateTime.Now.ToString();
            string sFileName = System.DateTime.Now.ToString("yyyyMMdd") + ".LOG";

            System.IO.StreamWriter sw = new System.IO.StreamWriter(slogFilePath + sFileName, true, System.Text.Encoding.GetEncoding("ks_c_5601-1987"));
            sw.AutoFlush = true;


            sw.WriteLine(sDateTime + " - Message: " + msg);
            sw.WriteLine("---------------------------------------------------------------------------");

            sw.Flush();
            sw.Close();
        }

    }
}