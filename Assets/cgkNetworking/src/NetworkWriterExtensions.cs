using UnityEngine.Networking;
using System;

public static class NetworkIOExtensions {

    public static void Write(this NetworkWriter writer, Guid guid) {
        writer.Write(guid.ToByteArray(), 16);
    }

    public static Guid ReadGuid(this NetworkReader reader) {
        return new Guid(reader.ReadBytes(16));
    }
}
