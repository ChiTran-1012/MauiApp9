using MauiApp9.Models;
using MauiApp9.Services;
using System.Collections.ObjectModel;

namespace MauiApp9.Pages;

public partial class AddPlacePage : ContentPage
{
    public ObservableCollection<City> Cities { get; set; } = new();
    public ObservableCollection<Place> Places { get; set; } = new();
    private Place editingPlace = null;

    public AddPlacePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var cities = await App.Database.GetCitiesAsync();
        Cities.Clear();
        foreach (var city in cities)
            Cities.Add(city);

        var places = await App.Database.GetPlacesAsync();
        Places.Clear();
        foreach (var place in places)
            Places.Add(place);
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        var selectedCity = CityPicker.SelectedItem as City;
        if (selectedCity == null)
        {
            await DisplayAlert("Lỗi", "Vui lòng chọn thành phố.", "OK");
            return;
        }

        string name = NameEntry.Text;
        string img = ImgEntry.Text;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(img))
        {
            await DisplayAlert("Lỗi", "Vui lòng nhập đầy đủ tên và ảnh.", "OK");
            return;
        }

        if (editingPlace == null)
        {
            var newPlace = new Place { Name = name, Img = img, CityId = selectedCity.Id };
            await App.Database.AddPlaceAsync(newPlace);
            await DisplayAlert("Thành công", "Đã thêm địa điểm!", "OK");
        }
        else
        {
            editingPlace.Name = name;
            editingPlace.Img = img;
            editingPlace.CityId = selectedCity.Id;
            await App.Database.UpdatePlaceAsync(editingPlace);
            await DisplayAlert("Thành công", "Đã cập nhật địa điểm!", "OK");
            editingPlace = null;
            AddButton.Text = "Thêm địa điểm";
            AddButton.BackgroundColor = Colors.Green;
        }

        // Làm mới danh sách
        Places.Clear();
        var updatedPlaces = await App.Database.GetPlacesAsync();
        foreach (var updatedPlace in updatedPlaces)
            Places.Add(updatedPlace);

        NameEntry.Text = "";
        ImgEntry.Text = "";
        CityPicker.SelectedIndex = -1;
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var place = button?.BindingContext as Place;
        if (place != null)
        {
            NameEntry.Text = place.Name;
            ImgEntry.Text = place.Img;
            CityPicker.SelectedItem = Cities.FirstOrDefault(c => c.Id == place.CityId);
            editingPlace = place;
            AddButton.Text = "Lưu chỉnh sửa";
            AddButton.BackgroundColor = Colors.Orange;
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var place = button?.BindingContext as Place;
        if (place != null)
        {
            bool confirm = await DisplayAlert("Xác nhận", "Bạn có chắc chắn muốn xóa địa điểm này?", "Có", "Không");
            if (confirm)
            {
                await App.Database.DeletePlaceByIdAsync(place.Id);
                await DisplayAlert("Thành công", "Địa điểm đã được xóa.", "OK");

                Places.Clear();
                var updatedPlaces = await App.Database.GetPlacesAsync();
                foreach (var updatedPlace in updatedPlaces)
                    Places.Add(updatedPlace);
            }
        }
    }
}
