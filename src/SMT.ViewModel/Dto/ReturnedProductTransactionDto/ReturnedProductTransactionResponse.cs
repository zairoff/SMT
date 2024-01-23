using SMT.ViewModel.Dto.ModelDto;

namespace SMT.ViewModel.Dto.ReturnedProductTransactionDto
{
    public class ReturnedProductTransactionResponse
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public ModelResponse Model { get; set; }

        public int Count { get; set; }

        public string TransactionType { get; set; }

        public string Date { get; set; }
    }
}
