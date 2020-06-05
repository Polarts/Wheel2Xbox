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
                }
            };

            #endregion

            hidService.InputReceived += onInputReceived;
        }

        #endregion

        #region Methods

        private void onInputReceived(InputReportChangeEventArgs args)
        {
            var buttonsReport = scpService.Buttons;
            // for logging purposes
            string pressed = "", 
                   unpressed = "";

            buttonIdentities.ForEach(buttonId =>
            {
                if (args.Changes[buttonId.Index] == buttonId.Value)
                {
                    pressed += buttonId.OutputButton.ToString();
                    buttonsReport ^= buttonId.OutputButton;
                }
                if (args.Changes[buttonId.Index] == -buttonId.Value)
                {
                    unpressed += buttonId.OutputButton.ToString();
                    buttonsReport &= ~buttonId.OutputButton;
                }
            });
            scpService.Buttons = buttonsReport;
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
