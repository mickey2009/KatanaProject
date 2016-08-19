﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using FunctionalTests.Common;
using Owin;
using Xunit;
using Xunit.Extensions;

namespace FunctionalTests.Facts.General
{
    public class ResponseStatusCodeTest
    {
        [Theory, Trait("FunctionalTests", "General")]
        [InlineData(HostType.IIS)]
        [InlineData(HostType.HttpListener)]
        public void TestResponseStatusCode(HostType hostType)
        {
            using (ApplicationDeployer deployer = new ApplicationDeployer())
            {
                string applicationUrl = deployer.Deploy(hostType, ConfigurationTest);
                HttpResponseMessage httpResponseMessage = null;

                HttpClientUtility.GetResponseTextFromUrl(applicationUrl + "/BadRequestPath", out httpResponseMessage);
                Assert.Equal(httpResponseMessage.StatusCode, HttpStatusCode.BadRequest);

                HttpClientUtility.GetResponseTextFromUrl(applicationUrl + "/GoodRequestPath", out httpResponseMessage);
                Assert.Equal(httpResponseMessage.StatusCode, HttpStatusCode.OK);
            }
        }

        public void ConfigurationTest(IAppBuilder appBuilder)
        {
            appBuilder.Run(context =>
            {
                if (context.Request.Path.Value.Contains("BadRequestPath"))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }

                return context.Response.WriteAsync("Responded..");
            });
        }
    }
}
