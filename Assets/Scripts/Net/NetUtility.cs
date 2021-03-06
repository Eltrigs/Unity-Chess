using System;
using Unity.Networking.Transport;
using UnityEngine;

public static class NetUtility
{
    public static void onData(DataStreamReader stream, NetworkConnection cnn, Server server = null)
    {
        NetMessage msg = null;
        //Pop the first byte off the connection stream to check for OpCode
        var opCode = (OpCode)stream.ReadByte();
        //Now deal with the message according to the recieved OpCode
        switch (opCode)
        {
            case OpCode.KEEP_ALIVE:
                msg = new NetKeepAlive(stream); break;
            case OpCode.WELCOME:
                msg = new NetWelcome(stream); break;
            case OpCode.START_GAME:
                msg = new NetStartGame(stream); break;
            case OpCode.MAKE_MOVE:
                msg = new NetMakeMove(stream); break;
            case OpCode.REMATCH:
                msg = new NetRematch(stream); break;
            default:
                Debug.LogError("Message recieved had no OpCode");
                break;
        }

        if (server != null)
        {
            msg.recievedOnServer(cnn);
        }
        else
            msg.recievedOnClient();
    }
    
    // Net messages
    public static Action<NetMessage> C_KEEP_ALIVE;
    public static Action<NetMessage> C_WELCOME;
    public static Action<NetMessage> C_START_GAME;
    public static Action<NetMessage> C_MAKE_MOVE;
    public static Action<NetMessage> C_REMATCH;
    public static Action<NetMessage, NetworkConnection> S_KEEP_ALIVE;
    public static Action<NetMessage, NetworkConnection> S_WELCOME;
    public static Action<NetMessage, NetworkConnection> S_START_GAME;
    public static Action<NetMessage, NetworkConnection> S_MAKE_MOVE;
    public static Action<NetMessage, NetworkConnection> S_REMATCH;
}
