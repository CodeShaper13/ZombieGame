using fNbt;

/// <summary>
/// Interface for objects that can to saved to and read from NBT.
/// </summary>
public interface INbtSerializable {

    void writeToNbt(NbtCompound tag);

    void readFromNbt(NbtCompound tag);
}

