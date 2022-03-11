using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using RSInvoiceLibrary.Helpers;
using RSInvoiceLibrary.Models;
using System.ServiceModel;
using System.Xml;

namespace RSInvoiceLibrary
{
    public class InvoiceService
    {
         readonly  RSInvoiceService.NtosServiceSoapClient _ntosServiceSoapClient;
         readonly BasicHttpsBinding _binding;
         readonly EndpointAddress _address;
         private string MyIp => WhatIsMyIP();
         private string _webServiceAddress = "https://www.revenue.mof.ge/ntosservice/ntosservice.asmx";

        private string UserName { get;  set; }
        private string UserPassword { get; set; }
        private int UserId { get; set; }

        public InvoiceService(string userName, string password, int userId)
        {
            UserName = userName;    
            UserPassword = password;    
            UserId = userId;    

             _binding = new BasicHttpsBinding()
            {
                MaxBufferPoolSize = 2147483647,
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxArrayLength = 200000000,
                    MaxStringContentLength = 200000000
                }
            };
             _address = new EndpointAddress(_webServiceAddress);
            _ntosServiceSoapClient = new RSInvoiceService.NtosServiceSoapClient(_binding, _address);
        }

        public string WhatIsMyIP()
        {
            return _ntosServiceSoapClient.what_is_my_ip();
        }

        public void GetInvoiceDesc(string serUserName = "erp2021:206322102" , string serPassword = "123456", int invoiceId = 224760312)
        {
            var xml =  _ntosServiceSoapClient.get_invoice_desc(UserId, invoiceId, serUserName, serPassword).Any1;
            var json = JsonConvert.SerializeXmlNode(xml);
            var obj = JsonConvert.DeserializeObject<GetSerUsersResponseGetSerUsersResult>(json);
        }

        public void GetServiceUser()
        {
            int userId;
            var xml = _ntosServiceSoapClient.get_ser_users(UserName, UserPassword, out userId);
            if (UserId != userId) throw new Exception("Correct userId is not detected !");
            //var xml = new GetSerUsersResponseGetSerUsersResult();
            var newObj = xml.GetNewObject(xml.Any1.OuterXml);
           // return newObj;
        }
        public bool Check(string serUserName, string serPassword)
        {
            int userId = UserId;
            int serUserId;

            return   _ntosServiceSoapClient.chek(serUserName, serPassword, ref userId, out serUserId) ? true : false;    
        }

        public bool ChangeInvoiceStatus(long invoiceId, int status, string serUserName, string serPassword)
        {
            int userId = UserId;
        
            return _ntosServiceSoapClient.change_invoice_status(userId, invoiceId, status, serUserName, serPassword) ? true : false;
        }

    }
}