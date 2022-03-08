using System.Net;

namespace ApiService.ViewModels
{
    public class ErrorMessage
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Error { get; set; }
    }
}