using SQLite;
using MauiApp9.Models;

namespace MauiApp9.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _db;

    public DatabaseService(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        // Không gọi CreateTable nếu đã có file sẵn
    }

    public Task<List<City>> GetCitiesAsync() => _db.Table<City>().ToListAsync();

    public Task<List<Place>> GetPlacesByCityIdAsync(int cityId) =>
        _db.Table<Place>().Where(p => p.CityId == cityId).ToListAsync();
}
