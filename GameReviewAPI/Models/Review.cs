public class Review
{
    public int Id { get; set; }  // Primary Key
    public string Username { get; set; }
    public int GameId { get; set; }
    public string ReviewText { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }

    public Game Game { get; set; } // Foreign Key relationship
}
