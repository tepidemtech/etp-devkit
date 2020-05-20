﻿//----------------------------------------------------------------------- 
// ETP DevKit, 1.2
//
// Copyright 2019 Energistics
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

using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;

namespace Energistics.Etp.v12.Protocol.StoreQuery
{
    /// <summary>
    /// Describes the interface that must be implemented by the customer role of the StoreQuery protocol.
    /// </summary>
    /// <seealso cref="IProtocolHandler" />
    [ProtocolRole((int)Protocols.StoreQuery, "customer", "store")]
    public interface IStoreQueryCustomer : IProtocolHandler
    {
        /// <summary>
        /// Sends a FindObjects message to a store.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="format">The format of the data (XML or JSON).</param>
        /// <returns>The positive message identifier on success; otherwise, a negative number.</returns>
        long FindObjects(string uri, string format = "xml");

        /// <summary>
        /// Handles the FindObjectsResponse event from a store.
        /// </summary>
        event ProtocolEventHandler<FindObjectsResponse, FindObjects> OnFindObjectsResponse;

        /// <summary>
        /// Handles the Chunk event from a store.
        /// </summary>
        event ProtocolEventHandler<Chunk> OnChunk;
    }
}
