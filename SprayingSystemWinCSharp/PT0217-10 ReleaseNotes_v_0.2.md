# Release Notes v 0.2 Spraying System

This prototype release includes the integration of the Epson RC+ 7.0.

venkata.dandey@nih.gov; wyatt.peele@nih.gov

## Install Epson RC+ 7.0

The Epson RC+ 7.0 Software must be installed for the new code to work.
Please ask Venkata to install the software if the system does not have it.

## Configure the Robot

Luis has worked with the simulator. The simulator configuration was defined
with the help from Epson representatives.
Luis does not know how to configure the Epson system.

## Robot settings in SprapyingSystemConfig.json

This json file is a text file that you should edit to setup a real robot.
I provide the "id" and the "project" fields to be modified.

The "id" field is 9 for my robot simulator. The actual robot may have another number?

The "project" file is a project file that the installation provided.
For a real robot, we may have a different project file. TBD.

The "simulation" field should be changed from true to false to run
with a real robot. It is true to run with the simulator, please change it.

The "locations" values should not be modified. The system is not using them right now.

## Define points (locations) for motion (real or simulator)

Once your simulator is configured, you need to define 4 points in the simulator.
Use the EPSON RC+ 7.0 software and the define the 4 points.

Point 1 is Home
Point 2 is a standby location. This is a point that can be used to adjust twizers?
Point 3 is a location just before the spray unit.
Point 4 is a location that is after the pray unit.
A spraying command will move the robot from Before spray to After srapy location.

For a real robot you will need to define the 4 locations as well.

## Run with the UI buttons

First Connect to the robot -- this will have a long time, wait until the UI is responsive.
You may have to wait for about a minute.

Then you can move by pressing the buttons. DO not press the buttons too fast.
Wait for the motion to complete.

If you run with the simulator the screen will display a small window showing
a 3D rendering of the robot and you will see the motion there.
This window will pop-up in the center of the screen and usually hides behind the UI.
Just move the UI window to search for the robot simulator.

## This is a very fragile version

Please contact me if you problems.
Once we get the physical robot connected then we will make the software more robust.

2022-07-19

