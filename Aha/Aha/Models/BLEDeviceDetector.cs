using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;

namespace Aha.Models
{
    public class BLEDeviceDetector
    {
        private readonly IBluetoothLE _bluetoothManager;
        private readonly IAdapter _adapter;
        private CancellationTokenSource _scanCancellationTokenSource;
        private Dictionary<string, IDevice> _devices = new Dictionary<string, IDevice>();

        public event EventHandler<IDevice> LocationContextDeviceDetected;

        public BLEDeviceDetector()
        {
            _bluetoothManager = CrossBluetoothLE.Current;
            _adapter = _bluetoothManager.Adapter;

            if (_bluetoothManager != null && _adapter != null)
            {
                ConfigureBLE();
            }
        }

        private void ConfigureBLE()
        {
            _adapter.ScanMode = ScanMode.LowLatency;
            //Uncomment the below line to set the scan timeout
            _adapter.ScanTimeout = 1000; // ms
            _adapter.DeviceDiscovered += OnDeviceDiscovered;
        }

        //private void OnDeviceAdvertised(object sender, DeviceEventArgs args)
        //{
        //    CheckAndRaiseEvent(args.Device);
        //}

        private void OnDeviceDiscovered(object sender, DeviceEventArgs args)
        {
            CheckAndRaiseEvent(args.Device);
        }

        private void CheckAndRaiseEvent(IDevice device)
        {
            if (device.Name.Contains("LocationContext"))
            {
                if (!_devices.ContainsKey(device.Id.ToString())){
                    _devices.Add(device.Id.ToString(), device);
                    LocationContextDeviceDetected?.Invoke(this, device);
                }
            }
        }

        public async Task StartScanningAsync()
        {
            if (!_bluetoothManager.IsOn)
            {
                Debug.WriteLine("Bluetooth is not ON. Please turn on Bluetooth and try again.");
                return;
            }

            _scanCancellationTokenSource = new CancellationTokenSource();
            await _adapter.StartScanningForDevicesAsync(_scanCancellationTokenSource.Token);
        }

        public void StopScanning()
        {
            _scanCancellationTokenSource?.Cancel();
        }
    }
}
