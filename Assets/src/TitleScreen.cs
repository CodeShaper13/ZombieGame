using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    private void OnGUI() {
        float w = 250;
        float h = 80;

        if(GUI.Button(new Rect(20, 20, w, h), "Single Player Game")) {
            SceneManager.LoadScene("SinglePlayer");
            Main.instance().isSinglePlayerGame = true;
        }
        if(GUI.Button(new Rect(20, 120, w, h), "Host Multiplayer Game")) {
            this.func();
        }
        if(GUI.Button(new Rect(20, 220, w, h), "Join Multiplayer Game")) {
            this.func();
        }
        if(GUI.Button(new Rect(20, 320, w, h), "Exit")) {
            #if UNITY_EDITOR
                print("Exiting Game...");
            #endif
            Application.Quit();
        }
    }

    private void func() {
        Main.instance().isSinglePlayerGame = false;
        SceneManager.LoadScene("MultiPlayer");
    }
}
