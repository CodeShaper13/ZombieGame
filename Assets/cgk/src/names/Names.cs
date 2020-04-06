using UnityEngine;

public class Names {

    private static Names maleNames;
    private static Names femaleNames;
    private static Names lastNames;

    private readonly TextAsset textAsset;
    private string[] names;

    public static void initialize(TextAsset maleNames, TextAsset femaleNames, TextAsset lastNames) {
        Names.maleNames = new Names(maleNames);
        Names.femaleNames = new Names(femaleNames);
        Names.lastNames = new Names(lastNames);
    }

    public static bool isInitialized() {
        return Names.maleNames != null && Names.femaleNames != null && Names.lastNames != null;
    }

    private Names(TextAsset text) {
        this.textAsset = text;
        this.names = FileUtils.readTextAsset(this.textAsset, true).ToArray();
    }

    /// <summary>
    /// Returns a random name for the list.
    /// </summary>
    private string getRndName() {
        return this.names[Random.Range(0, this.names.Length)];
    }

    /// <summary>
    /// Creates a random name.  If the passed name is not male or female, then either gender's names could be used.
    /// </summary>
    public static void getRandomName(EnumGender gender, out string firstName, out string lastName) {
        if(gender == EnumGender.MALE) {
            firstName = Names.maleNames.getRndName();
        } else if(gender == EnumGender.FEMALE) {
            firstName = Names.femaleNames.getRndName();
        } else {
            firstName = (Random.Range(0, 1) == 0 ? Names.femaleNames.getRndName() : Names.maleNames.getRndName());
        }
        lastName = Names.lastNames.getRndName();
    }
}