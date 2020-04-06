using fNbt;
using System.IO;
using UnityEngine;

public class GameState {

    public GameState() { }

    public void readFromFile() {
        string s = this.func();
        if(File.Exists(s)) {
            NbtFile file = new NbtFile();
            file.LoadFromFile(s);

            NbtCompound rootTag = file.RootTag;
            this.readFromNbt(rootTag);
        }
    }

    public void saveToFile() {
        NbtCompound rootTag = new NbtCompound("map");
        this.writeToNbt(rootTag);

        NbtFile file = new NbtFile(rootTag);
        file.SaveToFile(this.func(), NbtCompression.None);
    }

    private void readFromNbt(NbtCompound nbt) {

    }

    private void writeToNbt(NbtCompound nbt) {

    }

    private string func() {
        return "gameData.nbt";
    }
}
