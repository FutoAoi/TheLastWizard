using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;


public class PlayerAttackManager : MonoBehaviour
{
    [Header("魔法リスト")]
    private const int _maxMagic = 3;
    [SerializeField] MagicData[] _magics = new MagicData[_maxMagic];

    [SerializeField] Transform _startShootingPosition;
    [SerializeField] GameObject _camera;
    [SerializeField] TMP_Text _text;
    [SerializeField, Tooltip("攻撃判定の表示時間")] private float _showCollisionObjectTime;
    [SerializeField, Tooltip("当たり判定のコライダー")] private GameObject _hitCheckerObject;

    int _magicIndex = 0;

    public void MagicShoot()
    {
        MagicData magic = _magics[_magicIndex];

        if(Time.time - magic.LastShootTime < magic.Cooldown)
        {
            StartCoroutine(MagicCooldownText());
            return;
        }
        MagicBehavior shootMagic = MagicObjectPool.Instance.GetMagic(_magicIndex);

        shootMagic.transform.position = _startShootingPosition.position;
        shootMagic.transform.rotation = _camera.transform.rotation;

        shootMagic.AddStatus(magic.AttackPower, magic.Range, magic.MagicSpeed);

        magic.LastShootTime = Time.time;
    }

    public void SetMagic()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) _magicIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) _magicIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) _magicIndex = 2;
    }

    IEnumerator MagicCooldownText()
    {
        _text.text = "クールダウン中です";
        yield return new WaitForSeconds(1);
        _text.text = "";
    }

    /// <summary>
    /// 魔法を装備
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="magic"></param>
    public void EquipMagic(int slotIndex, MagicData magic)
    {
        if(slotIndex < 0 || slotIndex >= _magicIndex)
        {
            Debug.LogError("スロット番号が不正です");
            return;
        }
        _magics[slotIndex] = magic;
    }

    /// <summary>
    /// 魔法を外す
    /// </summary>
    /// <param name="slotIndex"></param>
    public void UnequipMagic(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _maxMagic)
        {
            Debug.LogError("スロット番号が不正です");
            return;
        }

        _magics[slotIndex] = null;
    }

    public async UniTask Melee()
    {
        _hitCheckerObject.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_showCollisionObjectTime), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

        _hitCheckerObject.SetActive(false);
    }
}
