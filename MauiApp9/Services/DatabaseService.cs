using SQLite;
using MauiApp9.Models;

namespace MauiApp9.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _db;

    public DatabaseService(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _ = CreateTablesAsync();
    }

    private async Task CreateTablesAsync()
    {
        await _db.CreateTableAsync<Place>();
        await _db.CreateTableAsync<City>();
    }

    public Task<List<City>> GetCitiesAsync() => _db.Table<City>().ToListAsync();

    public Task<List<Place>> GetPlacesAsync() => _db.Table<Place>().ToListAsync();

    public Task<List<Place>> GetPlacesByCityIdAsync(int cityId) =>
        _db.Table<Place>().Where(p => p.CityId == cityId).ToListAsync();

    public Task<int> AddPlaceAsync(Place place) => _db.InsertAsync(place);

    public Task<int> UpdatePlaceAsync(Place place) => _db.UpdateAsync(place);

    public Task<int> DeletePlaceByIdAsync(int id) =>
        _db.Table<Place>().Where(p => p.Id == id).DeleteAsync();
}
