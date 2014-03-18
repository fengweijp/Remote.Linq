﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

using System;

internal static class TypeExtensions
{
    public static Type GetUnderlyingSystemType(this Type type)
    {
        return type.UnderlyingSystemType;
    }
}
