using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BombermanGame
{
    class PlayerInfo
    {
        int peerID { get; }
        string name = "unknown";
        private Socket socket;
        public Socket Socket { get { return socket; } }

        public PlayerInfo(int peerID, Socket connection) {
            this.peerID = peerID;
            this.socket = connection;
        }

        public byte[] GetPlayerAddress() {
            IPEndPoint remoteIpEndPoint = socket.RemoteEndPoint as IPEndPoint;
            return remoteIpEndPoint.Address.GetAddressBytes();
        }
    }
}
