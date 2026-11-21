using System.Collections.Generic;

namespace SMT.ViewModel.Dto.AssistantDto
{
    public class AssistantAskResponse
    {
        public string Answer { get; set; }

        /// <summary>Optional table backing the answer; empty when the answer is a single figure.</summary>
        public List<string> Columns { get; set; } = new List<string>();

        public List<List<string>> Rows { get; set; } = new List<List<string>>();

        /// <summary>Which data sources were queried, e.g. "daily_quality(2026-11-01..2026-11-20, line 3)" - shown so users can see where numbers came from.</summary>
        public List<string> Sources { get; set; } = new List<string>();
    }
}
