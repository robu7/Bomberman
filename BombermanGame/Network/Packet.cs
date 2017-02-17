using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BombermanGame.Network
{
    [Serializable]
    public enum  MessageType { HostResponse, ChatMessage };

    public class ChatMessage
    {
        public string message { get; }
        public int player { get; }

        public ChatMessage() {
            message = "";
            player = 0;
        }

        public ChatMessage(int player, string message) {
            this.message = message;
            this.player = player;
        }
    }

    public static class PacketBuilder
    {

        static public byte[] Build_HostResponse() {
            List<byte> data = new List<byte>();

            return data.ToArray();
        }
        static public byte[] Build_ChatMessage(string message) {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((int)MessageType.HostResponse));
            data.AddRange(BitConverter.GetBytes(message.Length));
            data.AddRange(Encoding.ASCII.GetBytes(message));
            return data.ToArray();
        }
    }

    public static class PacketDecoder
    {
        static public int Read_Int(byte[] data) {
            BitConverter.ToInt32(data, 0);
            return 0;
        }
        static public ChatMessage Decode_ChatMessage(byte[] data) {
            int messageLength = BitConverter.ToInt32(data, 4);
            string message = (Encoding.ASCII.GetString(data, 8, messageLength));
            return new ChatMessage(2, message);
        }

    }

}
