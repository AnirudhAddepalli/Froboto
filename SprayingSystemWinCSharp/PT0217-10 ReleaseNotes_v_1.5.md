# Release Notes v 1.5

2022-12-08

## Requirements from NIEH

// The RPI should be finished.
//   [] Blot
//      () Blot to grid (position also)
//      () OR Grid to blot (position also)
//
if (ProcessOptionsViewModel.Blot)
{
    // if blot_to_grid_option
    //      Move the robot to blot_to_grid
    //      Wait for timeout_blot_motion
    //      Fire the solenoid for the blot_time
    //
    // if grid_to_blot_option
    //      Move the robot to grid_to_blot
    //      wait for timeout_blot_motion
    //      wait for blot_time
}

## SprayingSystemConfig.json

There are two new locations:

Blot_to_grid and Grid_to_blot
You need to teach these new points.
Then, you have to enter the new Point # (I have used 5 and 6).

If you click on Blot, then you can select either Blot to Grid or Grid to Blot.
Make sure you teach these two new locations.
The timeout blot motion, is the time that the robot takes to complete the motion for these two new moves.

## New POST message sent to RPI (set-blotsolenoid)

The App will send a command to the RPI to turn on the Blot Solenoid.
The RPI will then receive the message and then fire the solenoid for a short time.

## END
