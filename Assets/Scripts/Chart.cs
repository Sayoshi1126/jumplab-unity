using UnityEngine;

public class Chart : MonoBehaviour
{
    public DD_DataDiagram diagram;

    private GameObject lineX;
    private GameObject lineY;
    private GameObject lineCameraX;
    private GameObject lineCameraY;
    private GameObject lineVX;
    private GameObject lineVY;
    private GameObject lineCameraVX;
    private GameObject lineCameraVY;
    private float time;

    private void Start()
    {
        lineX = diagram.AddLine("jumperX", Color.red);
        lineY = diagram.AddLine("jumperY", Color.blue);
        //lineVX = diagram.AddLine("jumperVX", Color.red);
        //lineVY = diagram.AddLine("jumperVY", Color.blue);
        lineCameraX = diagram.AddLine("cameraX", Color.yellow);
        lineCameraY = diagram.AddLine("cameraY", Color.green);
        //lineCameraVX = diagram.AddLine("cameraVX", Color.yellow);
        //lineCameraVY = diagram.AddLine("cameraVY", Color.green);
    }

    private void Update()
    {
        time += Time.deltaTime * 5;
        if (!Settings.Instance.showVelocityChart)
        {
            var x = Settings.Instance.jumperX;
            diagram.InputPoint(lineX, new Vector2(0.1f, x + 5));
            var y = Settings.Instance.jumperY;
            diagram.InputPoint(lineY, new Vector2(0.1f, y + 5));
            var cx = Settings.Instance.cameraX;
            diagram.InputPoint(lineCameraX, new Vector2(0.1f, cx + 5));
            var cy = Settings.Instance.cameraY;
            diagram.InputPoint(lineCameraY, new Vector2(0.1f, cy + 5));
        }
        else
        {
            var x = Settings.Instance.jumperVX;
            diagram.InputPoint(lineX, new Vector2(0.1f, x + 5));
            var y = Settings.Instance.jumperVY;
            diagram.InputPoint(lineY, new Vector2(0.1f, y + 5));
            var cx = Settings.Instance.cameraVX;
            diagram.InputPoint(lineCameraX, new Vector2(0.1f, cx + 5));
            var cy = Settings.Instance.cameraVY;
            diagram.InputPoint(lineCameraY, new Vector2(0.1f, cy + 5));
        }
    }
}