using ApiService.Controllers;
using ApiService.Models;
using ApiService.Services.Interfaces;
using Moq;
using Xunit;

namespace ApiUnitTest
{
    public class InboundUnitTest
    {
        public Mock<IPhoneService> Mock = new Mock<IPhoneService>();
        [Fact]
        public void InboundSms()
        {


            var smsRequest = new PhoneNumber
            {
                From = "456465455",
                To = "545456454878",
                Text = "hello world",
                AccountId = 1,
                Number = "42452454"
            };
            Mock.Setup(p => p.Save(smsRequest));
            InboundController emp = new InboundController(Mock.Object);
            var result = emp.InboundSms(smsRequest);
            Assert.NotNull(result);
        }
    }
}
