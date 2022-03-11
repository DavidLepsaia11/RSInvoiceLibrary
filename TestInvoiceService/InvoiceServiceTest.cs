using RSInvoiceLibrary;
using Xunit;

namespace TestInvoiceService
{
    public class InvoiceServiceTest
    {
        private InvoiceService invoiceService => new InvoiceService("tbilisi", "123456", 783);

        [Fact]
        public void WhatIsMyIpTest()
        {
            invoiceService.WhatIsMyIP();
        }

        [Fact]
        public void GetServiceUsersTest()
        {
            invoiceService.GetServiceUser();
        }

        [Fact]
        public void CheckTest()
        {
            invoiceService.Check("erp2021: 206322102", "123456");
        }

        [Fact]
        public void GetInvoiceDescTest()
        {
            invoiceService.GetInvoiceDesc("erp2021: 206322102", "123456", 224760312);
        }

        [Fact]
        public void ChangeInvoiceStatusTest()
        {
            invoiceService.ChangeInvoiceStatus(224760312,2, "erp2021: 206322102","123456");
        }
    }
}