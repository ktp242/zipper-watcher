using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;

namespace TestApp01
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public IStream connection;
        public RemoteDevice arduino;

        public void SetupRemoteArduino()
        {
            this.InitializeComponent();
            //create a bluetooth connection and pass it to the RemoteDevice
            //I am using a constructor that accepts a device name or ID.

            ushort port = System.Convert.ToUInt16("your host port");

            connection = new NetworkSerial(new Windows.Networking.HostName("Your host name"), port);
            arduino = new RemoteDevice(connection);

            //add a callback method (delegate) to be invoked when the device is ready, refer to the Events section for more info
            arduino.DeviceReady += Setup;

            //always begin your IStream
            connection.begin(115200, SerialConfig.SERIAL_8N1);
        }

        //treat this function like "setup()" in an Arduino sketch.
        public void Setup()
        {
            //set digital pin 13 to OUTPUT
            arduino.pinMode(13, PinMode.OUTPUT);

            //set analog pin 3 to INPUT
            arduino.pinMode(3, PinMode.INPUT);
        }

        //This function will read a value from our DIGITAL INPUT pin 3
        public void ReadAndReport()
        {
            PinState val = arduino.digitalRead(3);
            if (val == PinState.HIGH)
            {
                arduino.digitalWrite(13, PinState.HIGH);
            }
            else
            {
                arduino.digitalWrite(13, PinState.LOW);
            }
        }
    }
}
