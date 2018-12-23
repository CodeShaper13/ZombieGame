using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KeyedSettings {

    private readonly SortedDictionary<string, SettingEntry> dict;
    private readonly bool forceDefaults;

    public KeyedSettings(TextAsset textAsset, bool forceDefaults) {
        List<string> list = FileUtils.readTextAsset(textAsset, false);
        this.dict = new SortedDictionary<string, SettingEntry>();
        this.forceDefaults = forceDefaults;

        if(!forceDefaults) {
            string[] stringArray;
            string key, value;
            string comment = null;
            object settingValue;

            foreach(string line in list) {
                if(line.StartsWith("#")) {
                    comment = line.Substring(2).Trim();
                }
                else if(line.Contains("=")) {
                    stringArray = line.Split('=');
                    key = stringArray[0];
                    value = stringArray[1];

                    float f;
                    bool flag;

                    if(float.TryParse(value, out f)) {
                        settingValue = f;
                    }
                    else if(bool.TryParse(value, out flag)) {
                        settingValue = flag;
                    }
                    else {
                        settingValue = value; // String
                    }

                    this.dict.Add(key, new SettingEntry(settingValue, comment));

                    comment = null;
                }
            }
        }
    }

    public void save(string path) {
        if(this.forceDefaults) {
            return; //Don't save, as we are forcing the code values to be used for this runtime.
        }

        StreamWriter writer = new StreamWriter(path, false);

        string comment;
        foreach(KeyValuePair<string, SettingEntry> entry in this.dict) {
            comment = entry.Value.comment;
            if(comment != null) {
                writer.WriteLine("# " + comment);
            }
            writer.WriteLine(entry.Key + "=" + entry.Value.value.ToString());
        }
        writer.Close();
    }

    public float getFloat(string key, float defaultValue, string comment = null) {
        if(!this.forceDefaults) {
            if(this.dict.ContainsKey(key)) {
                return (float)this.dict[key].value;
            }
            else {
                this.dict.Add(key, new SettingEntry(defaultValue, comment));
            }
        }
        return defaultValue;
    }

    public int getInt(string key, int defaultValue, string comment = null) {
        if(!this.forceDefaults) {
            if(this.dict.ContainsKey(key)) {
                return (int)(float)this.dict[key].value;
            }
            else {
                this.dict.Add(key, new SettingEntry(defaultValue, comment));
            }
        }
        //TODO update the dict, we always want to overide comments.
        return defaultValue;
    }

    public bool getBool(string key, bool defaultValue, string comment = null) {
        if(!this.forceDefaults) {
            if(this.dict.ContainsKey(key)) {
                return (bool)this.dict[key].value;
            }
            else {
                this.dict.Add(key, new SettingEntry(defaultValue, comment));
            }
        }
        return defaultValue;
    }

    public string getString(string key, string defaultValue, string comment = null) {
        if(!this.forceDefaults) {
            if(this.dict.ContainsKey(key)) {
                return (string)this.dict[key].value;
            }
            else {
                this.dict.Add(key, new SettingEntry(defaultValue, comment));
            }
        }
        return defaultValue;
    }

    private struct SettingEntry {

        public readonly object value;
        /// <summary> May be null. </summary>
        public readonly string comment;

        public SettingEntry(object value, string comment) {
            this.value = value;
            this.comment = comment;
        }
    }
}