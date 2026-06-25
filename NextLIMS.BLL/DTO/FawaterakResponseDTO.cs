using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO
{
    public class CustomerDataDto
    {
        [JsonPropertyName("customer_unique_id")]
        public string CustomerUniqueId { get; set; }

        [JsonPropertyName("customer_first_name")]
        public string CustomerFirstName { get; set; }

        [JsonPropertyName("customer_last_name")]
        public string CustomerLastName { get; set; }

        [JsonPropertyName("customer_email")]
        public string CustomerEmail { get; set; }

        [JsonPropertyName("customer_phone")]
        public string CustomerPhone { get; set; }
    }

    public class PaymentRequestDto
    {
        [JsonPropertyName("transaction_key")]
        public string TransactionKey { get; set; }

        [JsonPropertyName("transaction_id")]
        public int TransactionId { get; set; }

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("pay_load")]
        public string PayLoad { get; set; }

        [JsonPropertyName("paidAmount")]
        public string PaidAmount { get; set; }

        [JsonPropertyName("paidCurrency")]
        public string PaidCurrency { get; set; }

        [JsonPropertyName("paidAt")]
        public string PaidAt { get; set; }

        [JsonPropertyName("customerData")]
        public CustomerDataDto CustomerData { get; set; }

        [JsonPropertyName("hashKey")]
        public string HashKey { get; set; }
    }
}
