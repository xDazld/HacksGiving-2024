using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aha.Models;
using Aha.Views;
using Plugin.BLE.Abstractions.Contracts;

namespace Aha.ViewModels
{
    public class BLEDectectionViewModel : AbstractViewModel
    {
        public BLEDeviceDetector BleDeviceDetector;
        private Boolean isScanning = false;

        public BLEDectectionViewModel()
        {
            BleDeviceDetector = new BLEDeviceDetector();
            BleDeviceDetector.LocationContextDeviceDetected += OnLocationContextDeviceDetected;
            StartBLEScanning();
        }

        private async void StartBLEScanning()
        {
            isScanning = true;
            await BleDeviceDetector.StartScanningAsync();
        }

        private async void OnLocationContextDeviceDetected(object sender, IDevice device)
        {
            if (isScanning)
            {
                isScanning = false;
                BleDeviceDetector.StopScanning();
                Debug.WriteLine($"Device detected: {device.Name}");
                // Do something with the device
                //Invoke on the main thread
                    
                await Shell.Current.GoToAsync("//Chat");
            }
        }
    }
}
