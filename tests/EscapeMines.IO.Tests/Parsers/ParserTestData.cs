using EscapeMines.Common;
using EscapeMines.Common.Enums;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace EscapeMines.IO.Tests.Parsers
{
    public class MinesTestData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { "1,1", true, new List<GameItem> { new GameItem(ItemType.Mine) { Position = new ItemPosition { X = 1, Y = 1 } } } };
            yield return new object[] { "11,-3 0,14", true, new List<GameItem> { new GameItem(ItemType.Mine) { Position = new ItemPosition { X = 11, Y = -3 } }, new GameItem(ItemType.Mine) { Position = new ItemPosition { X = 0, Y = 14 } } } };

            yield return new object[] { "", false, null };
            yield return new object[] { ",5 4,5", false, null };
            yield return new object[] { "4e5 -1,4", false, null };
            yield return new object[] { "5-6 333", false, null };
            yield return new object[] { "what?", false, null };
            yield return new object[] { "4,5,6,7,", false, null };
            yield return new object[] { "4,0 2,--6", false, null };
        }
    }

    public class ExitTestData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { "1 1", true, new GameItem(ItemType.Exit) { Position = new ItemPosition { X = 1, Y = 1 } } };
            yield return new object[] { "0 -5", true, new GameItem(ItemType.Exit) { Position = new ItemPosition { X = 0, Y = -5 } } };

            yield return new object[] { "", false, null };
            yield return new object[] { ",5 4,5", false, null };
            yield return new object[] { "4e5 -1,4", false, null };
            yield return new object[] { "5-6 333", false, null };
            yield return new object[] { "what?", false, null };
            yield return new object[] { "4,5", false, null };
            yield return new object[] { "4 --6", false, null };
        }
    }

    public class TurtleTestData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { "1 1 W", true, new Turtle { Position = new ItemPosition { X = 1, Y = 1 }, Direction = TurtleDirection.West } };
            yield return new object[] { "0 -5 E", true, new Turtle { Position = new ItemPosition { X = 0, Y = -5 }, Direction = TurtleDirection.East } };
            yield return new object[] { "0 0 N", true, new Turtle { Position = new ItemPosition { X = 0, Y = 0 }, Direction = TurtleDirection.North } };
            yield return new object[] { "-12 15 S", true, new Turtle { Position = new ItemPosition { X = -12, Y = 15 }, Direction = TurtleDirection.South } };

            yield return new object[] { "", false, null };
            yield return new object[] { "6 9 R", false, null };
            yield return new object[] { "N N 8", false, null };
            yield return new object[] { "4 1 WW", false, null };
            yield return new object[] { "what?", false, null };
            yield return new object[] { "3,4 S", false, null };
            yield return new object[] { "2 --6 N", false, null };
        }
    }

    public class CommandsTestData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { "M", true, new List<GameCommand> { GameCommand.Move } };
            yield return new object[] { "L R M", true, new List<GameCommand> { GameCommand.TurnLeft, GameCommand.TurnRight, GameCommand.Move } };
            yield return new object[] { "M M L L R R", true, new List<GameCommand> { GameCommand.Move, GameCommand.Move, GameCommand.TurnLeft, GameCommand.TurnLeft, GameCommand.TurnRight, GameCommand.TurnRight } };

            yield return new object[] { "", false, null };
            yield return new object[] { "G", false, null };
            yield return new object[] { "l", false, null };
            yield return new object[] { "RLM", false, null };
            yield return new object[] { "M R r", false, null };
            yield return new object[] { "M,R,L", false, null };
            yield return new object[] { "M M 3", false, null };
        }
    }
}
