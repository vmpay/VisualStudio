using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;
using Calc.Models;


namespace Calc.Controllers
{
    public class CalcController : ApiController
    {
        [Route("api/add")]
        [HttpGet]
        public HttpResponseMessage GetSum([FromUri]double a, [FromUri]double b)
        {
            string xml = string.Format("{0}", a + b);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml");
            return response;
        }

        [Route("api/sub")]
        [HttpGet]
        public HttpResponseMessage GetDiff([FromUri]double a, [FromUri]double b)
        {
            string xml = string.Format("{0}", a - b);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml");
            return response;
        }

        [Route("api/sqrt")]
        [HttpGet]
        public HttpResponseMessage GetSqrt([FromUri]double a)
        {
            string xml = string.Format("{0}", Math.Sqrt(a));
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml");
            return response;
        }

        [Route("api/jedenprzezx")]
        [HttpGet]
        public HttpResponseMessage GetJedenprzezx([FromUri]double a)
        {
            string xml = string.Format("{0}", 1/a);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml");
            return response;
        }

        [Route("api/xpow2")]
        [HttpGet]
        public HttpResponseMessage GetXpow2([FromUri]double a)
        {
            string xml = string.Format("{0}", a*a);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml");
            return response;
        }

        [Route("api/xpowy")]
        [HttpGet]
        public HttpResponseMessage GetXpowy([FromUri]double a, [FromUri]double b)
        {
            string xml = string.Format("{0}", Math.Pow(a,b));
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml");
            return response;
        }

        [Route("api/mul")]
        [HttpGet]
        public HttpResponseMessage GetProduct([FromUri]double a, [FromUri]double b)
        {
            string xml = string.Format("{0}", a * b);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml");
            return response;
        }

        [Route("api/div")]
        [HttpGet]
        public HttpResponseMessage GetDiv([FromUri]double a, [FromUri]double b)
        {
            string xml = string.Format("{0}", a / b);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml");
            return response;
        }
    }
}
