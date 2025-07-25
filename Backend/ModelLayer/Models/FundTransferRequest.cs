using System;

namespace ModelLayer.Models;

public class FundTransferRequest
    {
        public long FromAccountNumber{ get; set; }
        public long ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransferMode { get; set; } // NEFT, RTGS, IMPS
        public string Remarks { get; set; }
    }
