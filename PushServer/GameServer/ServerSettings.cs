using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Net;
using System.Net.NetworkInformation;
using System.Xml;

namespace ArGateway
{
    public class cServerSettings
    {
        private const string LISTENPORTTAG = "GAME_PORT";
        private const string DBIPTAG = "DB_IP";
        private const string DBNAMETAG = "DB_NAME";
        private const string DBIDTAG = "DB_LOGINID";
        private const string DBPWDTAG = "DB_LOGINPWD";
        private const string DBPORTTAG = "DB_PORT";
        private const string HEARTBEATTAG = "HEARTBEAT";

        private const string m_strXMLFile = "ArGatewayConfig.xml";

        public const ushort GAMEPORT_MIN = 10000;
        public const ushort GAMEPORT_MAX = 65535;

        private static cServerSettings m_instance = null;

        private IPAddress m_mssqlip;
        public IPAddress DBIP
        {
            get
            {
                return m_mssqlip;
            }
            set
            {
                m_mssqlip = value;
            }
        }

        private ushort m_listenport;        // ����Ʈ���̼����� �����Ʈ
        public ushort ListenPort
        {
            get
            {
                return m_listenport;
            }
            set
            {
                if (value >= GAMEPORT_MIN && value <= GAMEPORT_MAX)
                {
                    m_listenport = value;
                }
            }
        }

        private string m_strDBName;
        /// <summary>
        /// ���ε���(�������� �� ���������� �����Ǵ� ���)
        /// </summary>
        public string DBName
        {
            get
            {
                return m_strDBName;
            }
            set
            {
                m_strDBName = value;
            }
        }

        private string m_dbid;              // ������Ӿ��̵�
        public string DBID
        {
            get
            {
                return m_dbid;
            }
            set
            {
                m_dbid = value;
            }
        }

        private string m_strDBPwd;             // ������Ӿ�ȣ
        public string DBPwd
        {
            get
            {
                return m_strDBPwd;
            }
            set
            {
                m_strDBPwd = value;
            }
        }

        private ushort m_nDBPort;            // ���������Ʈ
        public ushort DBPort
        {
            get
            {
                return m_nDBPort;
            }
            set
            {
                if (value >= 1433)
                {
                    m_nDBPort = value;
                }
            }
        }

        private bool m_bHeartBeat;           // Ŭ���̾�Ʈ�� ���� ��Ʈ��Ʈ�˻翩��
        public bool HeartBeat
        {
            get
            {
                return m_bHeartBeat;
            }
            set
            {
                m_bHeartBeat = value;
            }
        }

        public cServerSettings()
        {
            m_listenport = 13003; // default port number of Server
            m_mssqlip = this.GetNetworkIPAddress();
            m_strDBName = "127.0.0.1";
            m_dbid = "sa";
            m_strDBPwd = "sa";
            m_nDBPort = 1433;
            m_bHeartBeat = true;
        }

        public static cServerSettings GetInstance()
        {
            if (m_instance == null)
            {
                m_instance = new cServerSettings();
            }
            return m_instance;
        }

        /// <summary>
        /// ��Ʈ��ī�忡 �Ҵ��� �������ּҸ� ��´�.
        /// </summary>
        /// <returns>ī�忡 �Ҵ�� IPAddress������Ʈ(���� ��� Loopback�ּҰ� �ȴ�.)</returns>
        public IPAddress GetNetworkIPAddress()
        {
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                if (nics == null || nics.Length < 1)
                {
                    return IPAddress.Loopback;
                }
                NetworkInterface adapter = nics[0]; // ù��° ��ġ�� �����Ѵ�.
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
                if (uniCast != null)
                {
                    if (uniCast.Count > 0)
                    {
                        return uniCast[0].Address;
                    }
                }
                return IPAddress.Loopback;
            }
            catch (Exception)
            {
                return IPAddress.Loopback;
            }
        }

        // ����: �������Ϸκ��� �������� �о �ش� ���������� �����Ѵ�.
        public void LoadSettings()
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(m_strXMLFile);
                XmlNodeList xmlNodList = xmlDoc.GetElementsByTagName("config");
                XmlNode xmlNode = xmlNodList.Item(0);
                XmlNode node = xmlNode.FirstChild;
                while (node != null)
                {
                    switch (node.Name)
                    {
                        case DBIPTAG:
                            DBIP = IPAddress.Parse(node.InnerText);
                            break;
                        case LISTENPORTTAG:
                            ListenPort = Convert.ToUInt16(node.InnerText);
                            break;
                        case DBNAMETAG:
                            m_strDBName = node.InnerText;
                            break;
                        case DBIDTAG:
                            m_dbid = node.InnerText;
                            break;
                        case DBPWDTAG:
                            m_strDBPwd = node.InnerText;
                            break;
                        case DBPORTTAG:
                            DBPort = Convert.ToUInt16(node.InnerText);
                            break;
                        case HEARTBEATTAG:
                            m_bHeartBeat = Convert.ToBoolean(node.InnerText);
                            break;
                    }
                    node = node.NextSibling;
                }
            }
            catch (Exception)
            {

            }
        }

        // ����: ���������� �������Ͽ� �����Ѵ�.
        // �����: TRUE ����, FALSE ����
        public bool SaveSetting()
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml("<config/>");
                XmlElement root = xmlDoc.DocumentElement;
                string strTemp;

                XmlDocumentFragment docFrag = xmlDoc.CreateDocumentFragment();

                strTemp = string.Format("<{0}>{1}</{2}>", LISTENPORTTAG, m_listenport, LISTENPORTTAG);
                docFrag.InnerXml = strTemp;
                root.AppendChild(docFrag);

                strTemp = string.Format("<{0}>{1}</{2}>", DBIPTAG, m_mssqlip, DBIPTAG);
                docFrag.InnerXml = strTemp;
                root.AppendChild(docFrag);

                strTemp = string.Format("<{0}>{1}</{2}>", DBNAMETAG, m_strDBName, DBNAMETAG);
                docFrag.InnerXml = strTemp;
                root.AppendChild(docFrag);

                strTemp = string.Format("<{0}>{1}</{2}>", DBIDTAG, m_dbid, DBIDTAG);
                docFrag.InnerXml = strTemp;
                root.AppendChild(docFrag);

                strTemp = string.Format("<{0}>{1}</{2}>", DBPWDTAG, m_strDBPwd, DBPWDTAG);
                docFrag.InnerXml = strTemp;
                root.AppendChild(docFrag);

                strTemp = string.Format("<{0}>{1}</{2}>", DBPORTTAG, m_nDBPort, DBPORTTAG);
                docFrag.InnerXml = strTemp;
                root.AppendChild(docFrag);

                strTemp = string.Format("<{0}>{1}</{2}>", HEARTBEATTAG, m_bHeartBeat, HEARTBEATTAG);
                docFrag.InnerXml = strTemp;
                root.AppendChild(docFrag);

                xmlDoc.Save(m_strXMLFile);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
