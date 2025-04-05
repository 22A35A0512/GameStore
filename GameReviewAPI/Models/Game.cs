public class Game
{
    public int Id { get; set; }  // Primary Key
    public string Name { get; set; }
    public string Genre { get; set; }
    public List<Review> Reviews { get; set; }  // Navigation property
}
