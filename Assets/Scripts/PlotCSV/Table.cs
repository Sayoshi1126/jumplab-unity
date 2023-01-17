using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Table
{
    List<TableRow> rows;
    List<string> columns;
    string savePath;

    public Table()
    {
        rows = new List<TableRow>();
        columns = new List<string>();
    }

    public void AddColumn(string column)
    {
        columns.Add(column);
    }


    public void AddRow(TableRow _row)
    {
        rows.Add(_row);
    }

    public void Save(String _fileName)
    {

        string filePath = Path.Combine(Application.dataPath, @_fileName);
        string dirPath = filePath.Substring(0, filePath.LastIndexOf("/", StringComparison.CurrentCulture));

        if (!Directory.Exists(dirPath))
        Directory.CreateDirectory(dirPath);

        // Tableファイルに書き込む
        try
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {

                //最初にcolumns（ヘッダーの内容）を書き込む
                sw.WriteLine(Format(columns));

                //取得した情報を書き込む
                foreach (TableRow row in rows)
                    sw.WriteLine(Format(row));
                sw.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    string Format(List<string> _columns)
    {
        string str = "";
        int lastIndex = _columns.Count - 1;

        for (int i = 0; i < lastIndex; i++)
            str += _columns[i] + ",";

        str += _columns[lastIndex];

        return str;

    }
    string Format(TableRow row)
    {
        string str = "";
        int lastIndex = columns.Count - 1;

        for (int i = 0; i < lastIndex; i++)
            str += row.GetObject(columns[i]) + ",";

        str += row.GetObject(columns[lastIndex]);

        return str;

    }
}
