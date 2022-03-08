using System.Collections.Generic;

namespace ApiService.ViewModels
{
    public class GenericResponse
    {
        public bool Success { get; set; }
        public List<ErrorMessage> Errors { get; set; }
        public List<string> Messages { get; set; }
        public IList<object> Data { get; set; }
    }
}