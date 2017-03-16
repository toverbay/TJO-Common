using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace TJO.TestTools.UnitTesting.Should.Core.Assertions
{
    internal class AssertEqualityComparer<T> : IEqualityComparer<T>
    {
        private static readonly TypeInfo NullableType =
            typeof(Nullable<>).GetTypeInfo();

        public bool Equals(T x, T y)
        {
            var type = typeof(T);
            var info = type.GetTypeInfo();

            // Null?
            if (!info.IsValueType ||
                (info.IsGenericType &&
                 info.GetGenericTypeDefinition().GetTypeInfo().IsAssignableFrom(NullableType)))
            {
                if (object.Equals(x, default(T)))
                {
                    return object.Equals(y, default(T));
                }

                if (object.Equals(y, default(T)))
                {
                    return false;
                }
            }

            // x implements IEquatable<T> and is assignable from y?
            var xInfo = x.GetType().GetTypeInfo();
            var yInfo = y.GetType().GetTypeInfo();
            if (xInfo.IsAssignableFrom(yInfo) && x is IEquatable<T>)
            {
                return ((IEquatable<T>)x).Equals(y);
            }

            // y implements IEquatable<T> and is assignable from x?
            if (yInfo.IsAssignableFrom(xInfo) && y is IEquatable<T>)
            {
                return ((IEquatable<T>)y).Equals(x);
            }

            // Enumerable?
            var enumerableX = x as IEnumerable;
            var enumerableY = y as IEnumerable;

            if (enumerableX != null && enumerableY != null)
            {
                return new EnumerableEqualityComparer().Equals(enumerableX, enumerableY);
            }

            // Last case: rely on Object.Equals
            return object.Equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }
    }
}