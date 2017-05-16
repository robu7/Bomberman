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
        private int peerID;
        public int PeerID { get { return peerID; } set { peerID = value; } }
        string name = "unknown";
        public string Name { get { return name; } set { name = value; } }
        private Socket socket;
        public Socket Socket { get { return socket; } set { socket = value; } }

        private Player player;
        public Player PlayerInstance { get { return player; } set { player = value; } }

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
