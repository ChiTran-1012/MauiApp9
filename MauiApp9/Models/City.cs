using SQLite;

namespace MauiApp9.Models;

public class City
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
}
