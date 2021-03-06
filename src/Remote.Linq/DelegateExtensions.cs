﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Remote.Linq
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class DelegateExtensions
    {
        public static object? DynamicInvokeAndUnwrap(this Delegate @delegate, params object?[] args)
        {
            try
            {
                return @delegate.DynamicInvoke(args);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException!;
            }
        }
    }
}
