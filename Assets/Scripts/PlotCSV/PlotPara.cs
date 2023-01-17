using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotPara : MonoBehaviour
{
    // Start is called before the first frame update
    Table table;
    [SerializeField] private float time;
    [SerializeField] string fileName;

    void Start()
    {
        table = new Table();
        time = 0;

        //ヘッダーを作成
        table.AddColumn("SaveTime");
        table.AddColumn("MaxVx");
        table.AddColumn("MaxVy");
        table.AddColumn("AxNormal");
        table.AddColumn("AxBrake");
        table.AddColumn("AxJumping");
        table.AddColumn("AerialInertia");
        table.AddColumn("JumpVelocity");
        table.AddColumn("GravityRising");
        table.AddColumn("GravityFalling");
        table.AddColumn("VerticalSpeedSustainLevel");

        TableRow newRow = new TableRow();

        newRow.SetFloat("SaveTime", time);
        newRow.SetFloat("MaxVx", 1f);
        newRow.SetFloat("MaxVy", Settings.Instance.jumpParam.maxVy);
        newRow.SetFloat("AxNormal", Settings.Instance.jumpParam.axNormal);
        newRow.SetFloat("AxBrake", Settings.Instance.jumpParam.axBrake);
        newRow.SetFloat("AxJumping", Settings.Instance.jumpParam.axJumping);
        newRow.SetString("AerialInertia", Settings.Instance.jumpParam.aerialInertia.ToString());
        newRow.SetFloat("JumpVelocity", Settings.Instance.jumpParam.jumpVelocity);
        newRow.SetFloat("GravityRising", Settings.Instance.jumpParam.gravityRising);
        newRow.SetFloat("GravityFalling", Settings.Instance.jumpParam.gravityFalling);
        newRow.SetString("VerticalSpeedSustainLevel", Settings.Instance.jumpParam.verticalSpeedSustainLevel.ToString());
        table.AddRow(newRow);
    }

    void Update()
    {
        time += Time.deltaTime;

        if(10f<time)
        {
            TableRow newRow = new TableRow();

            newRow.SetFloat("SaveTime", time);
            newRow.SetFloat("MaxVx", 1f);
            newRow.SetFloat("MaxVy", Settings.Instance.jumpParam.maxVy);
            newRow.SetFloat("AxNormal", Settings.Instance.jumpParam.axNormal);
            newRow.SetFloat("AxBrake", Settings.Instance.jumpParam.axBrake);
            newRow.SetFloat("AxJumping", Settings.Instance.jumpParam.axJumping);
            newRow.SetString("AerialInertia", Settings.Instance.jumpParam.aerialInertia.ToString());
            newRow.SetFloat("JumpVelocity", Settings.Instance.jumpParam.jumpVelocity);
            newRow.SetFloat("GravityRising", Settings.Instance.jumpParam.gravityRising);
            newRow.SetFloat("GravityFalling", Settings.Instance.jumpParam.gravityFalling);
            newRow.SetString("VerticalSpeedSustainLevel", Settings.Instance.jumpParam.verticalSpeedSustainLevel.ToString());
            table.AddRow(newRow);
            table.Save("data/"+fileName+".csv");
            time = 0;
        }
    }

}
