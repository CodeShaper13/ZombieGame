using UnityEngine;
using System;
using UnityEngine.Networking;

public class Test1 : MonoBehaviour {

    // Use this for initialization
    private void Awake() {
        NetworkWriter writer = new NetworkWriter();

        Guid guid = Guid.NewGuid();
        print(guid);
        writer.Write(guid.ToByteArray(), 16);

        byte[] bytes = new NetworkReader(writer).ReadBytes(16);
        print(bytes.Length);
        Guid guid2 = new Guid(bytes);
        print(guid2);
        print(guid == guid2);
    }

    void a() {
        Guid guid = Guid.NewGuid();
        print(guid.ToString());
        byte[] b = guid.ToByteArray();
        print(b.Length);
        foreach(byte b1 in b) {
            print(b1);
        }
    }
}
