using System.Text.Json.Serialization;

namespace SMT.ViewModel.Models
{
    public abstract class Resource : Link
    {
        [JsonIgnore]
        public Link Self { get; set; }
    }
}
