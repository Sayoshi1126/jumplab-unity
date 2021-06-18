using UnityEngine;

public class Example : MonoBehaviour
{
    public DD_DataDiagram diagram;

    private GameObject line;
    private float time;

    private void Start()
    {
        line = diagram.AddLine("ピカチュウ", Color.yellow);
    }

    private void Update()
    {
        time += Time.deltaTime * 5;
        var y = (Mathf.Sin(time) + 1) * 2 + 3;
        diagram.InputPoint(line, new Vector2(0.1f, y));
    }
}