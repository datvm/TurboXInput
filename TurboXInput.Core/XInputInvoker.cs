using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TurboXInput.Core
{

    public static class XInputInvoker
    {
        private const string XInputDllName = "XINPUT1_4.DLL";

        [DllImport(XInputDllName)]
        public static extern void XInputEnable(bool enabled);

        [DllImport(XInputDllName)]
        public static extern XInputGetCapabilitiesResult XInputGetCapabilities(uint dwUserIndex, XInputGetCapabilitiesFlag dwFlags, ref XInputCapabilities pCapabilities);

    }

    [Flags]
    public enum XInputGetCapabilitiesResult : uint
    {
        ERROR_SUCCESS = 0,
        ERROR_DEVICE_NOT_CONNECTED = 0x48F,
    }

    [Flags]
    public enum XInputGetCapabilitiesFlag : uint
    {
        XINPUT_FLAG_GAMEPAD,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputCapabilities
    {
        public byte Type;
        public byte SubType;
        public ushort Flags;
        public XInputGamepad Gamepad;
        public XInputVibration Vibration;

        [Flags]
        public enum XInputCapabilitiesFlag
        {
            XINPUT_CAPS_VOICE_SUPPORTED,
            XINPUT_CAPS_FFB_SUPPORTED,
            XINPUT_CAPS_WIRELESS,
            XINPUT_CAPS_PMD_SUPPORTED,
            XINPUT_CAPS_NO_NAVIGATION,
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputGamepad
    {
        public XInputGamepadFlag wButtons;
        byte bLeftTrigger;
        byte bRightTrigger;
        short sThumbLX;
        short sThumbLY;
        short sThumbRX;
        short sThumbRY;

        [Flags]
        public enum XInputGamepadFlag
        {
            DPadUp = 0x00000001,
            DPadDown = 0x00000002,
            DPadLeft = 0x00000004,
            DPadRight = 0x00000008,
            Start = 0x00000010,
            Back = 0x00000020,
            LeftThumb = 0x00000040,
            RightThumb = 0x00000080,
            LeftShoulder = 0x0100,
            RightShoulder = 0x0200,
            Guide = 0x0400,
            A = 0x1000,
            B = 0x2000,
            X = 0x4000,
            Y = 0x8000
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputVibration
    {
        ushort wLeftMotorSpeed;
        ushort wRightMotorSpeed;
    }

}
