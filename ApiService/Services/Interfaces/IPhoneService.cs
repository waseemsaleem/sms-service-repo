﻿using System.Collections.Generic;
using ApiService.Models;
using ApiService.ViewModels;

namespace ApiService.Services.Interfaces
{
   public interface IPhoneService
    {
        IEnumerable<PhoneNumber> GetAll();
        GenericResponse Save(PhoneNumber phoneDetails);
        PhoneNumber Add(PhoneNumber phoneDetails);

    }
}