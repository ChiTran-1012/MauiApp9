using SQLite;
using MauiApp9.Models;

namespace MauiApp9.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _db;

        public DatabaseService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _ = CreateTablesAsync(); // Gọi tạo bảng nếu chưa có
        }

        // Tạo bảng nếu chưa tồn tại
        private async Task CreateTablesAsync()
        {
            await _db.CreateTableAsync<City>();
            await _db.CreateTableAsync<Place>();
        }

        // Lấy danh sách tất cả thành phố
        public Task<List<City>> GetCitiesAsync() =>
            _db.Table<City>().ToListAsync();

        // Lấy tất cả địa điểm
        public Task<List<Place>> GetPlacesAsync() =>
            _db.Table<Place>().ToListAsync();

        // Lấy địa điểm theo ID thành phố
        public Task<List<Place>> GetPlacesByCityIdAsync(int cityId) =>
            _db.Table<Place>().Where(p => p.CityId == cityId).ToListAsync();

        // Thêm địa điểm
        public Task<int> AddPlaceAsync(Place place) =>
            _db.InsertAsync(place);

        // Xóa địa điểm (theo đối tượng)
        public Task<int> DeletePlaceAsync(Place place) =>
            _db.DeleteAsync(place);

        // (Tùy chọn) Xóa địa điểm theo Id
        public async Task<int> DeletePlaceByIdAsync(int id)
        {
            var place = await _db.FindAsync<Place>(id);
            if (place != null)
            {
                return await _db.DeleteAsync(place);
            }
            return 0;
        }
    }
}
