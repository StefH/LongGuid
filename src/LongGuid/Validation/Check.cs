// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

// Copied from https://github.com/aspnet/EntityFramework/blob/dev/src/Shared/Check.cs
namespace System.LongGuid.Validation
{
    // [ExcludeFromCodeCoverage]
    [DebuggerStepThrough]
    internal static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T Condition<T>([NoEnumeration] T value, [NotNull] Predicate<T> condition, [InvokerParameterName] [NotNull] string parameterName)
        {
            NotNull(condition, nameof(condition));
            NotNull(value, nameof(value));

            if (!condition(value))
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (ReferenceEquals(value, null))
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName] [NotNull] string parameterName, [NotNull] string propertyName)
        {
            if (ReferenceEquals(value, null))
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));
                NotNullOrEmpty(propertyName, nameof(propertyName));

                throw new ArgumentException(CoreStrings.ArgumentPropertyNull(propertyName, parameterName));
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static IEnumerable<T> NotNullOrEmpty<T>(IEnumerable<T> value, [InvokerParameterName] [NotNull] string parameterName)
        {
            NotNull(value, parameterName);

            var notNullOrEmpty = value as T[] ?? value.ToArray();
            if (!notNullOrEmpty.Any())
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(CoreStrings.CollectionArgumentIsEmpty(parameterName));
            }

            return notNullOrEmpty;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotNullOrEmpty(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            Exception e = null;
            if (ReferenceEquals(value, null))
            {
                e = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException(CoreStrings.ArgumentIsEmpty(parameterName));
            }

            if (e != null)
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw e;
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static Guid NotEmpty(Guid value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException(CoreStrings.ArgumentIsEmpty(parameterName));
            }

            return value;
        }

        public static IEnumerable<T> HasNoNulls<T>(IEnumerable<T> value, [InvokerParameterName] [NotNull] string parameterName)
        {
            NotNull(value, parameterName);

            var hasNoNulls = value as T[] ?? value.ToArray();
            if (hasNoNulls.Any(e => e == null))
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(parameterName);
            }

            return hasNoNulls;
        }
    }
}