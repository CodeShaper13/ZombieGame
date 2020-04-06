using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileUtils {

    public const string COMMENT = "#";

    /// <summary>
    /// Reads a Text Asset and returns the contents.  Empty
    /// line and lines starting with "#" are ignored.
    /// </summary>
    public static List<string> readTextAsset(TextAsset textAsset, bool removeComments = true) {
        string[] strings = textAsset.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        List<string> list = new List<string>(strings);
        if(removeComments) {
            list.RemoveAll(i => i.StartsWith(COMMENT));
        }
        return list;
    }

    /// <summary>
    /// Reads a file and returns the contents.  Empty
    /// line and lines starting with "#" are ignored.
    /// </summary>
    public static List<string> readTextFile(string path, bool removeComments = true) {
        string[] strings = File.ReadAllLines(path);
        List<string> list = new List<string>(strings);
        if(removeComments) {
            list.RemoveAll(i => i.StartsWith(COMMENT));
        }
        return list;
    }
}