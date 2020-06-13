using Newtonsoft.Json;
using ScpDriverInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        static bool isCreated = false;

        static int[] pressedButtons = new int[8];

        HidService hidService;
        
        ScpX360Service scpService;

        Configurations configs;

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

            using (StreamReader r = new StreamReader("W2XConfigs.json"))
            {
                string json = r.ReadToEnd();
                configs = JsonConvert.DeserializeObject<Configurations>(json);
            }

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

            foreach(var kvp in configs.ButtonIdentities)
            {
                var buttonId = kvp.Value;
                if (args.Changes[buttonId.Index] == buttonId.Value)
                {
                    pressedButtons[buttonId.Index] = buttonId.Value;
                    pressed += buttonId.OutputButton.ToString();
                    newController.Buttons ^= buttonId.OutputButton;
                }
                if (args.Changes[buttonId.Index] == -buttonId.Value)
                {
                    pressedButtons[buttonId.Index] = 0;
                    unpressed += buttonId.OutputButton.ToString();
                    newController.Buttons &= ~buttonId.OutputButton;
                }
            }

            #endregion

            #region Wheel binding

            var wheelIdentity = configs.AxisIdentities["Wheel"];
            var currentSector = args.FullReport[wheelIdentity.SectorIndex.Value] - pressedButtons[wheelIdentity.SectorIndex.Value];

            var currentWheelValue = args.FullReport[wheelIdentity.Index];
            short finalWheelValue;

            if (currentSector < 2)
            {
                short value = (short)((-510 + currentWheelValue + (255 * currentSector)) * WHEEL_AXIS_TRANSFORM * configs.SteeringSensitivity);
                finalWheelValue = (short)(value > 0? -32767 : value);
            }
            else
            {
                short value = (short)((currentWheelValue + (255 * (currentSector - 2))) * WHEEL_AXIS_TRANSFORM * configs.SteeringSensitivity);
                finalWheelValue = (short)(value < 0? 32767 : value);
            }

            newController.LeftStickX = finalWheelValue;

            // log axis state
            Console.Write($"LeftStickX: {newController.LeftStickX} | ");

            #endregion

            #region Pedal bindings

            for (int i=1; i<3; i++)
            {
                var axis = configs.AxisIdentities.ElementAt(i).Value;
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
