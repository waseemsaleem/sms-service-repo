using System.Collections.Generic;
using ApiService.Models;
using ApiService.ViewModels;

namespace ApiService.Services.Interfaces
{
   public interface IPhoneService
    {
        List<PhoneNumber> GetAll();
        GenericResponse InOutboundSms(PhoneNumber phoneDetails);

    }
}
