using ScpDriverInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wheel2Xbox.Services
{
    public class ScpX360Service
    {
        #region Fields

        ScpBus bus;

        X360Controller controller;

        static bool isCreated = false;

        #endregion


        #region Ctor and Factory

        public static ScpX360Service Create()
        {
            if (isCreated)
                throw new InvalidOperationException("You cannot create more than one ScpX360Service");

            return new ScpX360Service();
        }

        private ScpX360Service()
        {
            bus = new ScpBus();
            controller = new X360Controller();

            bus.PlugIn(1);
        }

        #endregion


        #region Methods

        #endregion
    }
}
