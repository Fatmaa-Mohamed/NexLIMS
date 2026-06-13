using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Data.Payment
{
    public class FawaterkSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://staging.fawaterk.com/api/v2";
    }

    public class InvoiceRequest
    {
        public int PaymentMethodId { get; set; } 
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 1200;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
    }
}
