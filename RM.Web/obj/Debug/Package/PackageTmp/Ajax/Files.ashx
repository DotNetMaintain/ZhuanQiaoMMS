<%@ WebHandler Language="c#" Class="File_WebHandler" Debug="true" %>

using System;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class File_WebHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;
        if (files.Count > 0)
        {
            Random rnd = new Random();
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];

                if (file.ContentLength > 0)
                {
                  
                    
                    string fileName = file.FileName;
                    string extension = Path.GetExtension(fileName);
                    int num = rnd.Next(5000, 10000);
                    string path = "../MMS/file/" + num.ToString() + extension;

                    CutForSquare(file, System.Web.HttpContext.Current.Server.MapPath(path), 50, 50);
                   // file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));
                    context.Response.Write("../file/" + num.ToString() + extension);
                  //  file.SaveAs(path);
                }
            }
        }
    }





    public static void CutForSquare(System.Web.HttpPostedFile postedFile, string fileSaveUrl, int side, int quality)
    {
        //����Ŀ¼
        string dir = Path.GetDirectoryName(fileSaveUrl);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        //ԭʼͼƬ����ȡԭʼͼƬ�������󣬲�ʹ������Ƕ�����ɫ������Ϣ��
        System.Drawing.Image initImage = System.Drawing.Image.FromStream(postedFile.InputStream, true);

        //ԭͼ��߾�С��ģ�棬��������ֱ�ӱ���
        if (initImage.Width <= side && initImage.Height <= side)
        {
            initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        else
        {
            //ԭʼͼƬ�Ŀ���
            int initWidth = initImage.Width;
            int initHeight = initImage.Height;

            //���������Ȳü�Ϊ������
            if (initWidth != initHeight)
            {
                //��ͼ����
                System.Drawing.Image pickedImage = null;
                System.Drawing.Graphics pickedG = null;

                //����ڸߵĺ�ͼ
                if (initWidth > initHeight)
                {
                    //����ʵ����
                    pickedImage = new System.Drawing.Bitmap(initHeight, initHeight);
                    pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                    //��������
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //��λ
                    Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                    Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                    //��ͼ
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                    //���ÿ�
                    initWidth = initHeight;
                }
                //�ߴ��ڿ����ͼ
                else
                {
                    //����ʵ����
                    pickedImage = new System.Drawing.Bitmap(initWidth, initWidth);
                    pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                    //��������
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //��λ
                    Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                    Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                    //��ͼ
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                    //���ø�
                    initHeight = initWidth;
                }

                //����ͼ���󸳸�ԭͼ
                initImage = (System.Drawing.Image)pickedImage.Clone();
                //�ͷŽ�ͼ��Դ
                pickedG.Dispose();
                pickedImage.Dispose();
            }

            //����ͼ����
            System.Drawing.Image resultImage = new System.Drawing.Bitmap(side, side);
            System.Drawing.Graphics resultG = System.Drawing.Graphics.FromImage(resultImage);
            //��������
            resultG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            resultG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //��ָ������ɫ��ջ���
            resultG.Clear(Color.White);
            //��������ͼ
            resultG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, side, side), new System.Drawing.Rectangle(0, 0, initWidth, initHeight), System.Drawing.GraphicsUnit.Pixel);

            //�ؼ���������
            //��ȡϵͳ������������,������jpeg,bmp,png,gif,tiff
            ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo i in icis)
            {
                if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                {
                    ici = i;
                }
            }
            EncoderParameters ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

            //��������ͼ
            resultImage.Save(fileSaveUrl, ici, ep);

            //�ͷŹؼ���������������Դ
            ep.Dispose();

            //�ͷ�����ͼ��Դ
            resultG.Dispose();
            resultImage.Dispose();

            //�ͷ�ԭʼͼƬ��Դ
            initImage.Dispose();
        }
    }
    

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}