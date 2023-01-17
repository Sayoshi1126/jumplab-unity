using UnityEngine;
using UnityEditor;

//EditorWindowを継承することでEditorWindowそのものになれる
public class ItemEditorWindow : EditorWindow
{
    //MenuItem属性を付加することでUnity上部のEditorからItemEditorWindowを選択できる
    //CreateWindowに付加されてる
    [MenuItem("Window/Demo/ItemEditorWindow")]
    static public void CreateWindow()
    {
        //Windowを展開
        EditorWindow.GetWindow<ItemEditorWindow>();
    }

    
    private Vector2 scrollPosition;
    private string fileTitle = "File Title";
    private string fileCaption = "File Caption";

    private GUISkin skin;
    [SerializeField] private ItemData itemData;
    [SerializeField] string itemDataPath;

    private void OnEnable()
    {
        //AssetDatabaseクラスはエディターの機能で、プロジェクトのアセットにアクセスできる
        //LoadAssetAtPath関数によりAssetsから始まる相対パスを引数に渡すと方にあったアセットをロードできる
        this.skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Editor/Editor Resources/ItemEditorGUISkin.guiskin");

        //ItemDataをロード
        this.itemData = Resources.Load<ItemData>("ItemData").Clone();

        var defaultData = Resources.Load<ItemData>("ItemData");
        this.itemData = defaultData.Clone();
        this.itemDataPath = AssetDatabase.GetAssetPath(defaultData);
    }

    [SerializeField] private int selectedIndex;
    private void OnGUI()
    {
        //デフォルトで縦置きされるが、ヘッダーの領域確保のために記述
        //EditorGUILayoutやGUILayoutに定義されてる関数はGUIStyleクラスを引数に持つ
        //GUIStyleではGUIの見た目に関するパラメータが定義されている

        using (new EditorGUILayout.VerticalScope(this.skin.GetStyle("header")))
        {
            EditorGUILayout.LabelField(itemDataPath);

            Undo.RecordObject(itemData, "Modify FileName or Caption of ItemData");
            //textFieldやtextAreaではユーザーが編集可能
            //Areaは改行可能のため、2行分のスペースを確保
            //itemDataでscriptableObjectとGUIを結合
            itemData.fileName = EditorGUILayout.TextField(itemData.fileName);
            itemData.fileCaption = EditorGUILayout.TextArea(itemData.fileCaption, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f));

            using (new EditorGUILayout.HorizontalScope())
            {
                if(GUILayout.Button("アイテムを追加"))
                {
                    int itemLength = this.itemData.items.Length;
                    Undo.RecordObject(itemData, "Add Item");
                    System.Array.Resize(ref this.itemData.items, itemLength + 1);
                    this.itemData.items[itemLength] = new ItemData.Item()
                    {
                        name = "名前を入力してください",
                        caption = "説明文",
                    };
                }
                //アイテムを追加ボタンともとに戻すボタンが隣接してると気持ち悪いのでレイアウトを考慮しつつ両端に各ボタンをそろえる
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("もとに戻す"))
                {
                    this.itemData = AssetDatabase.LoadAssetAtPath<ItemData>(this.itemDataPath).Clone();
                    EditorGUIUtility.editingTextField = false;
                };
                if(GUILayout.Button("保存"))
                {
                    //var data = Resources.Load<ItemData>("ItemData");
                    var data = AssetDatabase.LoadAssetAtPath<ItemData>(this.itemDataPath);
                    EditorUtility.CopySerialized(this.itemData, data);
                    EditorUtility.SetDirty(data);
                    AssetDatabase.SaveAssets();
                };
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {


            //カッコ内をスクロールできるようにする
            using (var scroll = new EditorGUILayout.ScrollViewScope(this.scrollPosition,GUILayout.MinWidth(320f)))
            {
                this.scrollPosition = scroll.scrollPosition;

                for (int i = 0; i < this.itemData.items.Length; i++)
                {
                    var data = this.itemData.items[i];
                    //カッコ内を平行に並べる
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField(data.id.ToString(), GUILayout.MaxWidth(50f));
                        EditorGUILayout.LabelField(data.name, GUILayout.MaxWidth(200f));
                        //GUILayout.Buttonを押していればtrueそうでなければfalse
                        if(GUILayout.Button("編集", GUILayout.MaxWidth(50f)))
                        {
                            Undo.RecordObject(this, "Select Item");
                            selectedIndex = i;
                        }

                        
                    }
                }
            }

            using (new EditorGUILayout.VerticalScope(this.skin.GetStyle("inspector")))
            {
                if (0 <= this.selectedIndex && this.selectedIndex < this.itemData.items.Length)
                {
                    Undo.RecordObject(itemData, "Modify ItemData at" + this.selectedIndex);

                    var selectedItem = this.itemData.items[this.selectedIndex];
                    selectedItem.id = EditorGUILayout.IntField("ID",selectedItem.id);
                    selectedItem.name = EditorGUILayout.TextField("アイテム名", selectedItem.name);
                    selectedItem.type = (ItemType)EditorGUILayout.EnumPopup("アイテムタイプ", selectedItem.type);
                    selectedItem.caption = EditorGUILayout.TextArea(selectedItem.caption, GUILayout.Height(EditorGUIUtility.singleLineHeight * 4f));
                }
                GUILayout.FlexibleSpace();
            }
        }

        if (Event.current.type == EventType.DragUpdated)
        {
            if(DragAndDrop.objectReferences != null 
                && DragAndDrop.objectReferences.Length > 0 
                && DragAndDrop.objectReferences[0] is ItemData)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();
            }
        }
        else if(Event.current.type == EventType.DragPerform)
        {
            Undo.RecordObject(this, "Change ItemData");
            this.itemData = ((ItemData)DragAndDrop.objectReferences[0]).Clone();
            this.itemDataPath = DragAndDrop.paths[0];
            DragAndDrop.AcceptDrag();
            Event.current.Use();
        }

        if(DragAndDrop.visualMode == DragAndDropVisualMode.Copy)
        {
            var rect = new Rect(Vector2.zero, this.position.size);
            var bgColor = Color.white * new Color(1f, 1f, 1f, 0.2f);
            EditorGUI.DrawRect(rect,bgColor);
            EditorGUI.LabelField(rect, "ここにアイテムデータをドラッグ&ドロップしてください", this.skin.GetStyle("D&D"));
        }
    }
}
