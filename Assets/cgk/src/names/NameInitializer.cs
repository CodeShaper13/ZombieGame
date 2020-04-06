using UnityEngine;

/// <summary>
/// This component will initialize the Names class what it is awaken.
/// If the Names class is already initialized, nothing happens.
/// </summary>
public class NameInitializer : MonoBehaviour {

    [SerializeField]
    private TextAsset maleNames;
    [SerializeField]
    private TextAsset femaleNames;
    [SerializeField]
    private TextAsset lastNames;

    private void Awake() {
        if(!Names.isInitialized()) {
            Names.initialize(this.maleNames, this.femaleNames, this.lastNames);
        }
    }
}
