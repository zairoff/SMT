﻿using SMT.ViewModel.Dto.ModelDto;

namespace SMT.ViewModel.Dto.ProductTransactionDto
{
    public class ReadyProductTransactionResponse
    {
        public int Id { get; set; }

        public ModelResponse Model { get; set; }

        public int Count { get; set; }

        public string Status { get; set; }

        public string Date { get; set; }
    }
}
