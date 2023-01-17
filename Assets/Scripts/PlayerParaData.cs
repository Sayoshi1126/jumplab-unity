using UnityEngine;

[CreateAssetMenu(fileName="PlayerParaData", menuName = "ScriptableObjects/PlayerParaData" )]
public class PlayerParaData : ScriptableObject
{

    [System.Serializable]
    public class JumpParam
    {
        public int id;
        public string name;
        public string caption;

        public string comment;

        public float MaxVx;
        public float MaxVy;
        public float AxNormal;
        public float AxBrake;
        public float AxJumping;
        public bool AerialInertia;
        public float JumpVelocity;
        public float GravityRising;
        public float GravityFalling;
        public float VerticalSpeedSustainLevel;
    }

    public JumpParam[] Params;
    public string fileTitle="使用者氏名";
    public string fileCaption="説明文をここにかく";

    public PlayerParaData Clone()
    {
        return Instantiate(this);
    }
}
