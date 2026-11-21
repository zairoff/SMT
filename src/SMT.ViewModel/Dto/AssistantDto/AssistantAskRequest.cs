using System.Collections.Generic;

namespace SMT.ViewModel.Dto.AssistantDto
{
    public class AssistantAskRequest
    {
        /// <summary>The new question, in any language.</summary>
        public string Question { get; set; }

        /// <summary>UI language (en/ru/uz) - the answer is written in this language.</summary>
        public string Language { get; set; }

        /// <summary>Earlier questions and answers of this conversation, oldest first, so follow-ups ("and for line 2?") work.</summary>
        public List<AssistantTurn> History { get; set; } = new List<AssistantTurn>();
    }

    public class AssistantTurn
    {
        /// <summary>"user" or "assistant".</summary>
        public string Role { get; set; }

        public string Text { get; set; }
    }
}
