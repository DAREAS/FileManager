using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using FileManager.Tests.UnitTests.Extensions;
using System.Net;

namespace FileManager.Tests.UnitTests.WebApi.Controllers
{
    public abstract class BaseControllerTests<T> where T : Controller
    {
        private const string RequestId = "RequestId";

        // fields
        ///protected readonly PagedParametersRequest PagedParamenters = new PagedParametersRequest { Page = 1, RecordsPerPage = 3 };

        protected readonly IUrlHelper MockUrlHelper;
        protected readonly T Controller;

        // constructor
        protected BaseControllerTests()
        {
            MockUrlHelper = Substitute.For<IUrlHelper>();

            Controller = Activator.CreateInstance(typeof(T)) as T;

            Controller.SetHeaders();
            Controller.Url = MockUrlHelper;
            Controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "ClientApplicationName"),
                        new Claim("ClientApplicationKey", "ClientApplicationKey"),
                        new Claim("TenantId", "1")
                    })),
                    Items = new Dictionary<object, object>
                    {
                        [RequestId] = "RequestId"
                    }
                }
            };

            // Method to allow cleaning up data in the ThreadStorage in the initialization of each test (https://github.com/nsubstitute/NSubstitute/issues/260
            SubstitutionContext.Current.ThreadContext.DequeueAllArgumentSpecifications();
        }
    }
}
