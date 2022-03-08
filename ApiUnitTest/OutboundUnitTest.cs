//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ApiService.Controllers;
//using ApiService.Models;
//using ApiService.Services.Interfaces;
//using Xunit;

//namespace ApiUnitTest
//{
//    public class OutboundUnitTest : IClassFixture<OutboundController>
//    {
//        private readonly IPhoneService _phoneService;
//        public OutboundUnitTest(IPhoneService phoneService)
//        {
//            _phoneService = phoneService;
//        }
//        [Theory(DisplayName = "Inbound SMS")]
//        [InlineData("03446137410", "03446137410", "test", 3, "36333313413")]
//        public void OutboundSms(string from, string to, string body, int accId, string number)
//        {
//            var smsRequest = new PhoneNumber
//            {
//                From = from,
//                To = to,
//                Text = body,
//                AccountId = accId,
//                Number = number
//            };
//            var result = _phoneService.Add(smsRequest);
//            Assert.NotNull(result);
//        }
//    }
//}
