using MauiApp9.Models;
using MauiApp9.Services;
using System.Collections.ObjectModel;

namespace MauiApp9.Pages;

public partial class AddPlacePage : ContentPage
{
    public AddPlacePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public ObservableCollection<City> Cities { get; set; } = new ObservableCollection<City>();
    public ObservableCollection<Place> Places { get; set; } = new ObservableCollection<Place>();

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var cities = await App.Database.GetCitiesAsync();
        Cities.Clear();
        foreach (var city in cities)
        {
            Cities.Add(city);
        }

        var places = await App.Database.GetPlacesAsync();
        Places.Clear();
        foreach (var place in places)
        {
            Places.Add(place);
        }
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

        var place = new Place
        {
            Name = name,
            Img = img,
            CityId = selectedCity.Id
        };

        await App.Database.AddPlaceAsync(place);
        await DisplayAlert("Thành công", "Đã thêm địa điểm!", "OK");

        await LoadPlacesAsync();
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
                await App.Database.DeletePlaceAsync(place);
                await DisplayAlert("Thành công", "Địa điểm đã được xóa.", "OK");
                await LoadPlacesAsync();
            }
        }
    }

    private async Task LoadPlacesAsync()
    {
        Places.Clear();
        var updatedPlaces = await App.Database.GetPlacesAsync();
        foreach (var place in updatedPlaces)
        {
            Places.Add(place);
        }
    }
}