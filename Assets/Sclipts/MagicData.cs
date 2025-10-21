using UnityEngine;

[System.Serializable]
public class MagicData
{
    [SerializeField] private string _magicID; // 魔法のID
    [SerializeField] private MagicBehavior _magicPrefab; // 発射する魔法のPrefab
    private int _attackPower; // 攻撃力
    private int _range; // 射程
    private int _cooldown; // クールダウン時間(秒)
    private int _magicSpeed; // 詠唱速度

    [HideInInspector] public float _lastShootTime; // 最後に撃った時間

    public float LastShootTime => _lastShootTime;
    public MagicBehavior MagicPrefab => _magicPrefab;
    public string MagicID => _magicID;
    public int AttackPower => _attackPower;
    public int Range => _range;
    public int Cooldown => _cooldown;
    public int MagicSpeed => _magicSpeed;


    //MagicGenerator との同期関数
    public void GenerateIDFromParameters()
    {
        var parameters = new MagicGenerator.MagicParameters(_attackPower, _range, _cooldown, _magicSpeed);
        _magicID = MagicGenerator.GetID(parameters);
    }
    
    //ID から値を復元
    public void ApplyParametersFromID()
    {
        var parameters = MagicGenerator.GetMagicParameters(_magicID);
        _attackPower = parameters.attackPower;
        _range = parameters.range;
        _cooldown = parameters.cooldown;
        _magicSpeed = parameters.magicSpeed;
        _lastShootTime = -parameters.cooldown;
    }
}