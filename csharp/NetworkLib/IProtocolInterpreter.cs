namespace NetworkLib
{
    using NetworkLib.Utilities;
    using System;

    public interface IProtocolInterpreter
    {
        bool InterpretMessage(int sessionID, ByteArray byteArray);
    }
}

