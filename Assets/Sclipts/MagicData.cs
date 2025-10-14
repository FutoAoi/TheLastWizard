using UnityEngine;

[ExecuteAlways]
public class MagicData : MonoBehaviour
{
    [Header("マジックID")]
    [SerializeField] private string _magicID;

    [Header("各パラメータ")]
    [SerializeField] private int _attackPower;
    [SerializeField] private int _range;
    [SerializeField] private int _cooldown;
    [SerializeField] private int _chantingSpeed;

    private void GenerateID()
    {
        _magicID = $"{_attackPower:D3}{_range:D3}{_cooldown:D3}{_chantingSpeed:D3}";
    }

    private void GenerateParameter(string id)
    {
        string attackString = id.Substring(0, 3);
        string rangeString = id.Substring(3, 3);
        string cooldownString = id.Substring(6, 3);
        string chantingSpeedString = id.Substring(9, 3);

        _attackPower = int.Parse(attackString);
        _range = int.Parse(rangeString);
        _cooldown = int.Parse(cooldownString);
        _chantingSpeed = int.Parse(chantingSpeedString);
    }

    private void OnValidate()
    {
        if(!string.IsNullOrEmpty(_magicID) && _magicID.Length == 12)
        {
            GenerateParameter(_magicID);
        }
        else
        {
            GenerateID();
        }
    }
}
