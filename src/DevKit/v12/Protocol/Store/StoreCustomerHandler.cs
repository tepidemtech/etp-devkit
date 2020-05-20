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

using System;
using System.Collections.Generic;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.v12.Datatypes.Object;

namespace Energistics.Etp.v12.Protocol.Store
{
    /// <summary>
    /// Base implementation of the <see cref="IStoreCustomer"/> interface.
    /// </summary>
    /// <seealso cref="Etp12ProtocolHandler" />
    /// <seealso cref="Energistics.Etp.v12.Protocol.Store.IStoreCustomer" />
    public class StoreCustomerHandler : Etp12ProtocolHandler, IStoreCustomer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoreCustomerHandler"/> class.
        /// </summary>
        public StoreCustomerHandler() : base((int)Protocols.Store, "customer", "store")
        {
            RegisterMessageHandler<GetDataObjectsResponse>(Protocols.Store, MessageTypes.Store.GetDataObjectsResponse, HandleGetDataObjectsResponse);
            RegisterMessageHandler<Chunk>(Protocols.Store, MessageTypes.Store.Chunk, HandleChunk);
        }

        /// <summary>
        /// Sends a GetDataObjects message to a store.
        /// </summary>
        /// <param name="uris">The URIs.</param>
        /// <param name="format">The format of the response (XML or JSON).</param>
        /// <returns>The positive message identifier on success; otherwise, a negative number.</returns>
        public virtual long GetDataObjects(IList<string> uris, string format = "xml")
        {
            var header = CreateMessageHeader(Protocols.Store, MessageTypes.Store.GetDataObjects);

            var message = new GetDataObjects
            {
                Uris = uris.ToMap(),
                Format = format ?? "xml",
            };

            return Session.SendMessage(header, message);
        }

        /// <summary>
        /// Handles the GetDataObjectsResponse event from a store.
        /// </summary>
        public event ProtocolEventHandler<GetDataObjectsResponse> OnGetDataObjectsResponse;

        /// <summary>
        /// Sends a PutDataObjects message to a store.
        /// </summary>
        /// <param name="dataObjects">The data objects.</param>
        /// <returns>The positive message identifier on success; otherwise, a negative number.</returns>
        public virtual long PutDataObjects(IList<DataObject> dataObjects)
        {
            var header = CreateMessageHeader(Protocols.Store, MessageTypes.Store.PutDataObjects);

            var message = new PutDataObjects
            {
                DataObjects = dataObjects.ToMap(),
            };

            return Session.SendMessage(header, message);
        }

        /// <summary>
        /// Sends a DeleteDataObjects message to a store.
        /// </summary>
        /// <param name="uris">The URIs.</param>
        /// <returns>The positive message identifier on success; otherwise, a negative number.</returns>
        public virtual long DeleteDataObjects(IList<string> uris)
        {
            var header = CreateMessageHeader(Protocols.Store, MessageTypes.Store.DeleteDataObjects);

            var message = new DeleteDataObjects
            {
                Uris = uris.ToMap(),
            };

            return Session.SendMessage(header, message);
        }

        /// <summary>
        /// Sends a Chunk message to a store.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="blobId">The blob ID.</param>
        /// <param name="data">The chunk data.</param>
        /// <param name="messageFlags">The message flags.</param>
        /// <returns>The positive message identifier on success; otherwise, a negative number.</returns>
        public virtual long Chunk(IMessageHeader request, Guid blobId, byte[] data, MessageFlags messageFlags = MessageFlags.MultiPartAndFinalPart)
        {
            var header = CreateMessageHeader(Protocols.Store, MessageTypes.Store.Chunk, request.MessageId, messageFlags);

            var message = new Chunk
            {
                BlobId = blobId.ToUuid(),
                Data = data,
            };

            return Session.SendMessage(header, message);
        }

        /// <summary>
        /// Handles the Chunk event from a store.
        /// </summary>
        public event ProtocolEventHandler<Chunk> OnChunk;
        
        /// <summary>
        /// Handles the GetDataObjectsResponse message from a store.
        /// </summary>
        /// <param name="header">The message header.</param>
        /// <param name="response">The GetDataObjectsResponse message.</param>
        protected virtual void HandleGetDataObjectsResponse(IMessageHeader header, GetDataObjectsResponse response)
        {
            Notify(OnGetDataObjectsResponse, header, response);
        }

        /// <summary>
        /// Handles the DeleteDataObjects message from a store.
        /// </summary>
        /// <param name="header">The message header.</param>
        /// <param name="chunk">The Chunk message.</param>
        protected virtual void HandleChunk(IMessageHeader header, Chunk chunk)
        {
            Notify(OnChunk, header, chunk);
        }
    }
}
