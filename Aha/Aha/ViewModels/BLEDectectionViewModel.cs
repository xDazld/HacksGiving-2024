using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aha.Models;
using Aha.Views;
using CommunityToolkit.Mvvm.Input;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace Aha.ViewModels
{
    public class BLEDectectionViewModel : AbstractViewModel
    {
        public BLEDeviceDetector BleDeviceDetector;
        private Boolean isScanning = false;
        public ObservableCollection<IDevice> Devices { get; set; } = new ObservableCollection<IDevice>();
        public AsyncRelayCommand StartChatCommand { get; set; }

        public BLEDectectionViewModel()
        {
            StartChatCommand = new AsyncRelayCommand(StartChat);
        }

        public void Initialize()
        {
            BleDeviceDetector = new BLEDeviceDetector();
            BleDeviceDetector.LocationContextDeviceDetected += OnLocationContextDeviceDetected;
            StartBLEScanning();
        }

        private async Task StartChat()
        {
            //Need to determine the closest device and use that as location context
            IDevice currentClosest = Devices.FirstOrDefault();
            foreach (IDevice device in Devices)
            {
#if ANDROID || IOS
                await device.UpdateRssiAsync();
#endif
            }

            foreach (IDevice device in Devices)
            {
                if (device.Rssi > currentClosest.Rssi)
                {
                    currentClosest = device;
                }
            }
            string locationName = "";
            LocationContextManager.getLocationContextManager().BLEDeviceNameToLocationContext.TryGetValue(currentClosest.Name, out locationName);

            LocationContextManager.getLocationContextManager().CurrentNearestLocationContext = new LocationContext(locationName);
            Shell.Current.Navigation.PushAsync(new Chat());
        }

        private async void StartBLEScanning()
        {
            isScanning = true;
            await BleDeviceDetector.StartScanningAsync();
        }

        private async void OnLocationContextDeviceDetected(object sender, IDevice device)
        {
            BleDeviceDetector.StopScanning();
            Debug.WriteLine($"Device detected: {device.Name}");
            // Do something with the device
            //Invoke on the main thread
            Devices.Add(device);
            OnPropertyChanged(nameof(Devices));
        }
    }
}
