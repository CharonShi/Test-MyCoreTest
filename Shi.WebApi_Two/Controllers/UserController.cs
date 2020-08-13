using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shi.WebApi_Two.Controllers
{

    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("PostUser")]
        public IActionResult PostUser([FromForm] int l)
        {

            var request = Request.Form.Files;

            return Ok("ssss");
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser()
        {
            var request = Request.Form.Files;

            return Ok("ssss");
        }
    }
}