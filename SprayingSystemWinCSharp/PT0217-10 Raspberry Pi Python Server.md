# Raspberry Pi Python Server

The Spraying System has three major components: Windows PC, Robot, and Raspberry Pi.
The Windows PC and the Raspberry Pi communicate using HTTP protocol.
The Raspberry Pi acts as a server. It offers commands that clients can execute.
Windows PC acts as a client and makes calls to the server.

## Raspberry Pi Server

The Raspberry Pi server is Python code listening for HTTP messages.
The server requires an IP and Port to listen in. The code requires that.

There are a few messages that have been configured:
get-datetime
set-spray
set-led-on
set-led-off

Please read the code to see what each command is supposed to do.

set-spray command - this command should basically spray the grid.
Turn the sprayer on, wait a little bit of time, then turn the sprayer off.

## How to run the spray_server.py program

The spray_server.py program must be in a text file and stored in a folder in the Raspberry Pi.

The Raspberry Pi must be have at least Python 3.6 installed.
Check by typing this command at the console:
> python3 --version

Run the spray_server.py as follows:
> python3 spray_server.py

Then test the connection either using a Web-browser or the SprayingSystem app.
See sections below.

## Testing connectivity with SprayingSystem application

The Raspberry Pi must be running the "spray_server.py" program.

On the Windows PC computer, you will have to run the SprayingSystem app.
Note that the SprayingSystemConfig.json file must have been configured properly.
See PT0217-10 Configuration File

Start the SpryingSystem.exe program. 
Press "Connect" button for the Raspberry Pi.
See the status of the connection at the bottom of the window in the Status Bar.
Press the command buttons to Spray, LED on, LED off, and see the Raspberry Pi display corresponding messages.

## Testing connectivity with a Browser

When the Raspberry Pi is fully operational and the Spray_server.py is running, you can test using a browser.

Here are some queries you can make from the web-browser:
http://<ip address>:8081/get-datetime
http://<ip address>:8081/set-led-on
http://<ip address>:8081/set-led-off
http://<ip address>:8081/set-spray duration=30

## BIU python code

The BIU code is the Rubinstein lab's code.
https://github.com/johnrubinstein/BIUcontrol

## Documention Revision

piTree Software
Luis Allegri
2022-02-22 - A basic server
2022-09-28 - Revised with the integration of the BIU code.

## END