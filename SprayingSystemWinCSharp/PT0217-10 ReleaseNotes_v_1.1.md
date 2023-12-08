# Release Notes - v 1.1 Spraying system

## Grid Store positions use JUMP robot command to move to

Go to Grid Store position commands use the JUMP robot command.
This command can use a string as parameter.
The string must be defined in the JSON config file.

For example, to move with a 15 mm motion upwards to clear obstacle,
enter command = ":Z(-15)".
Depending on which position it will move to, the code will append the position.

The following configuration was added to the JSON file:
    "gridStoreJumpParameters": {
      "command" :  ":Z(-15)"
    }

## Spray and Plunge command update

Starts recording - optional
Sprays - optional
Delay (spray_time + blot_time)  -- optional spray_time, blot_time
Plunge
Stop recording - if turned on
