using Newtonsoft.Json;
using Selfblog.DomainObject;
using Selfblog.IService;
using Selfblog.Service;
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
            string url = Base64stringToImage(data.base64str.Replace(" ", "+"), data.photo_type, out image_id);
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
        private string Base64stringToImage(string base64str, int photo_type, out int image_id)
        {
            string dir = string.Empty;
            image_id = 0;
            try
            {
                var butter = Convert.FromBase64String(base64str);
                MemoryStream ms = new MemoryStream(butter);
                Bitmap bitmap = new Bitmap(ms);
                dir = string.Format("/UploadImage/{0}.jpg", Guid.NewGuid().ToString());
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




    }
}