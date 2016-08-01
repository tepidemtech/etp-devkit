﻿//----------------------------------------------------------------------- 
// ETP DevKit, 1.1
//
// Copyright 2016 Petrotechnical Data Systems
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
//-----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Energistics
{
    /// <summary>
    /// Provides static helper methods that can be used to process ETP messages asynchronously.
    /// </summary>
    public static class TestExtensions
    {
        private const int DefaultTimeoutInMilliseconds = 5000;

        /// <summary>
        /// Opens a WebSocket connection and waits for the SocketOpened event to be called.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="milliseconds">The timeout, in milliseconds.</param>
        /// <returns>An awaitable task.</returns>
        public static async Task<bool> OpenAsync(this EtpClient client, int milliseconds = DefaultTimeoutInMilliseconds)
        {
            var task = new Task<bool>(() => client.IsOpen);

            client.SocketOpened += (s, e) => task.Start();
            client.Open();

            return await task.WaitAsync(milliseconds);
        }

        /// <summary>
        /// Executes a task asynchronously and waits the specified timeout period for it to complete.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="task">The task to execute.</param>
        /// <param name="milliseconds">The timeout, in milliseconds.</param>
        /// <returns>An awaitable task.</returns>
        public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, int milliseconds = DefaultTimeoutInMilliseconds)
        {
            return await task.WaitAsync(TimeSpan.FromMilliseconds(milliseconds));
        }

        /// <summary>
        /// Executes a task asynchronously and waits the specified timeout period for it to complete.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="task">The task to execute.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>An awaitable task.</returns>
        /// <exception cref="System.TimeoutException">The operation has timed out.</exception>
        public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            var tokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, tokenSource.Token));

            if (completedTask == task)
            {
                tokenSource.Cancel();
                return await task;
            }

            throw new TimeoutException("The operation has timed out.");
        }
    }
}
