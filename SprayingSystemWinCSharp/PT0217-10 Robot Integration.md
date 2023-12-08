# Robot Integration

We are integrating a robot control model EPSON RC+ 7.0.
The Spraying System will integrate the robot-driver (provided by the EPSON RC+ 7.0 software).

## Robot Driver Installation

I am not sure about what is the minimal installation.

The robot driver is in the EPSON installer.
Install the EPSON RC+ 7.0 installation provided by Venkata from NIEH/NIH

## Robot Driver Configuration

After the robot driver is installed, we have to configure how we work with the driver.
We also have to configure the settings for what type of robot we are controlling.

### Configuration of Simulator

Venkata put me in touch with his Robot Vendor to help configure the robot.
The Robot Vendor setup a conference call with an EPSON rep we they guide on the phone how to setup
I did not write step by step instructions to setup the simulator.
I will document here what I recall after the conference call.

### Configuration of Real Instrument

I don't now what the steps are to setup an actual instrument.
My system is running with a robot simulator. The configuration for the simulator maybe
different than from a real instrument?

### Save Config to File

Also I don't know how to share my configuration settings with another computer system.
What I mean with this is that I don't know how to save to a file my Robot Configuration, 
save to file the configuration which the EPSON reps helped me setup.

## Robot Driver Interfacing

The .NET code (C# code) interfaces through a robot driver DLL (RCAPINet).
This DLL is installed by the robot driver and it interfaces to the robot or simulator.

### C# code reference

The C# projet references must include a reference to: RCAPINet
RCAPINet.dll was installed when we installed the robot driver.

Path: c:\EpsonRC70\exe\RCAPINet.dll

Properties of the reference:
Copy Local: True
Embed Interop Types: False
File Type: Assembly
Identity: RCAPINet
Path: see above
Resolved: True
Runtime Version: v4.0.30319
Specific Version: False
Strong Name: True
Version: 1.0.0.0

### Spel.Project requirement to run simulator

We need to define a project file path suggested by the EPSON representatives:
Spel is a class we instantiate from RCAPINet.
Spel.Project is a string that must point to the simulator project provided by EPSON.
Spel.Project = @"C:\EpsonRC70\projects\API_Demos\Demo1\demo1.sprj"

We also have to set the connection type:
Spel.Connect(9);   
9 is the Connection Number
T3 is the Bunny name for my robot.
