using System;
using System.IO;
using UnityEngine;

public static class ScreenshotHelper {

    /// <summary>
    /// The default directory to save screenshots to.
    /// </summary>
    public static readonly string defaultDirectory = "screenshots/";

    /// <summary>
    /// Takes a screenshot.  When no path is passes, the default screenshot directory is used with the current time as the name.
    /// </summary>
    public static void captureScreenshot(string screenshotPath = null) {
        if(screenshotPath == null) {
            string time = DateTime.Now.ToString();
            time = time.Replace('/', '-').Replace(' ', '_').Replace(':', '.').Substring(0, time.Length - 3);

            screenshotPath = ScreenshotHelper.defaultDirectory + time + ".png";
        }

        if(!Directory.Exists(ScreenshotHelper.defaultDirectory)) {
            Directory.CreateDirectory(ScreenshotHelper.defaultDirectory);
        }

        ScreenCapture.CaptureScreenshot(screenshotPath);
    }
}