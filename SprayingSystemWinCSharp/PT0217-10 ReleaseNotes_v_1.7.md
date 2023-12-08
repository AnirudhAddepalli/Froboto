# Release Notes v 1.7

2023-05-11


This version implements the following requirements:
R01, R02, R03, R04, R05, R06, R07

We will be implementing more requirements:
R08

## Current operation

- User must select check box [] RPI video recording
- User must press plunge
- If RPI video recording enabled then 
	- ask RPI to start video recording
	- delay 2 seconds for RPI start video recording
- Perform plunge tasks
- If RPI video recroding enabled then
	- delay 1 second
	- Ask RPI to send video
	- Save the received video under:	
		- <folder from configuration>
		- file name: <date>_<time>.mp4
	- Plays the last video received.
- User can press [PLAY] to replay the last video.
- Information is updated in the LOG page with video file name.

## Requirements 

### Requirements from NIEH (Venkat)

R01 
	- The function that should be called is cryocam.cryocam.capture_movie(run_name=”myrun”)

R02 
	- It will create a directory in /home/pi/picam_data/myrun/ and the final movie 
	to retrieve is going to be named movie.mp4 in that directory. 

R03
	- If the directory already exists, it will delete everything and overwrite 
	the previous data.

### Additional Functional Requirements (piTree)

R04 
	Save the image under a folder - from configuration.
	Save the image file name as:  <date><time><rpi_video.mp4>
	- rpi.video_folder: "c:/temp/"

R05
	Add delay after the user presses Plunge to get the RPI ready.
	Typically that delay is going to be 2 seconds?
	- rpi.video_startup_delay_msec = 2500

R06
	Add a small delay before retrieving the image to give time for completing the video.
	Alternative is to retrieve the video when the user presses play - Not done.
	- rpi.video_saving_delay_msec: 1000

R07
	When user presses PLAY button, play the last received video.

R09
	Play last video received immediately after receiving it.

R08
	The Python code has to record video in a separate thread
	so it doesn't interfere with the event loop for http-messaging.

	[] Use a thread to spawn the video acquisition code.

## Investigate

[done] RPI to send an MP4 file.
[done] Ask RPI to send an MP4 file.
[done] Download a text file. Check that the data is integral.
[done] WPF to playback an MP4 video.

## Design / Implementation

Procedure
	- RpiViewModel.StartRecordingCmd.Execute(null)
	- RpiViewModel.GetVideoFileCmd.Execute(null)
	Retrieve a video file.
	Save it with the timestamp when it was retrieved.
	Play the video file just retrieved

Classes
	IRpiDriver
		Interface for the set of communication commands that are implemented
		to monitor/control a Raspberry Pi which acts as the hardware 
		sensor/actuator interface for the Jet Spraying system.
	RpiDriver
		Implements IRpiDriver interface
	RpiController
		New messages
	RpiViewModel
		Commands for new messages
		User options for video delays.
	SprayingSystemConfig
		Rpi delays and folder name
	RpiConfig
		video folder name
		Delays
Files
	SprayingSystemConfig.json

WPF element for playback
```xml
    <Grid>
        <MediaElement 
            Source="c:/temp/video4.mp4" 
			Stretch="Uniform" 
			LoadedBehavior="Manual"
			Name="videoPlayer"/>
    </Grid>
```

videoPlayer.Play(), videoPlayer().Pause(), videoPlayer.Stop()

## JSON Config Changes

delay_to_start_video_recording_secs = 2.5

## Main App UI Changes

Checkbox to enable recording at the Raspberry PI and
to retrieve the video and play back in the WIndows UI.

## Robot Driver Changes

None.

## Camera Driver Changes

None.

## RPI Driver Changes

Added two commands for starting video recording, and for getting recorded video.

## RPI Code (Python)

Add two command handlers.
Add call to cryocam module.

## END
