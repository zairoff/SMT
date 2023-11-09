using SMT.Domain;

namespace SMT.ViewModel.Dto.ProductTransactionDto
{
    public class ReadyProductTransactionCreate
    {
        public int ModelId { get; set; }

        public int Count { get; set; }

        public ReadyProductTransactionType Status { get; set; }
    }
}
