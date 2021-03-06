﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Remote.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public interface IAsyncRemoteStreamQueryable<out T> : IQueryable<T>, IAsyncRemoteStreamQueryable
    {
        IAsyncEnumerable<T> ExecuteAsyncRemoteStream(CancellationToken cancellation = default);
    }
}
