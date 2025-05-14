using System.Collections.ObjectModel;
using System.ComponentModel;
using MauiApp9.Models;
using MauiApp9.Services;

namespace MauiApp9.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _db;

    public ObservableCollection<City> Cities { get; set; } = new();
    public ObservableCollection<Place> Places { get; set; } = new();

    private City _selectedCity;
    public City SelectedCity
    {
        get => _selectedCity;
        set
        {
            _selectedCity = value;
            OnPropertyChanged(nameof(SelectedCity));
            LoadPlacesAsync();
        }
    }

    public MainViewModel()
    {
        _db = App.Database;
        LoadCitiesAsync();
    }

    private async void LoadCitiesAsync()
    {
        var cities = await _db.GetCitiesAsync();
        Cities.Clear();
        foreach (var city in cities)
            Cities.Add(city);
    }

    public async Task LoadPlacesAsync()
    {
        if (SelectedCity == null) return;
        var places = await _db.GetPlacesByCityIdAsync(SelectedCity.Id);
        Places.Clear();
        foreach (var place in places)
            Places.Add(place);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
