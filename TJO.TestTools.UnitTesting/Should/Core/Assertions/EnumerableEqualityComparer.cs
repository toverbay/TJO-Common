using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace TJO.TestTools.UnitTesting.Should.Core.Assertions
{
    internal class EnumerableEqualityComparer : IEqualityComparer<IEnumerable>
    {
        public int Position { get; set; }

        public bool Equals(IEnumerable x, IEnumerable y)
        {
            var enumeratorX = x.GetEnumerator();
            var enumeratorY = y.GetEnumerator();

            Position = 0;

            while (true)
            {
                var hasNextX = enumeratorX.MoveNext();
                var hasNextY = enumeratorY.MoveNext();

                if (!hasNextX || !hasNextY)
                {
                    return hasNextX == hasNextY;
                }

                if (enumeratorX.Current != null || enumeratorY.Current != null)
                {
                    if (enumeratorX.Current != null && enumeratorY.Current == null)
                    {
                        return false;
                    }

                    if (enumeratorX.Current == null)
                    {
                        return false;
                    }

                    var xType = enumeratorX.Current.GetType().GetTypeInfo();
                    var yType = enumeratorY.Current.GetType().GetTypeInfo();

                    if (xType.IsAssignableFrom(yType))
                    {
                        if (!Equals(enumeratorX.Current, enumeratorY.Current, xType.AsType()))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (yType.IsAssignableFrom(xType))
                        {
                            if (!Equals(enumeratorY.Current, enumeratorX.Current, yType.AsType()))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                Position++;
            }
        }

        private bool Equals(object x, object y, Type baseType)
        {
            var assertComparerType = typeof(AssertEqualityComparer<>).MakeGenericType(baseType);
            var assertComparer = Activator.CreateInstance(assertComparerType);
            var compareMethod = assertComparerType.GetRuntimeMethod("Equals", new[] { baseType, baseType });
            return (bool)compareMethod.Invoke(assertComparer, new[] { x, y });
        }

        public int GetHashCode(IEnumerable obj)
        {
            throw new NotImplementedException();
        }
    }
}
