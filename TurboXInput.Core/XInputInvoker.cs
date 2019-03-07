using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TurboXInput.Core
{

    public static class XInputInvoker
    {
        public static readonly XInputGamepadButton[] AllButtons = Enum.GetValues(typeof(XInputGamepadButton)).Cast<XInputGamepadButton>().ToArray();

        private const string XInputDllName = "XINPUT1_4.DLL";

        [DllImport(XInputDllName)]
        public static extern void XInputEnable(bool enabled);

        [DllImport(XInputDllName)]
        public static extern XInputOpResult XInputGetCapabilities(uint dwUserIndex, XInputGetCapabilitiesFlag dwFlags, ref XInputCapabilities pCapabilities);

        [DllImport(XInputDllName)]
        public static extern XInputOpResult XInputGetState(uint dwUserIndex, ref XInputState pState);

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputState
    {
        public uint dwPacketNumber;
        public XInputGamepad GamePad;
    }

    [Flags]
    public enum XInputOpResult : uint
    {
        Success = 0,
        Error_DeviceNotConnected = 0x48F,
    }

    [Flags]
    public enum XInputGetCapabilitiesFlag : uint
    {
        XINPUT_FLAG_GAMEPAD,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputCapabilities
    {
        public XInputDeviceTypeFlag Type;
        public XInputDeviceSubtypeFlag SubType;
        public XInputCapabilitiesFlag Flags;
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

    public enum XInputDeviceTypeFlag : byte
    {
        XINPUT_DEVTYPE_GAMEPAD,
    }

    public enum XInputDeviceSubtypeFlag : byte
    {
        XINPUT_DEVSUBTYPE_UNKNOWN,
        XINPUT_DEVSUBTYPE_GAMEPAD,
        XINPUT_DEVSUBTYPE_WHEEL,
        XINPUT_DEVSUBTYPE_ARCADE_STICK,
        XINPUT_DEVSUBTYPE_FLIGHT_STICK,
        XINPUT_DEVSUBTYPE_DANCE_PAD,
        XINPUT_DEVSUBTYPE_GUITAR,
        XINPUT_DEVSUBTYPE_GUITAR_ALTERNATE,
        XINPUT_DEVSUBTYPE_GUITAR_BASS,
        XINPUT_DEVSUBTYPE_DRUM_KIT,
        XINPUT_DEVSUBTYPE_ARCADE_PAD,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputGamepad
    {
        public XInputGamepadButton wButtons;
        public byte bLeftTrigger;
        public byte bRightTrigger;
        public short sThumbLX;
        public short sThumbLY;
        public short sThumbRX;
        public short sThumbRY;
    }

    [Flags]
    public enum XInputGamepadButton : ushort
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

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputVibration
    {
        ushort wLeftMotorSpeed;
        ushort wRightMotorSpeed;
    }

}
