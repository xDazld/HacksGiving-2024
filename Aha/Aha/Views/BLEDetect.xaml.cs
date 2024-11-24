using Aha.ViewModels;

namespace Aha.Views;

public partial class BLEDetect : ContentPage
{
    public BLEDetect()
    {
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as BLEDectectionViewModel).Initialize();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as BLEDectectionViewModel).BleDeviceDetector.StopScanning();
    }
}