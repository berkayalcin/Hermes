namespace Hermes.API.User.Domain.Responses
{
    public class ServiceResponseModel
    {
        public ServiceResponseModel(string message, bool status)
        {
            Message = message;
            Status = status;
        }

        public string Message { get; set; }
        public bool Status { get; set; }
    }
}