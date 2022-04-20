using ECP.Export.Excel;

namespace Export.Excel.App
{
    public class Receipt
    {
        [OutputColumn(1, "Bill To")]
        public string BillTo { get; set; }
        [OutputColumn(2, "Shipper")]
        public string Shipper { get; set; }
        [OutputColumn(3, "Amount(Rs.)")]
        public int  Amount { get; set; }
    }
}
