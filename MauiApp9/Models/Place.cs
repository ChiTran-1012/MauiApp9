using SQLite;

namespace MauiApp9.Models;

public class Place
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public int CityId { get; set; }
    public string Img { get; set; } // ví dụ: "bana.jpg"
}
