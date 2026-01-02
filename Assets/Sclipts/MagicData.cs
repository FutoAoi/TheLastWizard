using UnityEngine;

[System.Serializable]
public class MagicData
{
    [HideInInspector] public float LastShootTime; // ÅŒã‚ÉŒ‚‚Á‚½ŽžŠÔ

    [SerializeField] private MagicBehavior _magicPrefab;
    [SerializeField] private MagicElement _magicElement;
    [SerializeField] private MagicType _magicType;
    [SerializeField] private int _currentAttackLevel = 1;
    [SerializeField] private int _currentRangeLevel = 1;
    [SerializeField] private int _currentCoolDownLevel = 1;
    [SerializeField] private int _currentMagicSpeedLevel = 1;

    private float _baseAttackPower = 1;
    private float _baseRange = 5;
    private float _baseCooldown = 3;
    private float _baseMagicSpeed = 5;
    private int _magicLevel = 1;

    public MagicBehavior MagicPrefab => _magicPrefab;
    public MagicElement MagicElement => _magicElement;
    public MagicType MagicType => _magicType;
    public float AttackPower => _baseAttackPower + (0.5f * _currentAttackLevel);
    public float Range => _baseRange + (0.5f * _currentRangeLevel);
    public float Cooldown => _baseCooldown - ( 0.2f * _currentCoolDownLevel);
    public float MagicSpeed => _baseMagicSpeed + (0.5f * _currentMagicSpeedLevel);
    public int CurrentAttackLevel => _currentAttackLevel;
    public int CurrentRangeLevel => _currentRangeLevel;
    public int CurrentCoolDownLevel => _currentCoolDownLevel;
    public int CurrentMagicSpeedLevel => _currentMagicSpeedLevel;
    public int MagicLevel => _magicLevel;

    public void LevelUp(LevelType type)
    {
        switch(type)
        {
            case LevelType.AttackLevel:
                _currentAttackLevel++;
                break;
            case LevelType.RangeLevel:
                _currentCoolDownLevel++;
                break;
            case LevelType.CoolDownLevel:
                _currentCoolDownLevel++;
                break;
            case LevelType.MagicSpeedLevel:
                _currentMagicSpeedLevel++;
                break;
        }
    }
}