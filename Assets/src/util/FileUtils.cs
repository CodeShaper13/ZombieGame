using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileUtils {

    /// <summary>
    /// Reads a Text Asset and returns the contents.  Empty
    /// line and lines starting with "#" are ignored.
    /// </summary>
    public static List<string> readTextAsset(TextAsset textAsset, bool ignoreCommentLines) {
        string[] strings = textAsset.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        List<string> list = new List<string>(strings);
        if(ignoreCommentLines) {
            list.RemoveAll(i => i.StartsWith("#"));
        }
        return list;
    }

    /// <summary>
    /// Returns true if a save game exists.
    /// </summary>
    //public static bool doesSaveExists() {
    //    return Directory.Exists(Main.SAVE_PATH) && File.Exists(Main.SAVE_PATH + GameState.FILE_NAME);
    //}
}