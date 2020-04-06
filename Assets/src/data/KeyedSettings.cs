using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KeyedSettings {

    public SortedDictionary<string, SettingEntry> dict;

    /// <summary>
    /// If true, the value found in the file is used, otherwise the default one is returned in getter methods.
    /// </summary>
    private readonly bool prioritizeSource;

    private readonly string path;
    private readonly string fileName;

    public KeyedSettings(string path, string fileName, bool prioritizeSource) {
        this.path = path;
        this.fileName = fileName;

        this.makeFiles();

        List<string> list = FileUtils.readTextFile(this.path + "/" + this.fileName, false);
        this.dict = new SortedDictionary<string, SettingEntry>();
        this.prioritizeSource = prioritizeSource;

        if(!this.prioritizeSource) {
            string[] stringArray;
            string key, value, dataType;
            string comment = null;
            object settingValue;

            foreach(string line in list) {
                comment = null;


                if(line.StartsWith("#")) {
                    comment = line.Substring(2).Trim();
                }
                else if(line.Contains("=")) {
                    stringArray = line.Split('=');
                    string s0 = stringArray[0];
                    dataType = s0.Substring(1, 1);
                    key = s0.Substring(s0.LastIndexOf(']') + 1);
                    value = stringArray[1];

                    float f;
                    int i;
                    bool flag;

                    if(dataType == "F") {
                        float.TryParse(value, out f);
                        settingValue = f;
                    }
                    else if(dataType == "I") {
                        int.TryParse(value, out i);
                        settingValue = i;
                    }
                    else if(dataType == "B") {
                        bool.TryParse(value, out flag);
                        settingValue = flag;
                    }
                    else {
                        settingValue = value; // String
                    }

                    SettingEntry entry = new SettingEntry(settingValue, comment);
                    this.dict.Add(key, new SettingEntry(settingValue, comment));
                }
            }
        }
    }

    private void makeFiles() {
        if(!Directory.Exists(this.path)) {
            Directory.CreateDirectory(this.path);
        }

        string s = this.path + "/" + this.fileName;
        if(!File.Exists(s)) {
            File.Create(s);
        }
    }

    public void save() {
        if(this.prioritizeSource) {
            return; //Don't save, as we are forcing the code values to be used for this runtime.
        }

        this.makeFiles();

        StreamWriter writer = new StreamWriter(this.path + "/" + this.fileName, false);

        string comment;
        foreach(KeyValuePair<string, SettingEntry> entry in this.dict) {
            comment = entry.Value.comment;
            if(comment != null) {
                writer.WriteLine("# " + comment);
            }

            object value = entry.Value.value;

            // Get the prefix.
            string prefix = "?";

            if(value is string) {
                prefix = "S";
            } else if(value is float) {
                prefix = "F";
            } else if(value is bool) {
                prefix = "B";
            } else if(value is int) {
                prefix = "I";
            }

            writer.WriteLine("[" + prefix + "]" + entry.Key + "=" + value.ToString());
        }
        writer.Close();
    }

    /// <summary>
    /// Gets a float setting.  If it can't be found, one is created with the passed default value.
    /// </summary>
    public float getFloat(string key, float defaultValue, string comment = null) {
        if(!this.prioritizeSource) {
            if(this.dict.ContainsKey(key)) {
                return Convert.ToSingle(this.dict[key].value);
            } else {
                this.dict.Add(key, new SettingEntry(defaultValue, comment));
            }
        }
        return defaultValue;
    }

    /// <summary>
    /// Gets an integer setting.  If it can't be found, one is created with the passed default value.
    /// </summary>
    public int getInt(string key, int defaultValue, string comment = null) {
        if(!this.prioritizeSource) {
            if(this.dict.ContainsKey(key)) {
                return (int)this.dict[key].value;
            } else {
                this.dict.Add(key, new SettingEntry(defaultValue, comment));
            }
        }
        return defaultValue;
    }

    /// <summary>
    /// Gets a boolean setting.  If it can't be found, one is created with the passed default value.
    /// </summary>
    public bool getBool(string key, bool defaultValue, string comment = null) {
        if(!this.prioritizeSource) {
            if(this.dict.ContainsKey(key)) {
                return (bool)this.dict[key].value;
            } else {
                this.dict.Add(key, new SettingEntry(defaultValue, comment));
            }
        }
        return defaultValue;
    }

    /// <summary>
    /// Gets a string setting.  If it can't be found, one is created with the passed default value.
    /// </summary>
    public string getString(string key, string defaultValue, string comment = null) {
        if(!this.prioritizeSource) {
            if(this.dict.ContainsKey(key)) {
                return (string)this.dict[key].value;
            }
            else {
                this.dict.Add(key, new SettingEntry(defaultValue, comment));
            }
        }
        return defaultValue;
    }

    public struct SettingEntry {

        public readonly object value;
        /// <summary> May be null. </summary>
        public string comment;

        public SettingEntry(object value, string comment) {
            this.value = value;
            this.comment = comment;
        }

        public string getComment() {
            return this.comment != null ? this.comment : string.Empty;
        }
    }
}