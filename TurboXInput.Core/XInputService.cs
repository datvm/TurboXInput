using System;
using System.Collections.Generic;
using System.Text;

namespace TurboXInput.Core
{

    public interface IXInputService
    {
        IEnumerable<uint> GetControllers();
        XInputState GetControllerState(uint controllerId);
    }

    public class XInputService : IXInputService
    {
        const uint MaxScannedPlayer = 5;

        public IEnumerable<uint> GetControllers()
        {
            var result = new List<uint>();

            for (uint i = 0; i < MaxScannedPlayer; i++)
            {
                var cab = new XInputCapabilities();
                var avail = XInputInvoker.XInputGetCapabilities(i, XInputGetCapabilitiesFlag.XINPUT_FLAG_GAMEPAD, ref cab);

                if (avail == XInputOpResult.Success)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        public XInputState GetControllerState(uint controllerId)
        {
            var result = new XInputState();
            var opResult = XInputInvoker.XInputGetState(controllerId, ref result);

            switch (opResult)
            {
                case XInputOpResult.Success:
                    return result;
                case XInputOpResult.Error_DeviceNotConnected:
                    throw new TurboInputException(TurboInputExceptionCode.DeviceNotConnected);
                default:
                    throw new TurboInputException(TurboInputExceptionCode.UnexpectedError, "Error Code: " + opResult.ToString());
            }
        }

    }

}
