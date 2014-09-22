using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hal.CookieGetterSharp;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Net.Sockets;
using System.Xml;
using System.Globalization;

namespace croozshinbun
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            comboBox1.Items.AddRange(CookieGetter.CreateInstances(true));
        }

        private Form2 f2;
        private Mutex mt = new Mutex();

        private bool threadswitch = false;

        private CookieContainer m_cc;//クッキーコンテナ
        private HttpWebRequest req;
        private Cookie ck;

        private string croozlvnum = "";
        private string kobetulvnum = "";
        private string MAI_SEND = "<thread thread=\"{0}\" version=\"{1}\" res_from=\"-{2}\"/>\0";
        //private string COM_SEND = "<chat thread=\"{0}\" ticket=\"{1}\" vpos=\"{2}\" postkey=\"{3}\" mail=\"{4}\" user_id=\"{5}\" premium=\"{6}\">{7}</chat>\0";
        private string COM_URL = "http://com.nicovideo.jp/community/";

        //コメントサーバ情報
        private string m_ComSrvAddr = "";//コメントサーバのアドレス
        private int m_ComSrvPort;//コメントサーバのポート
        private string m_ComSrvThread = "";//スレッドID

        //private string m_ticket = "";
        //private string m_vpos = "";
        //private string m_postkey = "";
        //private string m_mail = "";
        //private string m_premium = "";
        //private string m_locale = "";
        //private string m_server_time = "";
        //private int m_post100 = 0;

        //private int last_num = 0;
        //private int now_num = 0;

        private byte[] resBytes = new byte[1048576];

        private int mMillSec = 1000;//スレッドms
        private string th_res_from = "10";
        private string th_ver = "20061206";

        //private string m_start_time = "";//開始時刻                

        private Color[] cls = new Color[8] { Color.White, Color.Black, Color.Red, Color.Blue, Color.Yellow, Color.Green, Color.Orange, Color.Pink };
        private delegate void dldl();

        private static string[] expectedFormats = {"yyyy年MM月dd日"};

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.burauza = comboBox1.SelectedIndex;
            Properties.Settings.Default.Save();
            if (comboBox1.SelectedIndex > -1)
            {
                burauzackget();
            }
        }
        private void burauzackget()
        {
            mt.WaitOne();

            if (comboBox1.SelectedIndex > -1)
            {
                Properties.Settings.Default.burauza = comboBox1.SelectedIndex;
                Properties.Settings.Default.Save();

                ck = CookieGetter.CreateInstances(true)[comboBox1.SelectedIndex].GetCookie(new Uri("http://www.nicovideo.jp/"), "user_session");

                if (ck == null)
                {
                    MessageBox.Show("クッキーを取得できませんでした。\r\nブラウザでログインし直すか、ブラウザを再起動したり\r\n時間を置くなどして再度お試し下さい。");
                    button1.Enabled = false;
                }
                else
                {
                    m_cc = new CookieContainer();
                    m_cc.Add(ck);
                    button1.Enabled = true;
                }
            }

            mt.ReleaseMutex();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            comboBox1.Enabled = false;

            req = (HttpWebRequest)WebRequest.Create("http://live.nicovideo.jp/cruise");
            req.CookieContainer = m_cc;//取得済みのクッキーコンテナ

            using (StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
            {//クルーズ放送番号取得
                croozlvnum = new Regex(@"http://live.nicovideo.jp/watch/lv\d+").Match(sr.ReadToEnd()).Value.Replace("http://live.nicovideo.jp/watch/", "");
            }

            //初期値取得
            //comcom(String.Format(MAI_SEND, m_ComSrvThread, th_ver, th_res_from));
            try
            {
                req = (HttpWebRequest)WebRequest.Create("http://live.nicovideo.jp/api/getplayerstatus?v=" + croozlvnum);
                req.CookieContainer = m_cc;//取得済みのクッキーコンテナ
                //XML解析
                XmlDocument xdoc = new XmlDocument();
                using (StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    xdoc.LoadXml(sr.ReadToEnd());
                }

                //サーバ情報
                XmlNodeList ms = xdoc.DocumentElement.GetElementsByTagName("ms");
                m_ComSrvAddr = ms[0].ChildNodes[0].InnerText;
                m_ComSrvPort = int.Parse(ms[0].ChildNodes[1].InnerText);
                m_ComSrvThread = ms[0].ChildNodes[2].InnerText;

                f2 = new Form2();
                f2.Show();
                f2.Refresh();
                f2.rend();
                
                //コメント読込開始
                threadswitch = true;
                Thread th = new Thread(comentloop);
                th.IsBackground = true;
                th.Start();                
            }
            catch
            {
            }

            button2.Enabled = true;
        }

        private void comentloop()
        {
            while (threadswitch)
            {
                Invoke(new dldl(delegate
                {
                    mt.WaitOne();
                    comcom(String.Format(MAI_SEND, m_ComSrvThread, th_ver, th_res_from));
                    mt.ReleaseMutex();
                }));
                Thread.Sleep(mMillSec);
            }
        }

        private void comcom(string bun)
        {
            int resSize = 0;
            //Socketの作成
            using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                //接続
                sock.Connect(m_ComSrvAddr, m_ComSrvPort);

                //以下、送信処理
                byte[] puchData = Encoding.UTF8.GetBytes(bun);
                sock.Send(puchData, puchData.Length, SocketFlags.None);

                resSize = sock.Receive(resBytes, resBytes.Length, SocketFlags.None);
            }

            if (resSize != 0)
            {
                string[] stArrayData = Encoding.UTF8.GetString(resBytes, 0, resSize).Replace('\0', '\n').Split('\n');
                XmlDocument xmlDoc = new XmlDocument();
                for (int i = 0; i < stArrayData.Length; i++)
                {
                    bool sippai = true;
                    //try
                    //{
                    try
                    {
                        xmlDoc.LoadXml(stArrayData[i]);
                    }
                    catch { sippai = false; }

                    if (sippai)
                    {
                        if (xmlDoc.DocumentElement.Name == "chat")
                        {
                            if ("900000000" == xmlDoc.DocumentElement.GetAttribute("user_id"))
                            {
                                if (new Regex("^/crpanel start").Match(xmlDoc.DocumentElement.InnerText).Success)
                                {
                                    //放送番号取得
                                    kobetulvnum = new Regex(@"lv\d+").Match(xmlDoc.DocumentElement.InnerText).Value;

                                    try
                                    {
                                        req = (HttpWebRequest)WebRequest.Create("http://live.nicovideo.jp/api/getplayerstatus?v=" + kobetulvnum);
                                        req.CookieContainer = m_cc;//取得済みのクッキーコンテナ
                                        //XML解析
                                        XmlDocument xdoc = new XmlDocument();
                                        using (StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                                        {
                                            xdoc.LoadXml(sr.ReadToEnd());
                                        }

                                        //番組情報
                                        XmlNodeList stream = xdoc.DocumentElement.GetElementsByTagName("stream");

                                        foreach (XmlNode node in stream[0].ChildNodes)
                                        {
                                            if (node.Name == "default_community")
                                            {//コミュ番号
                                                f2.p_community = node.InnerText;
                                            }
                                            if (node.Name == "owner_name")
                                            {//オーナー名前
                                                f2.labelusername.Text = node.InnerText;
                                            }
                                            if (node.Name == "watch_count")
                                            {//来場者数
                                                f2.p_watch_count = node.InnerText;
                                            }
                                            if (node.Name == "comment_count")
                                            {//総コメント数
                                                f2.p_comment_count = node.InnerText;
                                            }
                                        }

                                        XmlNodeList marquee = xdoc.DocumentElement.GetElementsByTagName("marquee");

                                        foreach (XmlNode node in marquee[0].ChildNodes)
                                        {
                                            if (node.Name == "category")
                                            {//カテゴリ
                                                f2.p_cate = node.InnerText;
                                            }
                                        }

                                        StreamReader stre = new StreamReader(WebRequest.Create(COM_URL + f2.p_community).GetResponse().GetResponseStream(), Encoding.UTF8);
                                        string html = stre.ReadToEnd();
                                        stre.Close();

                                        //コミュ名
                                        f2.comname = new Regex("<h1 id=\"community_name\">.+</h1>", RegexOptions.Singleline).Match(html).Value.Replace("<h1 id=\"community_name\">", "").Replace("</h1>", "");
                                        //開設日
                                        f2.kaisetubi = new Regex("<p>開設日：<strong>.+</strong></p>", RegexOptions.Singleline).Match(html).Value.Replace("<p>開設日：<strong>", "").Replace("</strong></p>", "");
                                        //年齢
                                        DateTime temp;
                                        if (DateTime.TryParseExact(f2.kaisetubi, expectedFormats, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out temp))
                                        {
                                            f2.labelage.Text = (new DateTime((DateTime.Now - temp).Ticks).Year - 1).ToString();
                                        }
                                        else
                                        {
                                            f2.labelage.Text = "年齢不詳";
                                        }

                                        //レベル
                                        f2.labellevel.Text = new Regex("<strong>[0-9]+</strong> …", RegexOptions.Singleline).Match(html).Value.Replace("<strong>", "").Replace("</strong> …", "");

                                        int levelnum = int.Parse(f2.labellevel.Text);
                                        for (int l = 1; l <= 8; l++)
                                        {
                                            if (levelnum <= l * 32)
                                            {
                                                f2.labellevel.BackColor = cls[l - 1];
                                                if (l == 1 || l == 5)
                                                {
                                                    f2.labellevel.ForeColor = Color.Black;
                                                }
                                                else
                                                {
                                                    f2.labellevel.ForeColor = Color.White;
                                                }
                                                break;
                                            }
                                        }
                                        //メンバー数
                                        f2.menba = new Regex("<strong>[0-9]+</strong>人", RegexOptions.Singleline).Match(html).Value.Replace("<strong>", "").Replace("</strong>人", "");
                                        //累計来場者数
                                        f2.ruikeiraijo = new Regex("<strong>[0-9]+</strong>", RegexOptions.Singleline).Match(Regex.Split(html, "累計来場者数")[1]).Value.Replace("<strong>", "").Replace("</strong>", "");

                                        f2.rend();
                                        f2.label1.Text = f2.p_community + "\r\n"
                                            + "カテゴリ:" + f2.p_cate + "\r\n"
                                            + "来場者数:" + f2.p_watch_count + "\r\n"
                                            + "総コメント数:" + f2.p_comment_count + "\r\n"
                                            + "開設日:" + f2.kaisetubi + "\r\n"
                                            + "メンバー数:" + f2.menba + "\r\n"
                                            + "累計来場者数:" + f2.ruikeiraijo;

                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            threadswitch = false;

            mt.WaitOne();
            f2.Close();
            f2.Dispose();
            button1.Enabled = true;
            comboBox1.Enabled = true;
            mt.ReleaseMutex();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://com.nicovideo.jp/community/co1768971");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.nicovideo.jp/user/27036634");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://com.nicovideo.jp/community/co1605705");
        }
        
    }
}
