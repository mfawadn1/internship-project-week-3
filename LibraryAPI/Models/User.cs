namespace LibraryAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Member"; // Admin, Member, Librarian
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
