using EscapeMines.Common.Enums;
using System;
using System.Collections.Generic;

namespace EscapeMines.IO.Extensions
{
    public static class OutputExtensions
    {
        private static IDictionary<TurtleState, string> TurtleStateConsole = new Dictionary<TurtleState, string>
        {
            [TurtleState.ExitReached] = "Success",
            [TurtleState.Exploded] = "Mine Hit",
            [TurtleState.InDanger] = "Still in Danger"
        };

        public static void ToConsole(this TurtleState state)
        {
            Console.WriteLine(TurtleStateConsole[state]);
        }
    }
}
