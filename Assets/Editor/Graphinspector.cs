using UnityEngine;
using UnityEditor;

public abstract class GraphInspector : Editor
{
    // グラフの描画範囲
    const double MinX = -1.0;
    const double MaxX = 1.0;
    const double MinY = -0.1;
    const double MaxY = 1.0;

    const int DataNum = 512;
    const int XLabelNum = 9;
    const int YLabelNum = 5;

    private double[] buffer = null;
    private string[] xLabel = new string[XLabelNum];
    private string[] yLabel = new string[YLabelNum];

    // Inspector拡張
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        this.Draw();
    }

    // Inspectorが画面に表示されたときに呼ばれる
    private void OnEnable()
    {
        this.Plot();
    }

    // グラフをプロット
    protected void Plot()
    {
        this.buffer = new double[DataNum];

        for (int i = 0; i < DataNum; i++)
        {
            double x = MinX + (MaxX - MinX) * i / (DataNum - 1);
            double y = this.Eval(x);
            // 変換
            y -= MinY;
            y /= (MaxY - MinY);

            buffer[i] = y;
        }

        this.xLabel = new string[XLabelNum];
        this.yLabel = new string[YLabelNum];

        for (int i = 0; i < XLabelNum; i++)
        {
            double x = MinX + (MaxX - MinX) * i / (XLabelNum - 1);
            xLabel[i] = x.ToString();
        }

        for (int i = 0; i < YLabelNum; i++)
        {
            double y = MinY + (MaxY - MinY) * i / (YLabelNum - 1);
            yLabel[i] = y.ToString();
        }
    }

    // Inspectorへグラフを描画
    private void Draw()
    {
        if (this.buffer == null) { this.Plot(); }

        GraphDrawer.DrawGraph(
            buffer,
            buffer.Length,
            Color.blue,
            GraphDrawer.Mode.LineChart,
            this.xLabel,
            this.yLabel);
    }

    protected abstract double Eval(double x);
}