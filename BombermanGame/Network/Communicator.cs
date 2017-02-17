using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using BombermanGame.Network;


namespace BombermanGame
{
    class Communicator
    {
        Dictionary<int, PlayerInfo> peerList;
        GameForm parent;
        int peerID = 0;
        private bool isServer = false;
        // next id to assign to a new player connecting, 
        // prob make a bool isServer and a class ServerProperties later on 
        private int nextID = 1;   
        byte[] buffer = new byte[1024];

        public Communicator(GameForm gameform) {
            peerList = new Dictionary<int, PlayerInfo>();
            Console.WriteLine("Constructor Communicator");
            parent = gameform;
        }

        /*
         * Broadcast chat message to all connected peers
         */
        public void SendChatMessage(string message) {
            byte[] data = PacketBuilder.Build_ChatMessage(message);
            foreach (var peer in peerList.Values) {
                peer.Socket.BeginSend(data,0,data.Length,0,SendCallback, peer);
            }
        }


        /*
         * Broadcast specified message to all connected peers
         */
        public void Broadcast(byte[] data) {
            foreach (var peer in peerList.Values) {
                peer.Socket.BeginSend(data, 0, data.Length, 0, SendCallback, peer);
            }
        }


        /*
         * Connect to server with the specified properties
         */
        public void Connect() {
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            isServer = false;

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

        private void AcceptCallback(IAsyncResult ar) {
            try {
                Socket listener = (Socket)ar.AsyncState;
                Socket newPeer = listener.EndAccept(ar);

                ++nextID;
                peerList.Add(nextID, new PlayerInfo(nextID, newPeer));

                if (isServer) {
                    byte[] data = PacketBuilder.Build_HostResponse();
                    //newPeer.BeginSend();
                }
                // buffer = new byte[clientSocket.ReceiveBufferSize];

                newPeer.BeginReceive(buffer, 0, buffer.Length, 0, ReceiveCallback, newPeer);

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
                Socket server = (Socket)ar.AsyncState;

                // Complete the connection.  
                server.EndConnect(ar);

                peerList.Add(peerID, new PlayerInfo(nextID, server));

                Console.WriteLine("Socket connected to {0}",
                    server.RemoteEndPoint.ToString());

                var sendData = Encoding.ASCII.GetBytes("Hello");

                server.BeginSend(sendData, 0, sendData.Length, 0, SendCallback, server);
                // Listen for client data.
                server.BeginReceive(buffer, 0, buffer.Length, 0, ReceiveCallback, server);

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
                String response = String.Empty;
                // Read data from the remote device.  
                int bytesRead = peer.EndReceive(ar);

                if (bytesRead > 0) {
                    // There might be more data, so store the data received so far.  
                    //response = (Encoding.ASCII.GetString(buffer, 0, bytesRead));
                    int type = BitConverter.ToInt32(buffer, 0);
                    MessageType msgType = (MessageType)type;

                    ChatMessage message = new ChatMessage();
                    

                    switch(msgType) {
                        case MessageType.ChatMessage:
                            message = PacketDecoder.Decode_ChatMessage(buffer);
                            parent.PostChatMessage(message.message);
                            break;
                        case MessageType.HostResponse:
                            HandleHostMessage(buffer);
                            break;
                    }
                    /*
                    if (response.Contains("Chat:"))
                        response.Remove(0, 5);*/

                    Console.WriteLine(message.message);
                    // Get the rest of the data.  
                    peer.BeginReceive(buffer, 0, buffer.Length, 0, ReceiveCallback, peer);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void HandleHostMessage(byte[] buffer) {

        }
    }
}
