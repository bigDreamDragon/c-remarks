using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace textToImg.Controllers
{
    public class HomeController : Controller
    {
        public string TransStringToImage(string Text)
        {
            //文本插入换行符
            MemoryStream stream = new MemoryStream();
            StringBuilder sb = new StringBuilder(Text);
            int offset = 0;
            int nowLen = 0;
            ArrayList indexList = new ArrayList();
            for (int i = 1; i < Text.Length; i++)
            {
                char c = Text[i];
                bool IsChinese = (int)c >= 0x4E00 && (int)c <= 0x9FA5;
                if (IsChinese == true)
                {
                    nowLen += 2;
                }
                else
                {
                    nowLen++;
                }
                if (nowLen > 20)
                {
                    nowLen = 0;
                    indexList.Add(i);
                }
            }
            for (int i = 0; i < indexList.Count; i++)
            {
                sb.Insert((int)indexList[i] + offset, '\n');
                offset++;
            }
            string text = sb.ToString();
            Bitmap bm = new Bitmap(100, 100);//建立实体图片并设定大小
            Graphics gs = Graphics.FromImage(bm);
            Font font = new Font("华文新魏", 22.2f, FontStyle.Bold);//设定字型大小.样式
            bm = new Bitmap(Convert.ToInt32(gs.MeasureString(text, font).Width), Convert.ToInt32(gs.MeasureString(text, font).Height));
            gs = Graphics.FromImage(bm);
            gs.Clear(Color.White);
            gs.TextRenderingHint = TextRenderingHint.AntiAlias;
            gs.DrawString(text, font, new SolidBrush(Color.Black), 0, 0);
            gs.Flush();
            //bm.Save(stream, ImageFormat.Jpeg);
            bm.Save(stream, ImageFormat.Jpeg);
            byte[] byteArray = stream.GetBuffer(); //將Bitmap转为Byte[]
            return Convert.ToBase64String(byteArray); //转为Base64sting
        }
    }
}