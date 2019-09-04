using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace R_book
{
    public partial class book_Mian : Form
    {
        public book_Mian()
        {
            InitializeComponent();
            skinTabControl1.SelectedIndex = 2;
        }
        public string bookname = "";//书名

        public List<string> bookmllist_tj = new List<string>();//推荐 列表
        List<booklog> blog_tj = new List<booklog>();//推荐 列表

        public List<string> bookmllist_jp = new List<string>();//精品 列表
        List<booklog> blog_jp = new List<booklog>();//精品 列表

        public List<string> bookmllist_gx = new List<string>();//更新 列表
        List<booklog> blog_gx = new List<booklog>();//更新 列表 http://www.xiaoqiangxs.org/lawenxiaoshuo/5_9.html

        public List<string> bookmllist = new List<string>();//目录列表
        List<booklog> blog_mllist = new List<booklog>();//目录列表

        int pagecount = 0;
        int page = 0;

        string url = "http://www.xiaoqiangxs.org";

        public string bookmlurl = "http://www.xiaoqiangxs.org/lawenxiaoshuo/";

        WebToStream WTS = new WebToStream();

        private void GetTXT_Load(object sender, EventArgs e)
        {
            string blist = WTS.getWebData(bookmlurl);
          //  TextBox.Text = blist;
            if (blist != "")
            {
                getbooklist(blist);
                //bookmllist_tj
                foreach (string bn in bookmllist_tj)
                {
                    SkinListBoxItem lbei = new SkinListBoxItem();
                    lbei.Text = " " + bn;
                    Image image = Resource.book;
                    lbei.Image = image;
                    tjlistbox.Items.Add(lbei);
                }
                foreach (string bn in bookmllist_jp)
                {
                    SkinListBoxItem lbei = new SkinListBoxItem();
                    lbei.Text = " " + bn;
                    Image image = Resource.book;
                    lbei.Image = image;
                    jplistbox.Items.Add(lbei);
                }
             
                
            }
            else
            {
                this.Close();
            }
        }
        public void getbooklist(string html)
        {

            string[] strs = html.Split(new[] { "<div id=\"hotcontent\">", "<div id=\"newscontent\">", "<h2>好看的精品小说</h2>", "<div class=\"page_b page_b2\">喜欢就收藏我们</div>" }, StringSplitOptions.None);
            List<string> s = strs.ToList();
            // 推荐
            List<string> tjlist = s[1].Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            foreach (string tj in tjlist)
            {
                int p = tj.IndexOf("<div class=\"image\">");
                if (p <= 0)
                {
                    continue;
                }
                List<string> hl = tj.Split(new[] { "<div class=\"image\"><a href=\"", "\"><img", "alt=\"", "\"  width" }, StringSplitOptions.None).ToList();
                booklog tjt = new booklog();
                tjt.name = hl[3];
                tjt.url = hl[1];
                bookmllist_tj.Add(hl[3]);
                blog_tj.Add(tjt);
            }
            //精品
            List<string> jplist = s[3].Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            foreach (string jp in jplist)
            {
                int p = jp.IndexOf("http://www.xiaoqiangxs.org");
                if (p <= 0)
                {
                    continue;
                }
                List<string> hl = jp.Split(new[] { "href=\"", "\">", "</a>" }, StringSplitOptions.None).ToList();
                booklog jpt = new booklog();
                jpt.name = hl[3];
                jpt.url = hl[2];
                bookmllist_jp.Add(hl[3]);
                blog_jp.Add(jpt);
            }
            gxlistload(s[2]);
           // pagelabel.Text = s[2];
            string pagetext = s[2].Split(new string[] { "<em id=\"pagestats\">", "</em>" }, StringSplitOptions.None).ToList()[1];
            string[] paget = pagetext.Split('/');
            pagecount = int.Parse(paget[1]);
            page = int.Parse(paget[0]);
            pagelabel.Text = "第" + page + "页 共" + pagecount + "页";

        }

        private void gxlistload(string gxtext)
        {
            blog_gx.Clear();
            bookmllist_gx.Clear();
            gxlistbox.Items.Clear();
            int gxul = gxtext.IndexOf("</ul>");
            if (gxul > 0)
            {
                gxtext = gxtext.Substring(0, gxul);
                //TextBox.Text = gxtext;
            }
            List<string> gxlist = gxtext.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            foreach (string gx in gxlist)
            {
                int p = gx.IndexOf("http://www.xiaoqiangxs.org");
                if (p <= 0)
                {
                    continue;
                }
                List<string> hl = gx.Split(new[] { "href=\"", "\" target=\"_blank\">", "</a>" }, StringSplitOptions.None).ToList();
                booklog gxt = new booklog();
                gxt.name = hl[2];
                gxt.url = hl[1];
                bookmllist_gx.Add(hl[2]);
                blog_gx.Add(gxt);
            }
            foreach (string bn in bookmllist_gx)
            {
                SkinListBoxItem lbei = new SkinListBoxItem();
                lbei.Text = " " + bn;
                Image image = Resource.book;
                lbei.Image = image;
                gxlistbox.Items.Add(lbei);
            }
        }

        private void getmllist(string bookurl)
        {
            bookmllist.Clear();
            mululistBox.Items.Clear();
            blog_mllist.Clear();
            string mltext = WTS.getWebData(bookurl);
            List<string> mlhl = mltext.Split(new[] { "<dl>", "</dl>" }, StringSplitOptions.None).ToList();
            mltext = mlhl[1].Replace(" ", "");
            List<string> tjlist = mltext.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            foreach (string h in tjlist)
            {
                int p = h.IndexOf("<ahref=\"");
                if (p <= 0)
                {
                    continue;
                }
                List<string> hl = h.Split(new[] { "<ahref=\"", "\">", "</a>" }, StringSplitOptions.None).ToList();
                booklog jpt = new booklog();
                jpt.name = hl[2];
                jpt.url = hl[1];
                bookmllist.Add(hl[2]);
                blog_mllist.Add(jpt);
               
               // Image image = Resource.mulu;
                //lbei.Image = image;
                mululistBox.Items.Add(" " + hl[2]);
            }
           
        }
        private void Tjlistbox_MouseClick(object sender, MouseEventArgs e)
        {
            int index = tjlistbox.IndexFromPoint(e.X, e.Y);
            if (index < 0)
            {
                return;
            }
            b_name.Text = blog_tj[index].name;
            b_url.Text = blog_tj[index].url;
            getmllist(blog_tj[index].url);
        }

        private void Jplistbox_MouseClick(object sender, MouseEventArgs e)
        {
            int index = jplistbox.IndexFromPoint(e.X, e.Y);
            if (index < 0)
            {
                return;
            }
            b_name.Text = blog_jp[index].name;
            b_url.Text = blog_jp[index].url;
            getmllist(blog_jp[index].url);
        }

        private void Gxlistbox_MouseClick(object sender, MouseEventArgs e)
        {
            int index = gxlistbox.IndexFromPoint(e.X, e.Y);
            if (index < 0)
            {
                return;
            }
            b_name.Text = blog_gx[index].name;
            b_url.Text = blog_gx[index].url;
            getmllist(blog_gx[index].url);
        }

     
        public string getpagetxt(string burl)
        {
            string txturl = url + burl;
            string pageHtml = WTS.getWebData(txturl);
            string start = "<div id=\"content\">";
            string end = "<div class=\"bottem2\">";
            var startIndex = pageHtml.IndexOf(start);
            var endIndex = pageHtml.IndexOf(end);
            string temp = pageHtml.Substring(startIndex + 18, endIndex - startIndex - 18);
            temp = temp.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "      ").Replace("<br />", "").Replace("</div>", "");
            return temp;

        }
        List<booklog> down_mllist = new List<booklog>();//目录列表
        string downpath ="";
        string txtname = "";
        private void DownButton_Click(object sender, EventArgs e)
        {
            down_mllist.Clear();
            down_mllist = new List<booklog>(blog_mllist.ToArray());
            txtname = b_name.Text;
            downpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + Guid.NewGuid().ToString("N") + ".txt"; 
            downButton.Enabled = false;
            skinProgressBar.Value = 0;
            if (down_mllist.Count <= 0)
            {
                return;
            }
            wc.Text = "";
            Thread nonParameterThread = new Thread(new ThreadStart(downThread));
            nonParameterThread.Start();
        }
        public void downThread()
        {
            int s = down_mllist.Count;
            int p = 96 / s;
            int i = 0;
            File.AppendAllText(downpath,"《" + txtname + "》\r\n");
            File.SetAttributes(downpath, File.GetAttributes(downpath) | FileAttributes.Hidden);
            foreach (booklog ml in down_mllist)
            {
                i++;
                //NumberToChinese
                File.AppendAllText(downpath,"第"+ NumberToChinese(i) +"章    "+ ml.name + "\r\n\r\n" + getpagetxt(ml.url) + "\r\n");
                if (skinProgressBar.InvokeRequired)
                {
                   this.Invoke(new Action(() => skinProgressBar.Value += p));
                }
                else
                {
                   skinProgressBar.Value += p;
                }
            }
            //修改文件名
            string destFileName = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + txtname + ".txt";
            if (File.Exists(downpath))
            {
                File.Move(downpath, destFileName);
                File.SetAttributes(destFileName, File.GetAttributes(destFileName) & ~FileAttributes.Hidden);
            }
            if (wc.InvokeRequired)
            {
                this.Invoke(new Action(() => wc.Text = "已完成！"));
            
            }
            else
            {
                wc.Text = "已完成！";
            }
            if (skinProgressBar.InvokeRequired)
            {
                this.Invoke(new Action(() => skinProgressBar.Value = 100));
            }
            else
            {
                skinProgressBar.Value = 100;
            }
            if (downButton.InvokeRequired)
            {
                this.Invoke(new Action(() => downButton.Enabled = true));
            }
            
        }
        private void SsButton_Click(object sender, EventArgs e)//3_3233
        {
            string mltext = "";
            bookmllist.Clear();
            mululistBox.Items.Clear();
            blog_mllist.Clear();
            try
            {
                mltext = WTS.getWebData("http://www.xiaoqiangxs.org/" + ss_textBox.Text + "/");

            }
            catch (Exception ex)
            {
                MessageBox.Show("查无此书");
            }
            List<string> bkname = mltext.Split(new[] { "<h1>", "</h1>" }, StringSplitOptions.None).ToList();
            b_name.Text = bkname[1];
            b_url.Text = ss_textBox.Text;
            List<string> mlhl = mltext.Split(new[] { "<dl>", "</dl>" }, StringSplitOptions.None).ToList();
            mltext = mlhl[1].Replace(" ", "");
            List<string> tjlist = mltext.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            foreach (string h in tjlist)
            {
                int p = h.IndexOf("<ahref=\"");
                if (p <= 0)
                {
                    continue;
                }
                List<string> hl = h.Split(new[] { "<ahref=\"", "\">", "</a>" }, StringSplitOptions.None).ToList();
                booklog jpt = new booklog();
                jpt.name = hl[2];
                jpt.url = hl[1];
                bookmllist.Add(hl[2]);
                blog_mllist.Add(jpt);
                mululistBox.Items.Add(" " + hl[2]);
            }

        }

        private void MululistBox_MouseClick(object sender, MouseEventArgs e)
        {
            int index = mululistBox.IndexFromPoint(e.X, e.Y);
            if (index < 0)
            {
                return;
            }
            TextBox.Text = getpagetxt(blog_mllist[index].url);
        }
        bool loading = false;
        private void xqbutton_Click(object sender, EventArgs e)
        {
            if (loading|| page == 1)
            {
                return;
            }
            loading = true;
            page--;
            string nexurl = "http://www.xiaoqiangxs.org/lawenxiaoshuo/5_" + page + ".html";
            List<string> s = WTS.getWebData(nexurl).Split(new[] { "<div id=\"hotcontent\">", "<div id=\"newscontent\">", "<h2>好看的精品小说</h2>", "<div class=\"page_b page_b2\">喜欢就收藏我们</div>" }, StringSplitOptions.None).ToList();
            gxlistload(s[2]);
            pagelabel.Text = "第" + page + "页 共" + pagecount + "页";
            loading = false;
        }

        private void xhbutton_Click(object sender, EventArgs e)
        {
            if (loading || page == pagecount)
            {
                return;
            }
            loading = true;
            page++;
            string nexurl = "http://www.xiaoqiangxs.org/lawenxiaoshuo/5_" + page + ".html";
            List<string> s = WTS.getWebData(nexurl).Split(new[] { "<div id=\"hotcontent\">", "<div id=\"newscontent\">", "<h2>好看的精品小说</h2>", "<div class=\"page_b page_b2\">喜欢就收藏我们</div>" }, StringSplitOptions.None).ToList();
            gxlistload(s[2]);
            pagelabel.Text = "第" + page + "页 共" + pagecount + "页";
            loading = false;
        }
        /// <summary>
        /// 数字转中文
        /// </summary>
        /// <param name="number">eg: 22</param>
        /// <returns></returns>
        public string NumberToChinese(int number)
        {
            string res = string.Empty;
            string str = number.ToString();
            string schar = str.Substring(0, 1);
            switch (schar)
            {
                case "1":
                    res = "一";
                    break;
                case "2":
                    res = "二";
                    break;
                case "3":
                    res = "三";
                    break;
                case "4":
                    res = "四";
                    break;
                case "5":
                    res = "五";
                    break;
                case "6":
                    res = "六";
                    break;
                case "7":
                    res = "七";
                    break;
                case "8":
                    res = "八";
                    break;
                case "9":
                    res = "九";
                    break;
                default:
                    res = "零";
                    break;
            }
            if (str.Length > 1)
            {
                switch (str.Length)
                {
                    case 2:
                    case 6:
                        res += "十";
                        break;
                    case 3:
                    case 7:
                        res += "百";
                        break;
                    case 4:
                        res += "千";
                        break;
                    case 5:
                        res += "万";
                        break;
                    default:
                        res += "";
                        break;
                }
                res += NumberToChinese(int.Parse(str.Substring(1, str.Length - 1)));
            }
            return res;
        }

    }
}
class booklog
{
    public string name { get; set; }
    public string url { get; set; }
    public string type { get; set; }
}

class booklistbox
{
    public string Text { get; set; }
    public string Image { get; set; }
}

class tjbooklist
{
    public List<booklistbox> tj { get; set; }
}
class jpbooklist
{
    public List<booklistbox> tj { get; set; }
}
class gxbooklist
{
    public List<booklistbox> tj { get; set; }
}