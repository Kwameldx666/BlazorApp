using BlazorApp.Models;

namespace BlazorApp.Models.Response
{
    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; } // Например, ID транзакции из платежной системы
    }
}
