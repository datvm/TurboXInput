using System;
using System.Collections.Generic;
using System.Text;

namespace TurboXInput.Core
{

    public static class XInputExtensions
    {

        public static IEnumerable<XInputGamepadFlag> GetIncludedButtons(this XInputGamepadFlag buttons)
        {
            var result = new List<XInputGamepadFlag>();

            foreach (XInputGamepadFlag button in Enum.GetValues(typeof(XInputGamepadFlag)))
            {
                if (buttons.HasFlag(button))
                {
                    result.Add(button);
                }
            }

            return result;
        }

        public static bool HasButton(this XInputGamepadFlag buttons, XInputGamepadFlag button)
        {
            return (ushort)(buttons & button) == 1;
        }

    }

}
