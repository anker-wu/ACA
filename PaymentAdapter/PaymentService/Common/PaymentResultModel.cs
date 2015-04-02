namespace Accela.ACA.PaymentAdapter
{
    /// <summary>
    /// The payment result model.
    /// </summary>
    public class PaymentResultModel
    {
        public string TransactionId { get; set; }

        public string PaymentAmount { get; set; }

        public string ConvenienceFee { get; set; }

        public string ProcTransactionId { get; set; }

        public string PaymentMethod { get; set; }

        public string CreditCardType { get; set; }

        public string CCAuthCode { get; set; }

        public string CardNumber { get; set; }

        public string Payee { get; set; }

        public string PayeeAddress { get; set; }

        public string PayeePhone { get; set; }

        public string PaymentComment { get; set; }

        public string CustomBatchTransNbr { get; set; }
    }
}
