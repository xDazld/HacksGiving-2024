using Aha.ViewModels;

namespace Aha.Views;

public partial class BLEDetect : ContentPage
{
	public BLEDetect()
	{
		InitializeComponent();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as BLEDectectionViewModel).BleDeviceDetector.StopScanning();
    }
}