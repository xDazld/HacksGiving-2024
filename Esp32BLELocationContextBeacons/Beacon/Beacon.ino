#include <BLEDevice.h>
#include <BLEUtils.h>
#include <BLEAdvertising.h>

#define DEVICE_NAME "LocationContext_1" //Change the location context number for each location per exhibit

void setup() {
  Serial.begin(115200);

  // Initialize BLE
  BLEDevice::init(DEVICE_NAME);

  // Start advertising
  BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
  pAdvertising->setScanResponse(false); // Disable scan response
  pAdvertising->setMinPreferred(0x06); // For iPhone compatibility
  pAdvertising->setMinPreferred(0x12);

  // Set the advertisement data
  BLEAdvertisementData advertisementData;
  advertisementData.setName(DEVICE_NAME); // Set the device name
  advertisementData.setFlags(ESP_BLE_ADV_FLAG_GEN_DISC | ESP_BLE_ADV_FLAG_BREDR_NOT_SPT);
  
  // Apply the advertisement data
  pAdvertising->setAdvertisementData(advertisementData);
  
  pAdvertising->start();
  Serial.println("Broadcasting BLE advertisement...");
}

void loop() {
  // Do nothing here, just keep broadcasting
}
