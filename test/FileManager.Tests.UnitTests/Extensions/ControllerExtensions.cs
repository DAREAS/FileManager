using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Tests.UnitTests.Extensions
{
    public static class ControllerExtensions
    {
        private const string UserIdentifier = "UserIdentifier";
        private const string RequestId = "RequestId";
        private const string AcceptLanguage = "AcceptLanguage";

        public static void SetHeaders(this Controller controller)
        {
            // set headers
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            controller.ControllerContext.HttpContext.Request.Headers[UserIdentifier] = Guid.NewGuid().ToString();
            controller.ControllerContext.HttpContext.Request.Headers[RequestId] = Guid.NewGuid().ToString();
            controller.ControllerContext.HttpContext.Request.Headers[AcceptLanguage] = "pt-BR";
        }
    }
}
