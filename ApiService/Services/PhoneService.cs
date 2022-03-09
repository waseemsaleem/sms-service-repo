using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ApiService.Models;
using ApiService.Services.Interfaces;
using ApiService.ViewModels;

namespace ApiService.Services
{
    public class PhoneService : IPhoneService
    {
        private readonly accountdbContext _context;
        public PhoneService(accountdbContext context)
        {
            _context = context;
        }

        public IEnumerable<PhoneNumber> GetAll()
        {
            var result = _context.PhoneNumbers.ToList();
            return result;
        }

        public GenericResponse InOutboundSms(PhoneNumber smsRequest)
        {
            var phoneEntity = new PhoneNumber();
            var response = new GenericResponse();

            try
            {
                if (smsRequest != null)
                {
                    if (string.IsNullOrEmpty(smsRequest.From))
                    {
                        response.Error = "From Number is  Missing or Empty!";
                        response.Message = "";
                        return response;
                    }

                    if (smsRequest.From.Length < 6)
                    {
                        response.Error = "From Number should be minimum 6 characters!";
                        response.Message = "";
                        return response;
                    }

                    if (smsRequest.From.Length > 16)
                    {
                        response.Error = "From Number should Not be more than   16 characters!";
                        response.Message = "";
                        return response;
                    }

                    if (string.IsNullOrEmpty(smsRequest.To))
                    {
                        response.Error = "To Number is Empty or Missing!";
                        response.Message = "";
                        return response;
                    }

                    if (smsRequest.To.Length < 6)
                    {
                        response.Error = "To Number should be minimum 6 characters!";
                        response.Message = "";
                        return response;
                    }

                    if (smsRequest.To.Length > 16)
                    {
                        response.Error = "To Number should Not be more than  minimum 16 characters!";
                        response.Message = "";
                        return response;
                    }

                    if (string.IsNullOrEmpty(smsRequest.Text))
                    {
                        response.Error = "Message is Empty or Missing!";
                        response.Message = "";
                        return response;
                    }

                    if (smsRequest.Text.Length < 1)
                    {
                        response.Error = "Message should be minimum 1 characters!";
                        response.Message = "";
                        return response;
                    }

                    if (smsRequest.Text.Length > 120)
                    {
                        response.Error = "Message should Not be more than  minimum 120 characters!";
                        response.Message = "";
                        return response;
                    }

                    if (smsRequest.Text.ToLower().Contains("stop"))
                    {
                        response.Error = "Stop word is not supported";
                        response.Message = string.Empty;
                        return response;
                    }

                    phoneEntity.To = smsRequest.To;
                    phoneEntity.From = smsRequest.From;
                    phoneEntity.AccountId = smsRequest.AccountId;
                    phoneEntity.Number = smsRequest.Number;
                    phoneEntity.Text = smsRequest.Text;
                    _context.Add(smsRequest);
                    _context.SaveChanges();
                    response.Message = "Inbound Sms is Ok.";
                }
            }
            catch (Exception ex)
            {
                response.Error = "Unknown Failure";
                response.Message = string.Empty;
                return response;
            }
            return response;
        }


        public PhoneNumber Login(PhoneNumber phoneNumber)
        {
            _context.Add(phoneNumber);
            _context.SaveChanges();
            return phoneNumber;
        }

       
    }
}
