using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiService.Controllers;
using ApiService.Models;
using ApiService.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ApiUnitTest
{
    public class OutboundUnitTest
    {
        public Mock<IPhoneService> Service = new Mock<IPhoneService>();
        public Mock<IConfiguration> Config = new Mock<IConfiguration>();
        public Mock<IDistributedCache> Cache = new Mock<IDistributedCache>();
        public Mock<ILogger<OutboundController>> Log = new Mock<ILogger<OutboundController>>();

        [Fact]
        public void OutboundSms()
        {
            var smsRequest = new PhoneNumber
            {
                From = "03221844093",
                To = "03221432097",
                Text = "Outbound Sms",
                AccountId = 1,
                Number = "03221844093"
            };
            Service.Setup(p => p.InOutboundSms(smsRequest));
            OutboundController outboundController = new OutboundController(Service.Object, Config.Object, Cache.Object, Log.Object);
            var result = outboundController.OutboundSms(smsRequest);
            Assert.NotNull(result);
        }
    }
}
