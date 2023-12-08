# Configuration file in JSON Format

The Configuration contains information for the SprayingSystem.
You can think of the configuration file as the "User Options" file.

With this file you can configure the IP address of the Raspberry Pi.
The app uses this IP address to connected with the Raspberry Pi.
You also provide a timeout (in seconds) value that indicates communication failure.
There is communication failure when the raspberry Pi doesn't respond within timeout time.

For the camera, the system expects to have a folder path where to store images.
This folder must exist (please create one if you are setting the system the first time).
Image files will be saved under this file.

For the Robot, there will be locations.  Plesae ignore this for version 0.1.

## Example configuration

{
  "camera": {
    "temp_folder": "c:/temp/spraying_system/"
  },

  "rpi": {
    "ip_address": "127.0.0.1",
    "port": 8081,
    "message_timeout_sec": 3
  },

  "robot": {
    "locations": [
      {
        "name": "Home",
        "location": "0 ,0, 0, 0"
      },
      {
        "name": "Load_grid",
        "location": "10, 5, 0, 0"
      },
      {
        "name": "Nitrogen",
        "location": "10, 5, -10, 0"
      },
      {
        "name": "Bath",
        "location": "10, 7, -8, 0"
      }
    ]
  }
}

## Documention Revision

piTree Software
Luis Allegri
2022-02-22