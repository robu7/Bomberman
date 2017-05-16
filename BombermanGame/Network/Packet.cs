using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace BombermanGame
{
    [Serializable]
    public enum  MessageType { HostResponse, ChatMessage, NewPeer, ConnectMessage, PlayerMovement, Ready, DeployBomb};

    interface NetworkReciever
    {
        void HandleCommand<T>(T command);
        void ReadySignal(int peerID);
    }


    /*
     * 
     */
    public class HostResponse
    {
        int assignedID;
        public int AssignedID { get { return assignedID; } }
        string hostName;
        public string HostName { get { return hostName; } }

        public HostResponse(int id, string name) {
            assignedID = id;
            hostName = name;
        }

    }


    /*
     * 
     */
    public class ChatMessage
    {
        string message;
        public string Message { get { return message; } }
        int player;
        public int Player { get {return player; } set { player = value; } }
        string name;
        public string Name { get { return name; } set { name = value; } }


        public ChatMessage() {
            message = "";
            player = 0;
        }

        public ChatMessage(string message) {
            this.message = message;
        }
    }


    /*
     * 
     */
    public class ConnectMessage
    {
        int peerID;
        public int PeerID { get { return peerID; } }
        string peerName;
        public string Name { get { return peerName; } }

        public ConnectMessage(int id, string name) {
            peerID = id;
            peerName = name;
        }
    }


    /*
     * 
     */
    public class NewPeer
    {
        int peerID;
        public int PeerID { get { return peerID; } }
        IPAddress peerAddress;
        public IPAddress PeerAddress { get { return peerAddress; } }

        public NewPeer(int id, IPAddress address) {
            peerID = id;
            peerAddress = address;
        }
    }

    class PlayerMovement
    {
        int peerID;
        public int PeerID { get { return peerID; } }
        Game.Direction moveDirection;
        public Game.Direction MoveDirection { get { return moveDirection; } }

        public PlayerMovement(int player, Game.Direction moveDirection) {
            this.peerID = player;
            this.moveDirection = moveDirection;
        }
    }

    class DeployBomb
    {
        int peerID;
        public int PeerID { get { return peerID; } }

        public DeployBomb(int player) {
            this.peerID = player;
        }
    }


    static class PacketBuilder
    {
        static public byte[] Build_HostResponse(int assignID, string hostName) {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((int)MessageType.HostResponse));
            data.AddRange(BitConverter.GetBytes(assignID));
            data.AddRange(BitConverter.GetBytes(hostName.Length));
            data.AddRange(Encoding.ASCII.GetBytes(hostName));
            return data.ToArray();
        }

        /*
         * Builds message that contain sender ID and message string
         */
        static public byte[] Build_ChatMessage(string message) {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((int)MessageType.ChatMessage));
            data.AddRange(BitConverter.GetBytes(message.Length));
            data.AddRange(Encoding.ASCII.GetBytes(message));
            return data.ToArray();
        }

        /*
         * Message that the host sends when a new peer has connected to the game,
         * contains new peer ID and address which to connect
         */
        static public byte[] Build_NewConnectedPeer(PlayerInfo newPeer) {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((int)MessageType.NewPeer));
            data.AddRange(BitConverter.GetBytes(newPeer.PeerID));
            byte[] address = newPeer.GetPlayerAddress();
            data.AddRange(BitConverter.GetBytes(address.Length));
            data.AddRange(address);
            return data.ToArray();
        }

        /*
         * Initial message when a peer connects to another peer,
         * contains sender peer ID and address which to connect
         */
        static public byte[] Build_ConnectMessage(int myID, string name) {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((int)MessageType.ConnectMessage));
            data.AddRange(BitConverter.GetBytes(myID));
            data.AddRange(BitConverter.GetBytes(name.Length));
            data.AddRange(Encoding.ASCII.GetBytes(name));
            return data.ToArray();
        }

        /*
         * Player movement replication,
         * contains sender peer ID and movement directon
         */
         static public byte[] Build_PlayerMovement(int myID, Game.Direction direction) {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((int)MessageType.PlayerMovement));
            data.AddRange(BitConverter.GetBytes(myID));
            data.AddRange(BitConverter.GetBytes((int)direction));
            return data.ToArray();
        }

        /*
        * Ready signal,
        * contains sender peer ID
        */
        static public byte[] Build_Ready(int myID) {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((int)MessageType.Ready));
            data.AddRange(BitConverter.GetBytes(myID));
            return data.ToArray();
        }

        /*
        * Deploy bomb message,
        * contains sender peer ID
        */
        static public byte[] Build_DeployBomb(int myID) {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((int)MessageType.DeployBomb));
            data.AddRange(BitConverter.GetBytes(myID));
            //data.AddRange(BitConverter.GetBytes(pos.X));
            //data.AddRange(BitConverter.GetBytes(pos.Y));
            return data.ToArray();
        }

    }

    static class PacketDecoder
    {
        static public HostResponse Decode_HostResponse(byte[] data) {
            int id = BitConverter.ToInt32(data, 4);
            int nameLenght = BitConverter.ToInt32(data, 8);
            string hostName = (Encoding.ASCII.GetString(data, 12, nameLenght));
            return new HostResponse(id, hostName);
        }


        /*
         * 
         */      
        static public ChatMessage Decode_ChatMessage(byte[] data) {
            int messageLength = BitConverter.ToInt32(data, 4);
            string message = (Encoding.ASCII.GetString(data, 8, messageLength));
            return new ChatMessage(message);
        }

        /*
         * 
         */
        static public ConnectMessage Decode_ConnectMessage(byte[] data) {
            int peerID = BitConverter.ToInt32(data, 4);
            int nameLenght = BitConverter.ToInt32(data, 8);
            string peerName = (Encoding.ASCII.GetString(data, 12, nameLenght));
            return new ConnectMessage(peerID, peerName);
        }

        /*
         * 
         */
        static public NewPeer Decode_NewConnectedPeer(byte[] data) {
            int peerID = BitConverter.ToInt32(data, 4);
            int addressLength = BitConverter.ToInt32(data, 8);
            IPAddress peerAddress = new IPAddress(data.Skip(12).Take(addressLength).ToArray());
            return new NewPeer(peerID, peerAddress);
        }

        /*
        * 
        */
        static public PlayerMovement Decode_PlayerMovement(byte[] data) {
            int peerID = BitConverter.ToInt32(data, 4);
            Game.Direction dir = (Game.Direction)BitConverter.ToInt32(data, 8);
            return new PlayerMovement(peerID, dir);
        }

        /*
        * 
        */
        static public int Decode_Ready(byte[] data) {
            int peerID = BitConverter.ToInt32(data, 4);
            return peerID;
        }

        /*
        * 
        */
        static public DeployBomb Decode_DeployBomb(byte[] data) {
            int peerID = BitConverter.ToInt32(data, 4);
            return new DeployBomb(peerID);
        }
    }
}
