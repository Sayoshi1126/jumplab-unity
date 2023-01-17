using UnityEngine;

//GUIのアイテムタイプと連動する列挙クラス
public enum ItemType
{
    Weapon,
    Armor,
    Heal,
    Trap,
    ReinforcementMaterial,
    EvolutionaryMaterial,
    Important,
}

// ScriptableObjectを.assetでデータ保存するメニューを追加する専用の属性
[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    //Unityにシリアライズ可能であることを宣言
    //シリアライズとはデータを保存形式に沿って保存すること
    [System.Serializable]
    public class Item
    {
        public int id;
        public string name;
        public string caption;
        public ItemType type;
    }

    public string fileName;
    public string fileCaption;
    public Item[] items;

    public ItemData Clone()
    {
        return Instantiate(this);
    }
}
