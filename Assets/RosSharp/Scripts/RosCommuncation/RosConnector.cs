﻿/*
© Siemens AG, 2017
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

<http://www.apache.org/licenses/LICENSE-2.0>.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Threading;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class RosConnector : MonoBehaviour
    {
        public int timeout = 10;

        public RosSocket RosSocket { get; private set; }
        public enum Protocols { WebSocketSharp, WebSocketNET, WebSocketUWP };
        public Protocols Protocol;
        public string RosBridgeServerUrl = "ws://192.168.0.1:9090";
        public bool Connected { get; private set; } = false;

        private ManualResetEvent IsConnected = new ManualResetEvent(false);

        public void Awake()
        {
            RosBridgeClient.Protocols.IProtocol protocol = GetProtocol();
            protocol.OnConnected += OnConnected;
            protocol.OnClosed += OnClosed;

            RosSocket = new RosSocket(protocol);
        
            if (!IsConnected.WaitOne(timeout * 1000))
            {
                Debug.LogWarning("Failed to connect to RosBridge at: " + RosBridgeServerUrl);
            }
        }

        private RosBridgeClient.Protocols.IProtocol GetProtocol()
        {

#if WINDOWS_UWP
                return new RosBridgeClient.Protocols.WebSocketUWPProtocol(RosBridgeServerUrl);
#else
            switch (Protocol)
            {
                case Protocols.WebSocketSharp:
                    return new RosBridgeClient.Protocols.WebSocketSharpProtocol(RosBridgeServerUrl);
                case Protocols.WebSocketUWP:
                    Debug.Log("WebSocketUWP only works when deployed to HoloLens, defaulting to WebSocketNetProtocol");
                    return new RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl);
                default:
                    return new RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl);
            }
#endif
        }

        private void OnApplicationQuit()
        {
            RosSocket.Close();
        }

        private void OnConnected(object sender, EventArgs e)
        {
            IsConnected.Set();
            Connected = true;
            Debug.Log("Connected to RosBridge: " + RosBridgeServerUrl);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Connected = false;
            Debug.Log("Disconnected from RosBridge: " + RosBridgeServerUrl);
        }
    }
}