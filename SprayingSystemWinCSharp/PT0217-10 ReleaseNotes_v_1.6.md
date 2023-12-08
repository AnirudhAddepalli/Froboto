# Release Notes v 1.6

2023-04-06

## Requirements from NIEH

Email from Wyatt on 2023-04-07

-	Adding a power on button next to the robot connection button that will turn on the
	motors and set them to high power.

-	Fixing the configuration so that the speeds and accelerations can be properly set.

-	Maybe fixing the issue where it doesn’t print “connected” in the log if connection 
	is successful and maybe checking why its not printing “connection failed” when the 
	connection fails.

## JSON Config Changes

New default motion parameters.
Note that the speed is applied after the motor is powered on. But since each position
has its own speed settings, this global speed is superseeded by the location's setting.
But, accel and decel will apply to all motions. There is no configuration to change that.

```json
{
   "defaultMotionParams": {
      "speed": 100,
      "accel": 70,
      "decel": 70 
    },
}
```

## Main App UI Changes

Added a button to turn power on and to set High Power.
Please wait for the status to confirm that all succeeded.

## Robot Driver Changes

The drivers use small delays to ensure that the command is processed.
Setting the speed uses a small dalay. I don't know if that is necessary (10msecs)

**TODO:** Read the robot manual to check when the robot is available to reaceive a command.
Or find out how to check if a command finished (set speed, set accel, move, power on, high power on).

## Camera Driver Changes

None.

## RPI Driver Changes

None.

## RPI Code (Python)

None.

## END