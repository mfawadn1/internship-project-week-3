using System.Text.Json.Serialization;

namespace LibraryAPI.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
