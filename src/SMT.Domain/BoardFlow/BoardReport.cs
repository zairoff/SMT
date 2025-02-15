using System;

namespace SMT.Domain.BoardFlow
{
    public class BoardReport
    {
        public int Id { get; set; }

        public string QrCode { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int QrReaderId { get; set; }

        public QrReader QrReader { get; set; }

        public DateTime DateTime { get; set; }

        public BoardPassStatus Status { get; set; }
    }
}
