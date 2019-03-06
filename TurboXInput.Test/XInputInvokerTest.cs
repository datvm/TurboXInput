using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TurboXInput.Core;
using Xunit;

namespace TurboXInput.Test
{

    public class XInputInvokerTest
    {

        [Fact]
        public void ShouldEnableAppXInput()
        {
            XInputInvoker.XInputEnable(true);
        }

        [Theory]
        [InlineData(0)]
        public void ShouldGetControllerCapacity(uint index)
        {
            var result = new XInputCapabilities();
            var opResult = XInputInvoker.XInputGetCapabilities(index, XInputGetCapabilitiesFlag.XINPUT_FLAG_GAMEPAD, ref result);

            Assert.Equal(XInputOpResult.Success, opResult);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ShouldNotGetControllerCapacity(uint index)
        {
            var result = new XInputCapabilities();
            var opResult = XInputInvoker.XInputGetCapabilities(index, XInputGetCapabilitiesFlag.XINPUT_FLAG_GAMEPAD, ref result);

            Assert.Equal(XInputOpResult.Error_DeviceNotConnected, opResult);
        }

        [Theory]
        [InlineData(0)]
        public void ShouldGetCurrentState(uint index)
        {
            var result = new XInputState();
            var opResult = XInputInvoker.XInputGetState(index, ref result);

            Assert.Equal(XInputOpResult.Success, opResult);
        }

    }

}
