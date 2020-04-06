using UnityEngine;
using System;

#if UNITY_EDITOR
    using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New CampaignStory", menuName = "Campaign Story Data", order = 51)]
public class CampaignLevelData : ScriptableObject {

    [Header("Internal ID, do NOT change!")]
    public string internalId;
    [Header("")]
    [Tooltip("The text seen on the campaign select screen.")]
    public string levelDescription;
    [Tooltip("The EXACT name of the scene.  Do NOT include the path, only the name.")]
    public string sceneName;
    [Tooltip("How many resources the player starts with.")]
    public int startingResources;
    public EnumGameMode gameMode;
    public object[] data = new object[16];

    [Header("")]
    public Character[] characters = new Character[1];
    public Dialog[] startDialog = new Dialog[1] { new Dialog(0, "Hell World!") };
    public Dialog[] winDialog = new Dialog[1];
    public Dialog[] loseDialog = new Dialog[1];

    [Header("")]

    [Tooltip("The ID of the item given when completing a level.  Leave as -1 for no item.")]
    [SerializeField]
    private int itemId = -1;

    [Serializable]
    public class Character {
        public int charId = 0;
        [Tooltip("The name of this character")]
        public string name = "nul";
        public GameObject prefab;

        public Character(int charId, string name, GameObject prefab) {
            this.charId = charId;
            this.name = name;
            this.prefab = prefab;
        }
    }

    [Serializable]
    public class Dialog {
        [Tooltip("")]
        public int speakingCharID = 0;
        public string dialog = "...";

        public Dialog(int i, string s) {
            this.speakingCharID = i;
            this.dialog = s;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CampaignLevelData))]
    [CanEditMultipleObjects]
    public class CampaignLevelDataEditor : Editor {

        private SerializedProperty internalId;
        private SerializedProperty levelDescription;
        private SerializedProperty sceneName;
        private SerializedProperty startingResources;
        private SerializedProperty characters;
        private SerializedProperty startDialog;
        private SerializedProperty winDialog;
        private SerializedProperty loseDialog;

        private void OnEnable() {
            this.internalId = serializedObject.FindProperty("internalId");
            this.levelDescription = serializedObject.FindProperty("levelDescription");
            this.sceneName = serializedObject.FindProperty("sceneName");
            this.startingResources = serializedObject.FindProperty("startingResources");
            this.characters = serializedObject.FindProperty("characters");
            this.startDialog = serializedObject.FindProperty("startDialog");
            this.winDialog = serializedObject.FindProperty("winDialog");
            this.loseDialog = serializedObject.FindProperty("loseDialog");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            CampaignLevelData cld = this.target as CampaignLevelData;

            EditorGUILayout.PropertyField(this.internalId);
            EditorGUILayout.PropertyField(this.levelDescription);
            EditorGUILayout.PropertyField(this.sceneName);
            EditorGUILayout.PropertyField(this.startingResources);

            /*
            EditorGUILayout.PropertyField(this.gameMode);
            Type[] types = new GameModeDTP().args;
            int i = 0;
            EditorGUI.indentLevel = 2;
            foreach(Type t in types) {
                if(t == typeof(string)) {
                    cld.data[i] = EditorGUILayout.TextField(new GUIContent("String Arg"), (string)cld.data[i]);
                }
                else if(typeof(float) == t) {
                    cld.data[i] = EditorGUILayout.FloatField(new GUIContent("Float Arg"), (float)(cld.data[i] ?? 0f));
                }
                else if(t == typeof(bool)) {
                    cld.data[i] = EditorGUILayout.Toggle(new GUIContent("Bool Arg"), (bool)(cld.data[i] ?? false));
                }
                else if(t == typeof(int)) {
                    cld.data[i] = EditorGUILayout.IntField(new GUIContent("Integer Arg"), (int)(cld.data[i] ?? 0));
                }
                else {
                    EditorGUILayout.LabelField("UNKNOWN TYPE (" + t + ")");
                }
                i++;
            }
            EditorGUI.indentLevel = 0;

            if(this.gameMode.enumValueIndex == (int)EnumGameMode.DEFEND_THE_POINT) {
                EditorGUILayout.IntField(0);
            }
            */

            this.func(this.characters);

            EditorGUILayout.LabelField(string.Empty);

            this.func(this.startDialog);
            this.func(this.winDialog);
            this.func(this.loseDialog);


            serializedObject.ApplyModifiedProperties();
        }

        private void func(SerializedProperty field) {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(field, true);
            if(EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}
