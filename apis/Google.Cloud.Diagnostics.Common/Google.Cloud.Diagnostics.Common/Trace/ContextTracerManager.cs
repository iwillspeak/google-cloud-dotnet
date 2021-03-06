﻿// Copyright 2017 Google Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Threading;

namespace Google.Cloud.Diagnostics.Common
{
    /// <summary>
    /// Manages access to the current <see cref="IManagedTracer"/>.
    /// </summary>
    public static class ContextTracerManager
    {
#if NET45
        private static readonly string _callContextName = System.Guid.NewGuid().ToString("N");
        private static IManagedTracer CurrentTracer
        {
            get
            {
                return System.Runtime.Remoting.Messaging.CallContext.LogicalGetData(_callContextName) as IManagedTracer;
            }
            set
            {
                System.Runtime.Remoting.Messaging.CallContext.LogicalSetData(_callContextName, value);
            }
        }
#else
        private static readonly AsyncLocal<IManagedTracer> _currentTracer = new AsyncLocal<IManagedTracer>();
        private static IManagedTracer CurrentTracer
        {
            get { return _currentTracer.Value; }
            set { _currentTracer.Value = value; }
        }
#endif

        /// <summary>
        /// Sets the current <see cref="IManagedTracer"/>.  This is called for each new trace context.
        /// </summary>
        public static void SetCurrentTracer(IManagedTracer tracer)
        {
            CurrentTracer = tracer;
        }

        /// <summary>
        /// Gets the current <see cref="IManagedTracer"/> or a <see cref="NullManagedTracer"/> if none exists.
        /// </summary>
        public static IManagedTracer GetCurrentTracer()
        {
            return CurrentTracer ?? NullManagedTracer.Instance;
        }
    }
}
