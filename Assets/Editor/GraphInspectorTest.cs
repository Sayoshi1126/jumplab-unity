using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GraphTest))]
public class GraphInspectorTest : GraphInspector
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    protected override double Eval(double x)
    {
        GraphTest graph = target as GraphTest;
        return graph.Eval(x);
    }
}