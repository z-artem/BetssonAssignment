using System;
using Xunit;

namespace EscapeMines.Engine.Tests.Helpers
{
    public static class AssertHelper
    {
        public static void ThrowsArgumentNullException<T1, T2>(Tuple<T1, T2> args, Action<Tuple<T1, T2>> action)
            where T1 : class
            where T2 : class
        {
            Assert.Throws<ArgumentNullException>(() => action(Tuple.Create((T1)null, args.Item2)));
            Assert.Throws<ArgumentNullException>(() => action(Tuple.Create(args.Item1, (T2)null)));
        }

        public static void ThrowsArgumentNullException<T1, T2, T3>(Tuple<T1, T2, T3> args, Action<Tuple<T1, T2, T3>> action)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            Assert.Throws<ArgumentNullException>(() => action(Tuple.Create((T1)null, args.Item2, args.Item3)));
            Assert.Throws<ArgumentNullException>(() => action(Tuple.Create(args.Item1, (T2)null, args.Item3)));
            Assert.Throws<ArgumentNullException>(() => action(Tuple.Create(args.Item1, args.Item2, (T3)null)));
        }
    }
}
