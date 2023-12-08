# Release Notes v 1.2

## Requirements from NIEH

Items discussed 10/19/2022
* [done] Ability to change speed of each movement command
* [done] Ability to change movement type for each movement command
* [doing] Robot manager not returning keyboard use

Additional items
* [done] Default ip should be changed to 169.254.2.148
* [done] Default blot time should be set to 0
* [done] Default spray time should be set to 0
* [done] Add clean button to pulse the sprayer a couple of time (this was a function of the Rubinstein program so maybe we can just use it with fixed parameters we define in config) - RPI and PC code update.
* [] Maybe we can add the ability to include movement arguments for each movement command, like we have for gridStoreJump
* [] Blot time should be entered in seconds instead of milliseconds (Spray can stay ms)

## Items not addressed in this version

* [] Robot manager not returning keyboard use
I need to experiment with a new method of opening the Robot windows. 
I will create an isolated prototype for this and send the code for testing with the robot.

* [] Maybe we can add the ability to include movement arguments for each movement command, like we have for gridStoreJump
The Grid Store command can use the movement argument as designed because all 4 GritStore moves are the same kind of move.
However, for the other positions we may need different arguments for each point rather than for each type.
Each point is a move with a different purpose and they move to different areas of the work space, and may have the same type of move (go for example).

* [] Blot time should be entered in seconds instead of milliseconds (Spray can stay ms)
All times are currently entered in milliseconds.
Right now the Variables do not have a way to display the UNITS.
Each variable is generic. I would have to create units for all of them, then we can do this.

## Specifications - Speed and Motion Type

We are adding two fields to the robot locations. For each location in "locations",
we can provide the "speed" and "motiontype". Note that this variables must be 
written exactly as described, lower case.

The value of the speed is an integer value in the range from 0 to 100.
"speed": 0 - states that the system will use the speed set in the previous move.

The motiontype is a string value and can only be one of these: "move", "go", or "jump".
"motiontype": "go" - states that the system must use this motion type when moving to the point.

Valid motion types: "move", "go", "jump". 
Default motion type (if not specified): "go"

GridStore 1, 2, 3, and 4 will take motiontype: "move", "go", "jump"
Default motion type (if not specified): "jump"

Validation of data file
* Takes place when the file is loaded (i.e. at start of program)

Valid speed values: "0 ... 100"
* speed must be in the range 0 to 100 if none specified, default is 0.
* Error Recovery - when value out of range, program will stop.

Validate motiontype
* convert string to upper case
* string must either be: MOVE, GO, or JUMP. Only these can be used. 
* Error Recover - Stop program if value is not in list.

## Specifications - Command Cleaning Process (Clean Sprayer)

This command is enabled when the RPI is connected, and the Robot is connected.

The Robot Variables store the cleaning time and the cleaning cycles.
The cleaning time is default to 200 msecs.
The cleaning cycles is default to 5 cycles.
Both default values come from the Rubinstein (BUI) code.

The Clean command is sent to the RPI using a POST message with two parameters.
The RPI will receive the command and decode the two parameters and pass them to BUICleaning code.

The implementation of this feature requires changes to the C# code and to the Python code.

## END
