using System;
using System.Threading;
using SprayingSystem.Models;
using SprayingSystem.ViewModels;

namespace SprayingSystem.SprayingProcess
{
    public class SprayAndPlungeProcess
    {
        public static void Process(
            ProcessOptionsViewModel ProcessOptionsViewModel,
            RobotVariablesViewModel RobotVariablesViewModel,
            RobotVariablesModel RobotVariablesModel,
            RobotViewModel RobotViewModel,
            CameraViewModel CameraViewModel,
            RpiViewModel RpiViewModel,
            Action<string> logger)
        {
            logger("Process: Spray And Plunge");

            if (ProcessOptionsViewModel.RecordSpray)
                CameraViewModel.StartRecordingCmd.Execute(null);

            // The Raspberry Pi has a camera that can take pictures.
            if (ProcessOptionsViewModel.RpiRecordSpray)
            {
                RpiViewModel.StartVideoRecordingSprayCmd.Execute(null);
                Thread.Sleep(RpiViewModel.VideoStartUpDelayMillisec);
            }

            // this time should be only the spray time + preparation delay
            int totalWaitTime = TotalTimeToWaitForSpraying(ProcessOptionsViewModel, RobotVariablesModel, RpiViewModel);

            if (ProcessOptionsViewModel.Spray)
            {
                logger("Process: Spraying");
                //RpiViewModel.SprayCmd.Execute(null);  
                Thread.Sleep(totalWaitTime);
                RobotViewModel.sprayCommand(null); 
                
                RobotViewModel.blotSolenoidCommand(null);
                
                //RpiViewModel.BlotSolenoidReverseCmd.Execute(null);
                Thread.Sleep(int.Parse(RobotVariablesModel.BlotTime));
            }

            if (ProcessOptionsViewModel.Blot)
            {
                int timeoutBlotMotion = int.Parse(RobotVariablesModel.TimeoutBlotMotion);

                if (ProcessOptionsViewModel.Blot_BackBlot)
                {
                    logger("Process: Back Blotting");
                    RobotViewModel.MoveToBackBlotPosition(null);
                    //Thread.Sleep(timeoutBlotMotion);

                    //RpiViewModel.BlotSolenoidCmd.Execute(null);
                    RobotViewModel.blotSolenoidCommand(null);

                    // Wait for the solenoid to actuate.
                    Thread.Sleep(int.Parse(RobotVariablesModel.BlotTime));
                }
                else
                {
                    // ProcessOptionsViewModel.Blot_GridToBlot
                    logger("Process: Front Blotting");
                    RobotViewModel.MoveToFrontBlotPosition(null);
                    Thread.Sleep(timeoutBlotMotion);
                    //RpiViewModel.BlotSolenoidCmd.Execute(null);
                    RobotViewModel.blotSolenoidCommand(null);
                    Thread.Sleep(int.Parse(RobotVariablesModel.BlotTime));
                    RobotViewModel.MoveToBackBlotPosition(null);
                }
            }

            RobotViewModel.MoveToPlungePosition(null);

            if (ProcessOptionsViewModel.RecordSpray)
            {
                CameraViewModel.StopRecordingCmd.Execute(null);
                CameraViewModel.SaveVideoCmd.Execute(null);
                logger("Process: Video recorded and saved.");
            }

            if (ProcessOptionsViewModel.RpiRecordSpray)
            {
                // TODO: How to make sure the image was acquired and it is saved on file.
                Thread.Sleep(RpiViewModel.VideoSavingDelayMillisec);
                RpiViewModel.GetVideoFileCmd.Execute(null);
                // Play video from here?
                RpiViewModel.PlayVideoCmd.Execute(null);
            }

            RobotVariablesViewModel.WriteToLog();
        }

        public static int TotalTimeToWaitForSpraying(
            ProcessOptionsViewModel processOptionsViewModel,
            RobotVariablesModel robotVariablesModel,
            RpiViewModel rpiViewModel)
        {
            int totalWaitTime = 0;

            if (processOptionsViewModel.Spray)
            {
                totalWaitTime += rpiViewModel.SprayPreparationDelay;
                //totalWaitTime += GetSprayTime(processOptionsViewModel, robotVariablesModel, rpiViewModel);
            }

            //totalWaitTime += GetBlotTime(processOptionsViewModel, robotVariablesModel);

            return totalWaitTime;
        }

        private static int GetSprayTime(
            ProcessOptionsViewModel processOptionsViewModel,
            RobotVariablesModel robotVariablesModel,
            RpiViewModel rpiViewModel)
        {
            if (processOptionsViewModel.Spray)
            {
                if (!string.IsNullOrEmpty(robotVariablesModel.SprayTime))
                    return int.Parse(robotVariablesModel.SprayTime);
            }

            return 0;
        }

        private static int GetBlotTime(
            ProcessOptionsViewModel processOptionsViewModel,
            RobotVariablesModel robotVariablesModel)
        {
            if (processOptionsViewModel.Blot)
            {
                if (!string.IsNullOrEmpty(robotVariablesModel.BlotTime))
                    return int.Parse(robotVariablesModel.BlotTime);
            }

            return 0;
        }
    }
}
