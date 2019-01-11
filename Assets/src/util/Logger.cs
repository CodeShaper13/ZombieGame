using System;
using UnityEngine;

public static class Logger {

    public static void log(object obj) {
        Debug.Log(Logger.getPrefix() + obj);
    }

    public static void logError(object obj) {
        Debug.LogError(Logger.getPrefix() + obj);
    }

    public static void logWarning(object obj) {
        Debug.LogWarning(Logger.getPrefix() + obj);
    }

    private static string getPrefix() {
        return "[Logger][" + DateTime.Now.ToString() + "]: ";
    }
}
