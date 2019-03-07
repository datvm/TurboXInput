using System;
using System.Collections.Generic;
using System.Text;

namespace TurboXInput.Core
{

    public static class XInputExtensions
    {

        public static IEnumerable<XInputGamepadButton> GetIncludedButtons(this XInputGamepadButton buttons)
        {
            var result = new List<XInputGamepadButton>();

            foreach (XInputGamepadButton button in Enum.GetValues(typeof(XInputGamepadButton)))
            {
                if (buttons.HasFlag(button))
                {
                    result.Add(button);
                }
            }

            return result;
        }

        public static bool HasButton(this XInputGamepadButton buttons, XInputGamepadButton button)
        {
            return (ushort)(buttons & button) == 1;
        }

    }

}
