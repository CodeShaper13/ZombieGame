using UnityEngine;
using UnityEngine.UI;

public class I18nTextTranslator : MonoBehaviour {

    public string TextId;

    private void Start() {
        Text text = GetComponent<Text>();
        if(text != null) {
            if(TextId == "ISOCode"){
                text.text = I18n.GetLanguage();
            }
            else {
                text.text = I18n.translation(TextId);
            }
        }
    }
}