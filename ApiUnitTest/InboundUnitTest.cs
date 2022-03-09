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
    public class InboundUnitTest
    {
        public Mock<IPhoneService> Service = new Mock<IPhoneService>();
        public Mock<IConfiguration> Config = new Mock<IConfiguration>();
        public Mock<IDistributedCache> Cache = new Mock<IDistributedCache>();
        public Mock<ILogger<InboundController>> Log = new Mock<ILogger<InboundController>>();

        [Fact]
        public void InboundSms()
        {
            var smsRequest = new PhoneNumber
            {
                From = "03061981436",
                To = "03061981496",
                Text = "hello world",
                AccountId = 1,
                Number = "03061981436"
            };
            Service.Setup(p => p.InOutboundSms(smsRequest));
            InboundController inboundController = new InboundController(Service.Object, Config.Object,Cache.Object,Log.Object);
            var result = inboundController.InboundSms(smsRequest);
            Assert.NotNull(result);
        }
    }
}
