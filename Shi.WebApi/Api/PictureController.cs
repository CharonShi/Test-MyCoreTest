using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Ocsp;
using Shi.Models;
using Shi.Service;
using Ubiety.Dns.Core.Records;

namespace Shi.WebApi.Api
{
    [Route("api/[controller]")]
    public class PictureController : ApiBaseController
    {


        private readonly IHostEnvironment _hostEnvironment;

        public PictureController(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }



        /// <summary>
        /// 分页查询图片
        /// </summary>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        [HttpGet("GetPictrue")]
        public IActionResult GetPictrue(int pIndex = 1, int pSize = 20)
        {
            var list = PictrueBus.GetPicPage(pIndex, pSize, User.Id);
            return Ok(new ResData(list));
        }

        [HttpPost("PicUpload")]
        public IActionResult PicUpload()
        {
            var req = Request;

            var files = req.Form.Files;

            if (files.Count <= 0)
            {
                return Ok(ResData.Error());
            }

            var conPath = _hostEnvironment.ContentRootPath; //程序地址

            var basePath = AppDomain.CurrentDomain.BaseDirectory;//基目录

            foreach (var file in files)
            {
                if (file.Length > 0)
                {

                    string fileExt = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1, (file.FileName.Length - file.FileName.LastIndexOf(".") - 1));  //文件扩展名，不含“.”
                    long fileSize = file.Length; //获得文件大小，以字节为单位
                    string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                    var filePath = conPath + "/Img/" + newFileName;

                    using (var fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    };


                }

            }
            return Ok(new ResData());
        }



        [HttpPost("ReturnFile")]
        public IActionResult ReturnFile()
        {
            var req = Request;
            var conPath = _hostEnvironment.ContentRootPath; 
            var pathC = conPath + "/Img/tiem.png";

            if (!System.IO.File.Exists(pathC))
            {
                return Ok(ResData.Error());
            }

            var file2 = System.IO.File.OpenRead(pathC);

            return File(file2, "image/jpeg", Path.GetFileName(pathC));
        }

    }
}