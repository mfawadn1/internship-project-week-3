using System.Text.Json.Serialization;

namespace LibraryAPI.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Bio { get; set; }

        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
