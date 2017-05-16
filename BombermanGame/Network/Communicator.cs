using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace BombermanGame
{

    interface IReciever<T>
    {
        void HandleCommand(T command);
    }

    class Communicator
    {
        private Dictionary<int, PlayerInfo> peerList;
        public Dictionary<int, PlayerInfo> PeerList { get { return peerList; } }
        private NetworkReciever activeControl;
        public NetworkReciever ActiveControl { get { return activeControl; } set { activeControl = value; } }
        private int peerID = 0;
        public int PeerID { get { return peerID; } }

        private bool isHost = false;
        public bool IsHost { get { return isHost; }
            set {
                isHost = value;
                ++nextID;
                peerID = 1;
            } }

        // next id to assign to a new player connecting, 
        // prob make a bool isServer and a class ServerProperties later on 
        private int nextID = 1;   
        byte[] buff = new byte[1024];

        public Communicator(GameForm gameform) {
            peerList = new Dictionary<int, PlayerInfo>();
            Console.WriteLine("Constructor Communicator");
            //activeControl = gameform;
        }


        /*
         * Broadcast chat message to all connected peers
         */
        public void SendChatMessage(string message) {
            byte[] data = PacketBuilder.Build_ChatMessage(message);
            foreach (var peer in peerList.Values) {
                peer.Socket.BeginSend(data,0,data.Length,0,SendCallback, peer.Socket);
            }
        }


        /*
         * Broadcast specified message to all connected peers
         */
        public void Broadcast(byte[] data) {
            foreach (var peer in peerList.Values) {
                peer.Socket.BeginSend(data, 0, data.Length, 0, SendCallback, peer.Socket);
            }
        }


        /*
         * Connect to server with the specified properties
         */
        public void Connect(IPAddress connectAddress = null) {
            if (connectAddress == null) {
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                connectAddress = ipHostInfo.AddressList[0];
            }
            IPEndPoint remoteEP = new IPEndPoint(connectAddress, 11000);
            isHost = false;

            try {
                Socket client = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP, ConnectCallback, client);
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

        }


        /*
        * Connect to server with the specified properties
        */
        public void StartListening() {
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            //Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            try {
                // Create a TCP/IP socket.
                Socket listener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(localEndPoint);
                listener.Listen(10);
                listener.BeginAccept(AcceptCallback, listener);
            }
            catch (SocketException ex) {
                GameForm.ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex) {
                GameForm.ShowErrorDialog(ex.Message);
            }
        }


        /*
         * Accept a connection attempt 
         */
        private void AcceptCallback(IAsyncResult ar) {
            try {
                Socket listener = (Socket)ar.AsyncState;
                Socket newPeer = listener.EndAccept(ar);

                PlayerInfo newPlayer = new PlayerInfo(nextID, newPeer);
                byte[] data = new byte[1024];

                if (isHost && peerList.Count != 0) {
                    Console.WriteLine("As host update other players of the newly connected peer with address{0}", newPlayer.GetPlayerAddress());
                    data = PacketBuilder.Build_NewConnectedPeer(newPlayer);
                    Broadcast(data);
                }

                // Save player connection
                Console.WriteLine("Saving connection, nextID:{0}", nextID);
                peerList.Add(nextID, newPlayer);

                newPeer.BeginReceive(buff, 0, buff.Length, 0, ReceiveCallback, newPeer);
                ++nextID;

                listener.BeginAccept(AcceptCallback, listener);
            }
            catch (SocketException ex) {
                GameForm.ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex) {
                GameForm.ShowErrorDialog(ex.Message);
            }
        }

        private void ConnectCallback(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.  
                Socket peer = (Socket)ar.AsyncState;

                // Complete the connection.  
                peer.EndConnect(ar);

                peerList.Add(nextID, new PlayerInfo(nextID, peer));
                ++nextID;

                Console.WriteLine("Socket connected to {0}",
                    peer.RemoteEndPoint.ToString());

                var data = PacketBuilder.Build_ConnectMessage(peerID, "client");

                peer.BeginSend(data, 0, data.Length, 0, SendCallback, peer);
                // Listen for client data.
                peer.BeginReceive(buff, 0, buff.Length, 0, ReceiveCallback, peer);

                // Signal that the connection has been made.  
                //connectDone.Set();
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void SendCallback(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.  
                Socket peer = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = peer.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to peer.", bytesSent);

                // Signal that all bytes have been sent.  
                //sendDone.Set();
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar) {
            try {
                Socket peer = (Socket)ar.AsyncState;
                // Read data from the remote device.  
                int bytesRead = peer.EndReceive(ar);
                Console.WriteLine("Read {0} bytes from peer: {1}", bytesRead, peerList.First(x => x.Value.Socket == peer).Key);
                int senderID = peerList.First(x => x.Value.Socket == peer).Key;

                if (bytesRead > 0) {
                    // There might be more data, so store the data received so far.  
                    //response = (Encoding.ASCII.GetString(buffer, 0, bytesRead));
                    int type = BitConverter.ToInt32(buff, 0);
                    MessageType msgType = (MessageType)type;

                    Console.WriteLine("Read message: {0} from peer: {1}", msgType.ToString(), peerList.First(x => x.Value.Socket == peer).Key);

                    switch (msgType) {
                        case MessageType.ChatMessage:
                            ChatMessage message = PacketDecoder.Decode_ChatMessage(buff);
                            message.Player = senderID;
                            message.Name = peerList[senderID].Name;
                            Console.WriteLine("Message: {0}, from player {1}", message.Message, message.Player);
                            activeControl.HandleCommand(message);
                            //activeControl.PostChatMessage(message.Message, peerList[senderID].Name);
                            break;
                        case MessageType.HostResponse:
                            HandleHostResponse(buff);
                            break;
                        case MessageType.ConnectMessage:
                            HandleConnectMessage(buff, senderID);
                            if (isHost) {
                                var data = PacketBuilder.Build_HostResponse(peerList.First(x => x.Value.Socket == peer).Key, "Host");
                                peer.BeginSend(data, 0, data.Length, 0, SendCallback, peer);
                            }
                            break;
                        case MessageType.NewPeer:
                            HandleConnectToNewPeer(buff);
                            break;
                        case MessageType.PlayerMovement:
                            activeControl.HandleCommand(PacketDecoder.Decode_PlayerMovement(buff));
                            break;
                        case MessageType.Ready:
                            activeControl.ReadySignal(PacketDecoder.Decode_Ready(buff));
                            break;
                        case MessageType.DeployBomb:
                            activeControl.HandleCommand(PacketDecoder.Decode_DeployBomb(buff));
                            break;
                    }
                    Array.Clear(buff, 0, buff.Length);
                    // Get the rest of the data.  
                    peer.BeginReceive(buff, 0, buff.Length, 0, ReceiveCallback, peer);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void HandleHostResponse(byte[] buffer) {
                HostResponse response = PacketDecoder.Decode_HostResponse(buffer);
                peerID = response.AssignedID;
                peerList.First(x => x.Key == 1).Value.Name = response.HostName;

                foreach (var test in peerList) {
                    Console.WriteLine(test.Key);
                }

                //activeControl.AddPlayerToLobby(peerList[1]);
        }

        private void HandleConnectMessage(byte[] buffer, int senderID) {
            ConnectMessage message = PacketDecoder.Decode_ConnectMessage(buffer);
            peerList[senderID].Name = message.Name;
            //activeControl.AddPlayerToLobby(senderID, message.Name);
        }

        private void HandleConnectToNewPeer(byte[] buffer) {
            NewPeer newPeer = PacketDecoder.Decode_NewConnectedPeer(buffer);
            Console.WriteLine("New player ip: {0}", newPeer.PeerAddress.ToString());
        }
    }
}
