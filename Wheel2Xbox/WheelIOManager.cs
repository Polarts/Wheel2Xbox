using ScpDriverInterface;
using System;
using System.Collections.Generic;
using System.Text;
using Wheel2Xbox.Services;
using Wheel2Xbox.Types;

namespace Wheel2Xbox
{
    public class WheelIOManager
    {
        #region Constants

        const double WHEEL_AXIS_TRANSFORM = 32767 / 510;

        #endregion

        #region Fields

        HidService hidService;
        
        ScpX360Service scpService;

        static bool isCreated = false;

        List<ButtonIdentity> buttonIdentities;

        List<AxisIdentity> axisIdentities;

        #endregion
        
        #region Ctor and Factory

        public static WheelIOManager Create()
        {
            if (isCreated)
                throw new InvalidOperationException("You cannot create more than one WheelIOManager");

            isCreated = true;
            return new WheelIOManager();
        }

        private WheelIOManager()
        {
            hidService = HidService.Create();
            scpService = ScpX360Service.Create();

            #region generate input identities

            buttonIdentities = new List<ButtonIdentity>
            {
                new ButtonIdentity
                {
                    Name = "1",
                    Index = 2,
                    Value = 4,
                    OutputButton = X360Buttons.X
                },
                new ButtonIdentity
                {
                    Name = "2",
                    Index = 2,
                    Value = 8,
                    OutputButton = X360Buttons.A
                },
                new ButtonIdentity
                {
                    Name = "3",
                    Index = 2,
                    Value = 16,
                    OutputButton = X360Buttons.B
                },
                new ButtonIdentity
                {
                    Name = "4",
                    Index = 2,
                    Value = 32,
                    OutputButton = X360Buttons.Y
                },
                new ButtonIdentity
                {
                    Name = "5",
                    Index = 2,
                    Value = 64,
                    OutputButton = X360Buttons.LeftBumper
                },
                new ButtonIdentity
                {
                    Name = "6",
                    Index = 2,
                    Value = 128,
                    OutputButton = X360Buttons.RightBumper
                },
                new ButtonIdentity
                {
                    Name = "7",
                    Index = 3,
                    Value = 1,
                    OutputButton = X360Buttons.LeftStick
                },
                new ButtonIdentity
                {
                    Name = "8",
                    Index = 3,
                    Value = 2,
                    OutputButton = X360Buttons.RightStick
                },
                new ButtonIdentity
                {
                    Name = "9",
                    Index = 3,
                    Value = 4,
                    OutputButton = X360Buttons.Back
                },
                new ButtonIdentity
                {
                    Name = "10",
                    Index = 3,
                    Value = 8,
                    OutputButton = X360Buttons.Start
                },
                new ButtonIdentity
                {
                    Name = "11",
                    Index = 3,
                    Value = 16,
                    OutputButton = X360Buttons.Back
                },
                new ButtonIdentity
                {
                    Name = "12",
                    Index = 3,
                    Value = 32,
                    OutputButton = X360Buttons.Start
                },
                new ButtonIdentity
                {
                    Name = "Left",
                    Index = 5,
                    Value = -2,
                    OutputButton = X360Buttons.Left
                },
                new ButtonIdentity
                {
                    Name = "Up",
                    Index = 5,
                    Value = -8,
                    OutputButton = X360Buttons.Up
                },
                new ButtonIdentity
                {
                    Name = "Right",
                    Index = 5,
                    Value = -6,
                    OutputButton = X360Buttons.Right
                },
                new ButtonIdentity
                {
                    Name = "Down",
                    Index = 5,
                    Value = -4,
                    OutputButton = X360Buttons.Down
                }
            };

            axisIdentities = new List<AxisIdentity>
            {
                new AxisIdentity
                {
                    Name = "Wheel",
                    Index = 1,
                    SectorIndex = 2,
                    OutputAxis = X360Axis.LeftStickX
                },
                new AxisIdentity
                {
                    Name = "Left Pedal",
                    Index = 7,
                    OutputAxis = X360Axis.RightTrigger
                },
                new AxisIdentity
                {
                    Name = "Right Pedal",
                    Index = 6,
                    OutputAxis = X360Axis.LeftTrigger
                }
            };

            #endregion

            hidService.InputReceived += onInputReceived;
        }

        #endregion

        #region Methods

        private void onInputReceived(InputReportChangeEventArgs args)
        {
            var newController = new X360Controller(scpService.Controller);
            // for logging purposes
            string pressed = "", 
                   unpressed = "";

            #region Button bindings

            buttonIdentities.ForEach(buttonId =>
            {
                if (args.Changes[buttonId.Index] == buttonId.Value)
                {
                    pressed += buttonId.OutputButton.ToString();
                    newController.Buttons ^= buttonId.OutputButton;
                }
                if (args.Changes[buttonId.Index] == -buttonId.Value)
                {
                    unpressed += buttonId.OutputButton.ToString();
                    newController.Buttons &= ~buttonId.OutputButton;
                }
            });

            #endregion

            #region Wheel binding

            var wheelIdentity = axisIdentities[0];
            var currentSector = args.FullReport[wheelIdentity.SectorIndex.Value] - args.Changes[wheelIdentity.SectorIndex.Value];

            var currentWheelValue = args.FullReport[wheelIdentity.Index];
            short finalWheelValue = 0;

            if (currentSector < 2)
            {
                finalWheelValue = (short)((-510 + currentWheelValue + (255 * currentSector)) * WHEEL_AXIS_TRANSFORM);
            }
            else
            {
                finalWheelValue = (short)((currentWheelValue + (255 * (currentSector - 2))) * WHEEL_AXIS_TRANSFORM);
            }


            newController.LeftStickX = finalWheelValue;
            // log axis state
            Console.Write($"LeftStickX: {newController.LeftStickX} | ");

            #endregion

            #region Pedal bindings

            for (int i=1; i<3; i++)
            {
                var axis = axisIdentities[i];
                var currentValue = (byte)(255 - args.FullReport[axis.Index]);
                switch (axis.OutputAxis)
                {
                    case X360Axis.LeftTrigger:
                        newController.LeftTrigger = currentValue;
                        break;

                    case X360Axis.RightTrigger:
                        newController.RightTrigger = currentValue;
                        break;
                }

                //log axis state
                Console.Write($"{axis.OutputAxis}: {currentValue} | ");
            }

            #endregion

            scpService.Controller = newController;
            // log the button states
            Console.WriteLine($"Pressed: {pressed} | Unpressed: {unpressed}");
        }

        public void Stop()
        {
            scpService.Unplug();
        }

        #endregion
    }
}
