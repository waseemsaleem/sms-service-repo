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

        public GenericResponse Save(PhoneNumber smsRequest)
        {
            var phoneEntity = new PhoneNumber();
            var response = new GenericResponse();

            try
            {
                if (smsRequest != null)
                {
                    if (string.IsNullOrEmpty(smsRequest.From))
                    {
                        response.Errors.Add(new ErrorMessage() { Error = "From Number is  Missing or Empty!", HttpStatusCode = HttpStatusCode.BadRequest });
                    }

                    if (smsRequest.From.Length < 6)
                    {
                        response.Errors.Add(new ErrorMessage() { Error = "From Number should be minimum 6 characters!", HttpStatusCode = HttpStatusCode.BadRequest });
                    }

                    if (smsRequest.From.Length > 16)
                    {
                        response.Errors.Add(new ErrorMessage() { Error = "From Number should Not be more than   16 characters!", HttpStatusCode = HttpStatusCode.BadRequest });
                    }

                    if (string.IsNullOrEmpty(smsRequest.To))
                    {
                        response.Errors.Add(new ErrorMessage() { Error = "To Number is Empty or Missing!", HttpStatusCode = HttpStatusCode.BadRequest });
                    }

                    if (smsRequest.To.Length < 6)
                    {
                        response.Errors.Add(new ErrorMessage() { Error = "To Number should be minimum 6 characters!", HttpStatusCode = HttpStatusCode.BadRequest });
                    }

                    if (smsRequest.To.Length > 16)
                    {
                        response.Errors.Add(new ErrorMessage() { Error = "To Number should Not be more than  minimum 16 characters!", HttpStatusCode = HttpStatusCode.BadRequest });

                    }

                    if (response.Errors.Any())
                    {
                        response.Success = false;
                        return response;
                    }

                    phoneEntity.To = smsRequest.To;
                    phoneEntity.From = smsRequest.From;
                    phoneEntity.AccountId = smsRequest.AccountId;
                    phoneEntity.Number = smsRequest.Number;
                    phoneEntity.Text = smsRequest.Text;
                    _context.Add(smsRequest);
                    _context.SaveChanges();
                    response.Success = true;
                    response.Messages.Add("SMS is saved successfully.");
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors.Add(new ErrorMessage() { Error = ex.Message });
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

        public PhoneNumber Add(PhoneNumber phoneDetails)
        {
            _context.Add(phoneDetails);
            _context.SaveChanges();
            return phoneDetails;
        }
    }
}
