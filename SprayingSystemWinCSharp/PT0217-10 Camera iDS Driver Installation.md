# iDS Camera Installation

I installed the iDS camera driver as described below in my Windows 11 computer.
I use a USB hub attached to my computer.
The USB hub powered the camera through a USB3 port (the USB port is blue-ish).
I downloaded the software from iDS as described below. It was not easy to locate the driver.

I also tried the same driver that I used at NYSBC (New York Structural Biology Center).
The uEye Software Suite installed but I could not see the camera USB driver.
I also tried to install first the ids_peak then the uEye Software Suite. Did not work.

## Install iDS Peak Software

Download ids_peak_1.3.1.0.exe file from the iDS website.
You must register to be able to download.
I am including the installation in the release of Spraying System.

## Installation Procedure

•	Make sure the camera is NOT connected. If it is, disconnect it.
•	Run the installation exe: ids_peak_1.3.1.0.exe
•	Connect the camera (plug through a USB 3.0 port). The port powers the camera as well.
•	Verify that the camera is connected: Use Windows’ Device Manager

## Verifying that the camera is in Device Manager

Open "Device Manager" in Windows.

Expand "Imaging Devices" group.
If this group does not exist then the camera may not have installed or is not plugged in.

The "iDS USB3 Vision Core Device" should be in the devices tree.
This device is dated: 2021-05-26

The USB3 Vision Core device had the following Driver files:
c:\WINDOWS\system32\DRIVERS\ids_u3vcore.sys
c:\WINDOWS\system32\drivers\ksthunk.sys

## Documention Revision

piTree Software
Luis Allegri
2022-02-22

## END