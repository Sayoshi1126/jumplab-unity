using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVtest : MonoBehaviour
{
    Table table;

    void Start()
    {
        table = new Table();

        //ヘッダーを作成
        table.AddColumn("username");
        table.AddColumn("intValue");
        table.AddColumn("floatValue");
    }

    void Update()
    {
        TableRow newRow = new TableRow();
        newRow.SetString("username", "jon");//String型を追加
        newRow.SetInt("intValue", Random.Range(0, 10));//int型を追加
        newRow.SetFloat("floatValue", Random.Range(0.0f, 10.0f));//float型を追加
        table.AddRow(newRow);

        //スペースキーを押すと
        //Assetsのなかのdataフォルダに追加される
        if (Input.GetKeyDown(KeyCode.Space)) table.Save("data/hoge.csv");
    }
}
