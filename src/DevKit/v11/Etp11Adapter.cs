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

using Avro.IO;
using Avro.Specific;
using Energistics.Etp.Common;
using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.Common.Protocol.Core;
using Energistics.Etp.v11.Datatypes;
using Energistics.Etp.v11.Protocol.Core;
using System.Collections.Generic;
using System.Linq;

namespace Energistics.Etp.v11
{
    public class Etp11Adapter : EtpAdapterBase, IEtpAdapter
    {
        public Etp11Adapter() : base(EtpVersion.v11)
        {
        }

        public void RegisterCore(IEtpSession session)
        {
            if (session.IsClient)
                session.Register<ICoreClient, CoreClientHandler>();
            else
                session.Register<ICoreServer, CoreServerHandler>();
        }

        public void RequestSession(IEtpSession session)
        {
            session.InitializeInstanceSupportedProtocols();
            session.Handler<ICoreClient>().RequestSession(session.SessionSupportedProtocols);
        }

        public IMessageHeader CreateMessageHeader()
        {
            return new MessageHeader();
        }

        public IMessageHeader DecodeMessageHeader(Decoder decoder, string content)
        {
            return decoder.Decode<MessageHeader>(content);
        }

        public IMessageHeader DeserializeMessageHeader(string content)
        {
            return EtpExtensions.Deserialize<MessageHeader>(content);
        }

        public IAcknowledge CreateAcknowledge()
        {
            return new Acknowledge();
        }

        public IAcknowledge DecodeAcknowledge(ISpecificRecord body)
        {
            return (Acknowledge)body;
        }

        public IProtocolException DecodeProtocolException(ISpecificRecord body)
        {
            return (ProtocolException)body;
        }

        public IErrorInfo CreateErrorInfo()
        {
            return new ProtocolException();
        }

        public IProtocolException CreateProtocolException(IErrorInfo errorInfo)
        {
            if (errorInfo is ProtocolException)
                return (ProtocolException)errorInfo;

            return new ProtocolException
            {
                ErrorCode = errorInfo.Code,
                ErrorMessage = errorInfo.Message,
            };
        }
    }
}
