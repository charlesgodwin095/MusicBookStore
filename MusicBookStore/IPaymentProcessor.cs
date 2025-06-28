// Services/Payment/IpaymentProcessor.cs
using System.Threading.Tasks;
using MusicBookStore.Models;

namespace MusicBookStore.Services.Payment 
{
    public interface IpaymentProcessor
    {
        /// <summary>
        /// Processes a Payment for an order
        /// </summary>
        /// <param name="order">Order to process</param>
        /// <param name="paymentToken">Payment Source token (e.g., Stripe token, paypal order ID)</param>
        /// <param name="billingDetails">Customer billing information</param>
        /// <returns>Payment results with status and details</returns>
        Task<PaymentResult> ProcessPaymentAsync(Order order, string paymentToken, Billing billingDetails);

        /// <summary>
        /// Creates a payment intent for client-side confirmation
        /// </summary>
        /// <param name="amount">Payment amount</param>
        /// <param name="currency">Currency code (e.g.,"usd")</param>
        /// <returns>Client secret for payment confirmation</returns>
        Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "USD");

        /// <summary>
        /// Issues a full or partial refund
        /// </summary>
        ///  <param name="paymentId">Original payment ID</param>
        /// <param name="amount">Amount to refund (null for full refund)</param>
        /// <returns>Refund result with status</returns>
        Task<string> IssuesRefundAsync(string paymentId, decimal? amount = null);


        /// <summary>
        /// Create a customer in the payment system
        /// </summary>
        ///  <param name="user">Application user</param>
        /// <returns>Payment system customer ID</returns>
        Task<string> IssuesRefundAsync(ApplicationUser user);

        /// <summary>
        /// Verifies a webhook signature
        /// </summary>
        ///  <param name="payload">Request payload</param>
        /// <param name="signature">Signature header</param>
        /// <returns>Verification result</returns>
        Task<bool> VerifyWebhookSignatureAsync(string payload, string signature); 
    }

    public class RefundResult
    {
        public bool Success { get; set; }
        public string RefundId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class BillingDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }  
}