using Newtonsoft.Json;
using Qiniu.IO;
using Qiniu.RS;
using Selfblog.Common;
using Selfblog.DomainObject;
using Selfblog.IService;
using Selfblog.Service;
using Selfblog.WebUI.Models.AppSettingConfig;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Selfblog.WebUI.Areas.Allapis.Controllers
{
    public class Upload
    {
        public string base64str { get; set; }
        public int photo_type { get; set; }
        public string imagename { get; set; }
    }

    public class Result
    {
        public int status { get; set; }
        public string message { get; set; }
        public string imageurl { get; set; }
        public int image_id { get; set; }
    }

    public class ImageController : ApiController
    {
        //
        // GET: /AllApi/Image/
        IphotoService photoservice = new photoService();

        //Api：Api的HttpPost是System.Web.Http;,Post请求:接收参数必须[FromBody]

        [HttpPost]
        public object PostSaveImage([FromBody] Upload data)
        {
            //base64传过来的时候
            int image_id;

            //将图片保存至本地改成将文件上传至七牛
            //string url = Base64stringToImage(data.base64str.Replace(" ", "+"), data.photo_type, data.imagename, out image_id);

            string url = Base64stringToQINIU(data.base64str.Replace(" ", "+"), data.photo_type, data.imagename, out image_id);


            Result result = new Result();
            if (string.IsNullOrWhiteSpace(url))
            {
                result.status = 0;
                result.message = "图片上传失败";
            }
            else
            {
                result.status = 1;
                result.message = "图片上传成功";
                result.imageurl = url;
                result.image_id = image_id;
            }
            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// base64str转为图片保存
        /// </summary>
        /// <param name="base64str">base64str字符串</param>
        private string Base64stringToImage(string base64str, int photo_type, string imagename, out int image_id)
        {
            string dir = string.Empty;
            image_id = 0;
            try
            {
                var butter = Convert.FromBase64String(base64str);
                MemoryStream ms = new MemoryStream(butter);
                Bitmap bitmap = new Bitmap(ms);
                if (string.IsNullOrWhiteSpace(imagename))
                {
                    dir = string.Format("/UploadImage/{0}.jpg", Guid.NewGuid().ToString());
                }
                else
                {
                    dir = string.Format("/Content/Images/language/{0}.jpg", imagename);
                }
                photoDomainObject photo = new photoDomainObject() { photo_imageurl = dir, photo_typeid = photo_type };
                var Photo = photoservice.AddEntity(photo);
                image_id = Photo.photo_id;
                bitmap.Save(HttpContext.Current.Server.MapPath(dir), System.Drawing.Imaging.ImageFormat.Jpeg);
                ms.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dir;
        }

        private string Base64stringToQINIU(string base64str, int photo_type, string imagename, out int image_id)
        {
            string dir = string.Empty;
            image_id = 0;
            try
            {
                var butter = Convert.FromBase64String(base64str);
                Stream stream = new MemoryStream(butter);

                #region 七牛操作 —— 上传图片

                var token = GetToken();
                PutExtra extra = new PutExtra();
                Qiniu.IO.IOClient client = new IOClient();
                var date = DateTime.Now;
                var key = date.Year.ToString() + (date.Month + 1).ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.Millisecond.ToString() + ".jpg";

                PutRet ret = client.Put(token, key, stream, extra);

                #endregion

                photoDomainObject photo = new photoDomainObject() { photo_imageurl = ret.key, photo_typeid = photo_type };
                var Photo = photoservice.AddEntity(photo);
                image_id = Photo.photo_id;
                dir = AllConfig.QiNiuConfig(WebCommon.Get_AppSetting("QiNiuUrl"),ret.key); 
                //string.Format("{0}{1}", WebCommon.GetAppSetting("key"), ret.key);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dir;
        }

        //图片转为base64编码的字符串
        private string ImgToBase64String(string Imagefilename)
        {
            try
            {
                Bitmap bmp = new Bitmap(Imagefilename);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //base64编码的字符串转为图片
        private Bitmap Base64StringToImage(string strbase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);

                //bmp.Save("test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //bmp.Save("test.bmp", ImageFormat.Bmp);
                //bmp.Save("test.gif", ImageFormat.Gif);
                //bmp.Save("test.png", ImageFormat.Png);
                ms.Close();
                return bmp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //byte[] 装换为 Stream
        private Stream ByteTostream(byte[] butter)
        {
            Stream stream = new MemoryStream(butter);
            return stream;
        }

        //Stream 转换为 byte[]
        private byte[] ByteTostream(Stream stream)
        {
            byte[] butter = new byte[stream.Length];
            stream.Read(butter, 0, butter.Length);

            stream.Seek(0, SeekOrigin.Begin);
            return butter;
        }

        //将Stream写入文件
        public void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[]
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            // 把 byte[] 写入文件
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        //从文件读取 Stream
        public Stream FileToStream(string fileName)
        {
            // 打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            return stream;
        }


        #region 七牛相关

        //图片上传至七牛


        //获取Token
        private string GetToken()
        {

            Qiniu.Conf.Config.ACCESS_KEY = "ZZdUQg0-Ww5ngJTQ8F6i-1khKtLKUEYyuZP5alGv";
            Qiniu.Conf.Config.SECRET_KEY = "SZiQqnmOh0yP4n-Sh1gE_gXOtsDZFtV6pHUKUGbJ";
            var policy = new PutPolicy("jiangshuilang", 3600);
            return policy.Token();
        }
        #endregion

    }
}