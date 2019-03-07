using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurboXInput.Core;
using vJoyInterfaceWrap;
using Xunit;

namespace TurboXInput.Test
{
    public class VJoyServiceTest
    {

        private vJoy vjoy = new vJoy();

        [Fact]
        public void ShouldCheckVJoyEnabled()
        {
            Assert.True(this.vjoy.vJoyEnabled());
        }

        [Fact]
        public void ShouldCheckMatchingVersion()
        {
            uint dllVersion = 0, driverVersion = 0;
            var match = this.vjoy.DriverMatch(ref dllVersion, ref driverVersion);

            Assert.True(dllVersion == driverVersion);
            Assert.True(match);
        }

        [Theory]
        [InlineData(1)]
        public void ShouldGetStatus(uint id)
        {
            var status = this.vjoy.GetVJDStatus(id);

            Assert.Equal(VjdStat.VJD_STAT_FREE, status);
        }

        [Fact]
        public void ShouldGetFreeIds()
        {
            var freeIds = this.vjoy.GetFreeIds();

            Assert.Single(freeIds);
            Assert.Equal((uint) 1, freeIds.First());
        }

        [Theory]
        [InlineData(1)]
        public void ShouldAcquireVJoy(uint id)
        {
            this.vjoy.AcquireVJD(id);

            var vjoyStatus = this.vjoy.GetVJDStatus(id);
            Assert.Equal(VjdStat.VJD_STAT_OWN, vjoyStatus);
        }

        [Theory]
        [InlineData(1)]
        public void ShouldRelinquishVJoy(uint id)
        {
            this.vjoy.AcquireVJD(id);

            var vjoyStatus = this.vjoy.GetVJDStatus(id);
            Assert.Equal(VjdStat.VJD_STAT_OWN, vjoyStatus);

            this.vjoy.RelinquishVJD(id);

            vjoyStatus = this.vjoy.GetVJDStatus(id);
            Assert.Equal(VjdStat.VJD_STAT_FREE, vjoyStatus);
        }

    }
}
