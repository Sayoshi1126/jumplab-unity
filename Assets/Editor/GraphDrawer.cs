using UnityEngine;
using UnityEditor;
using System.Collections;

// グラフ描画
public class GraphDrawer
{
    public const int Grid = 10 + 1;

    private const float AreaHeight = 175f;

    private const float SoundLineScale = 1f;

    private const float SpectrumBarScale = 3f;
    private const float BarWidth = 0.5f;

    private static readonly Vector2 labelSize = new Vector2(100f, 50f);
    private static GUIStyle ylabelStyle = null;

    public enum Mode
    {
        LineChart, // 折れ線グラフ
        BarChart, // 棒グラフ
    }

    public static void DrawGraph(
      double[] buffer,
      int drawLength,
      Color color,
      Mode mode,
      string[] xLabel = null,
      string[] yLabel = null
      )
    {
        GUILayout.Space(8f);

        const float labelOffsetX = -105f;
        Rect area = GUILayoutUtility.GetRect(Screen.width / 2, AreaHeight);
        area.x += 38f;
        area.xMax -= 70f;

        // Grid
        int xdiv = Grid - 1;
        int ydiv = Grid - 1;

        if (xLabel != null) { xdiv = xLabel.Length - 1; }
        if (yLabel != null) { ydiv = yLabel.Length - 1; }

        for (int xi = 0; xi <= xdiv; ++xi)
        {
            var lineColor = (xi == 0 || xi == xdiv) ? Color.white : Color.gray;
            var lineWidth = (xi == 0 || xi == xdiv) ? 2f : 1f;
            var x = (area.width / xdiv) * xi;

            Drawing.DrawLine(
                new Vector2(area.x + x, area.y),
                new Vector2(area.x + x, area.yMax), lineColor, lineWidth, true);

            if (xLabel != null)
            {
                // x軸のラベル表示
                Vector2 vx = new Vector2(area.x + x - 4f, area.yMax + 1f);
                GUI.Label(new Rect(vx, labelSize), xLabel[xi]);
            }
        }

        for (int yi = 0; yi <= ydiv; ++yi)
        {
            var lineColor = (yi == 0 || yi == ydiv) ? Color.white : Color.gray;
            var lineWidth = (yi == 0 || yi == ydiv) ? 2f : 1f;
            var y = (area.height / ydiv) * yi;

            Drawing.DrawLine(
                new Vector2(area.x, area.y + y),
                new Vector2(area.xMax, area.y + y), lineColor, lineWidth, true);

            if (yLabel != null)
            {
                if (ylabelStyle == null)
                {
                    ylabelStyle = new GUIStyle(EditorStyles.label);
                    ylabelStyle.alignment = TextAnchor.UpperRight;
                }

                // y軸のラベル表示
                Vector2 vy = new Vector2(area.x + labelOffsetX, area.y + y - 7f);
                GUI.Label(new Rect(vy, labelSize), yLabel[ydiv - yi], ylabelStyle);
            }
        }

        if (buffer == null) { return; }

        var dx = area.width / (drawLength - 1);
        var dy = area.height;
        Vector2 previousPos = new Vector2(area.x, area.yMax);

        bool outed = true;
        int bufferLength = buffer.Length;
        if (bufferLength == 0) { Debug.LogError("bufferLength is zero"); return; }
        int pos = 0;
        for (var i = 0; i < drawLength; i++)
        {
            switch (mode)
            {
                case Mode.LineChart:
                    {
                        float x = area.x + dx * i;
                        double pre = pos > 0.0 ? buffer[pos - 1] : 0.0;
                        double value = buffer[pos];
                        float y = (float)(area.yMax - dy * value);
                        var currentPos = new Vector2(x, y);
                        if (!outed)
                        {
                            Drawing.DrawLine(previousPos, currentPos, color, 1f, true);
                        }
                        previousPos = currentPos;

                        if (value < 0 || value > 1)
                        {
                            outed = true;
                        }
                        else
                        {
                            outed = false;
                        }

                    }
                    break;
                case Mode.BarChart:
                    {
                        float x = area.x + dx * i;
                        double value = buffer[pos];
                        float barWidth = BarWidth;

                        value = MyMath.Clamp01(value);

                        float y = (float)(area.yMax - dy * value);
                        var currentPos = new Vector2(x, y);
                        Drawing.DrawLine(new Vector2(currentPos.x, area.yMax), currentPos, color, barWidth, true);
                    }
                    break;
                default:
                    break;
            }

            pos++;
        }

        GUILayout.Space(16f);
    }

    public static class MyMath
    {
        // UnityEngine.Mathf.Clamp01のdouble版
        public static double Clamp01(double value)
        {
            if (value < 0.0) { return 0.0; }
            if (value > 1.0) { return 1.0; }
            return value;
        }
    }
}