using Newtonsoft.Json;

namespace RSInvoiceLibrary.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Ip { get; set; }
        public string Notes { get; set; }
    }

    public class DocumentElement
    {
        public List<User> Users { get; set; }
    }

    public class GetSerUsersResponseGetSerUsersResult
    {
        public DocumentElement DocumentElement { get; set; }
    }
}