using MauiApp9.Pages;
using MauiApp9.ViewModels;

namespace MauiApp9;

public partial class MainPage : ContentPage
{
    private MainViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent(); // Bắt buộc phải có nếu dùng XAML

        // Gán ViewModel nếu chưa gán trong XAML
        _viewModel = new MainViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Gọi lại load dữ liệu mỗi khi trang hiển thị lại (sau khi thêm địa điểm mới)
        await _viewModel.LoadPlacesAsync();
    }

    private async void OnAddPlaceClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddPlacePage());
    }
}
