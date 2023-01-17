using UnityEditor;
using UnityEngine;

public class PlayerSettingEditorWindow : EditorWindow
{

    [MenuItem("Window/JumperSetting/PlayerEditorWindow")]
    static public void CreateWindow()
    {
        EditorWindow.GetWindow<PlayerSettingEditorWindow>();
    }

    private GUISkin skin;
    private PlayerParaData playerData;
    private string playerDataPath;

    private bool updateFromSettings;

    private void OnEnable()
    {
        //this.skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Editor/Editor Resources/ItemEditorGUISkin.guiskin");
        //this.playerData = Resources.Load<PlayerParaData>("PlayerPara").Clone();

        var defaultData = Resources.Load<PlayerParaData>("PlayerPara");
        this.playerData = defaultData.Clone();
        this.playerDataPath = AssetDatabase.GetAssetPath(defaultData);

        updateFromSettings = false;
    }

    private Vector2 scrollPosition;
    private string paraCaption = "パラメタ説明文";

    private int selectedIndex;
    private void OnGUI()
    {
        using (new EditorGUILayout.VerticalScope())
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField(playerDataPath);
                //Undo.RecordObject(playerData, "Modify FileName or Caption of PlayerData");
                playerData.fileTitle = EditorGUILayout.TextField(playerData.fileTitle);

                EditorGUILayout.TextField("設定名", "名前を書く");
                playerData.fileCaption = EditorGUILayout.TextArea(playerData.fileCaption, GUILayout.Height(EditorGUIUtility.singleLineHeight * 4f));

                //EditorGUILayout.TextArea(fileCaption, GUILayout.Height(EditorGUIUtility.singleLineHeight * 4f));
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("追加"))
                    {
                        int dataLength = this.playerData.Params.Length;
                        Undo.RecordObject(playerData, "Add preset");
                        System.Array.Resize(ref this.playerData.Params, dataLength + 1);
                        this.playerData.Params[dataLength] = new PlayerParaData.JumpParam()
                        {
                            name = "名前を入力してください",
                            caption = "説明文",
                        };
                    }
                    if (GUILayout.Button("複製"))
                    {
                        int dataLength = this.playerData.Params.Length;
                        Undo.RecordObject(playerData, "Add preset");
                        System.Array.Resize(ref this.playerData.Params, dataLength + 1);

                        var selectedPara = this.playerData.Params[selectedIndex];

                        this.playerData.Params[dataLength] = new PlayerParaData.JumpParam()
                        {
                            name = "名前を入力してください",
                            caption = "説明文",

                            MaxVx = selectedPara.MaxVx,
                            MaxVy = selectedPara.MaxVy,
                            AxNormal = selectedPara.AxNormal,
                            AxBrake = selectedPara.AxBrake,
                            AxJumping = selectedPara.AxJumping,
                            AerialInertia = selectedPara.AerialInertia,
                            JumpVelocity = selectedPara.JumpVelocity,
                            GravityRising = selectedPara.GravityRising,
                            GravityFalling = selectedPara.GravityFalling,
                            VerticalSpeedSustainLevel = selectedPara.VerticalSpeedSustainLevel,
                        };
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.Button("削除");
                    if (GUILayout.Button("保存"))
                    {
                        //var data = Resources.Load<PlayerParaData>(this.playerDataPath);
                        var data = AssetDatabase.LoadAssetAtPath<PlayerParaData>(this.playerDataPath);
                        EditorUtility.CopySerialized(this.playerData, data);
                        EditorUtility.SetDirty(data);
                        AssetDatabase.SaveAssets();
                    }
                }
                for (int i = 0; i < this.playerData.Params.Length; i++)
                {
                    var data = this.playerData.Params[i];
                    var selectedPara = this.playerData.Params[selectedIndex];

                    if (selectedIndex == i)
                    {
                        using (new EditorGUILayout.VerticalScope(GUI.skin.label))
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField(data.id.ToString(), GUILayout.MaxWidth(50f));
                                data.name = EditorGUILayout.TextField(data.name, GUILayout.MaxWidth(200f));
                                if (GUILayout.Button("編集", GUILayout.MaxWidth(50f)))
                                {
                                    Undo.RecordObject(playerData, "Select preset");
                                    selectedIndex = i;
                                }
                            }
                        }
                    }
                    else
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField(data.id.ToString(), GUILayout.MaxWidth(50f));
                            data.name = EditorGUILayout.TextField(data.name, GUILayout.MaxWidth(200f));
                            if (GUILayout.Button("編集", GUILayout.MaxWidth(50f)))
                            {
                                Undo.RecordObject(playerData, "Select preset");
                                selectedIndex = i;

                                updateFromSettings = true;

                            }
                        }
                    }
                }
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                var selectedPara = this.playerData.Params[selectedIndex];
                if (0 <= this.selectedIndex && this.selectedIndex < this.playerData.Params.Length)
                {
                    using (var scroll = new EditorGUILayout.ScrollViewScope(this.scrollPosition, GUILayout.MinWidth(620f)))
                    {
                        this.scrollPosition = scroll.scrollPosition;

                        Undo.RecordObject(playerData, "Modify PlayerSetting at" + this.selectedIndex);


                        selectedPara.id = selectedIndex;
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            selectedPara.MaxVx = EditorGUILayout.FloatField("MaxVx", selectedPara.MaxVx);
                            selectedPara.MaxVy = EditorGUILayout.FloatField("MaxVy", selectedPara.MaxVy);
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            selectedPara.AxNormal = EditorGUILayout.FloatField("AxNormal", selectedPara.AxNormal);
                            selectedPara.AxBrake = EditorGUILayout.FloatField("AxBrake", selectedPara.AxBrake);
                            selectedPara.AxJumping = EditorGUILayout.FloatField("AxJumping", selectedPara.AxJumping);
                        }
                        selectedPara.AerialInertia = EditorGUILayout.Toggle("Aerial Inertia", selectedPara.AerialInertia);
                        selectedPara.JumpVelocity = EditorGUILayout.FloatField("JumpVelocity", selectedPara.JumpVelocity);
                        selectedPara.GravityRising = EditorGUILayout.FloatField("GravityRising", selectedPara.GravityRising);
                        selectedPara.GravityFalling = EditorGUILayout.FloatField("GravityFalling", selectedPara.GravityFalling);
                        selectedPara.VerticalSpeedSustainLevel = EditorGUILayout.FloatField("VerticalSpeedSustainLevel", selectedPara.VerticalSpeedSustainLevel);

                        if (!EditorApplication.isPlaying||updateFromSettings)
                        {
                            Settings.Instance.jumpParam.maxVx = selectedPara.MaxVx;
                            Settings.Instance.jumpParam.maxVy = selectedPara.MaxVy;
                            Settings.Instance.jumpParam.axNormal = selectedPara.AxNormal;
                            Settings.Instance.jumpParam.axBrake = selectedPara.AxBrake;
                            Settings.Instance.jumpParam.axJumping = selectedPara.AxJumping;
                            Settings.Instance.jumpParam.aerialInertia = selectedPara.AerialInertia;
                            Settings.Instance.jumpParam.jumpVelocity = selectedPara.JumpVelocity;
                            Settings.Instance.jumpParam.gravityFalling = selectedPara.GravityFalling;
                            Settings.Instance.jumpParam.gravityRising = selectedPara.GravityRising;
                            Settings.Instance.jumpParam.verticalSpeedSustainLevel = selectedPara.VerticalSpeedSustainLevel;
                            updateFromSettings = false;
                        }
                        else
                        {
                            selectedPara.MaxVx = Settings.Instance.jumpParam.maxVx;
                            selectedPara.MaxVy = Settings.Instance.jumpParam.maxVy;
                            selectedPara.AxNormal = Settings.Instance.jumpParam.axNormal;
                            selectedPara.AxBrake = Settings.Instance.jumpParam.axBrake;
                            selectedPara.AxJumping = Settings.Instance.jumpParam.axJumping;
                            selectedPara.AerialInertia = Settings.Instance.jumpParam.aerialInertia;
                            selectedPara.JumpVelocity = Settings.Instance.jumpParam.jumpVelocity;
                            selectedPara.GravityFalling = Settings.Instance.jumpParam.gravityFalling;
                            selectedPara.GravityRising = Settings.Instance.jumpParam.gravityRising;
                            selectedPara.VerticalSpeedSustainLevel = Settings.Instance.jumpParam.verticalSpeedSustainLevel;

                        }
                    }

                    using (new EditorGUILayout.VerticalScope())
                    {
                        EditorGUILayout.LabelField("コメント");
                        selectedPara.comment = EditorGUILayout.TextArea(selectedPara.comment, GUILayout.Height(EditorGUIUtility.singleLineHeight * 4f));
                        //paraCaption = EditorGUILayout.TextArea(paraCaption, GUILayout.Height(EditorGUIUtility.singleLineHeight * 4f));
                    }
                }
            }
        }

        if (Event.current.type == EventType.DragUpdated)
        {
            if (DragAndDrop.objectReferences != null
                && DragAndDrop.objectReferences.Length > 0
                && DragAndDrop.objectReferences[0] is PlayerParaData)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();
            }
        }
        else if (Event.current.type == EventType.DragPerform)
        {
            Undo.RecordObject(this, "Change PlayerParaData");
            this.playerData = ((PlayerParaData)DragAndDrop.objectReferences[0]).Clone();
            this.playerDataPath = DragAndDrop.paths[0];
            DragAndDrop.AcceptDrag();
            Event.current.Use();
            updateFromSettings = true;
        }

        if (DragAndDrop.visualMode == DragAndDropVisualMode.Copy)
        {
            var rect = new Rect(Vector2.zero, this.position.size);
            var bgColor = Color.white * new Color(1f, 1f, 1f, 0.2f);
            EditorGUI.DrawRect(rect, bgColor);
            EditorGUI.LabelField(rect, "ここにアイテムデータをドラッグ&ドロップしてください", this.skin.GetStyle("D&D"));
        }
    }
}
