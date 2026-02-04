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
    [SerializeField] private MagicSlotUI[] _magicSlotUIs;

    int _magicIndex = 0;
    private MagicData _currentMagic;
    private bool _canThunder = false;
    private bool _canIce = false;
    private int _currentMagicIndex = 1;

    public MagicData CurrentMagic => _currentMagic;
    public MagicSlotUI[] MagicSlotUIs => _magicSlotUIs;
    public MagicData[] Magics => _magics;

    public void MagicShoot()
    {
        _currentMagic = _magics[_magicIndex];

        if(Time.time - _currentMagic.LastShootTime < _currentMagic.Cooldown)
        {
            StartCoroutine(MagicCooldownText());
            return;
        }
        MagicBehavior shootMagic = MagicObjectPool.Instance.GetMagic(_magicIndex);

        shootMagic.transform.position = _startShootingPosition.position;
        shootMagic.transform.rotation = _camera.transform.rotation;

        shootMagic.AddStatus(_currentMagic.AttackPower, _currentMagic.Range, _currentMagic.MagicSpeed, _currentMagic.MagicType);

        switch(_magics[_magicIndex].MagicElement)
        {
            case MagicElement.Fire:
                AudioManager.Instance.PlaySe("Hi");
                break;
            case MagicElement.Ice:
                AudioManager.Instance.PlaySe("Koori");
                break;
            case MagicElement.Thunder:
                AudioManager.Instance.PlaySe("Kaminari");
                break;
        }
        _currentMagic.LastShootTime = Time.time;
    }

    public void SetMagic()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) _magicIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && _canThunder) _magicIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && _canIce) _magicIndex = 2;

        // マウススクロール
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) _magicIndex--;
        if (scroll < 0f) _magicIndex++;

        // 範囲制限（ループ）
        if (_magicIndex < 0) _magicIndex = _currentMagicIndex - 1;
        if (_magicIndex >= _currentMagicIndex) _magicIndex = 0;
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

    public void UpdateMagicUI()
    {
        for (int i = 0; i < _maxMagic; i++)
        {
            MagicData magic = _magics[i];

            if (magic == null)
            {
                _magicSlotUIs[i].SetCooldown(1f, 0f);
                continue;
            }

            float elapsed = Time.time - magic.LastShootTime;
            float progress = Mathf.Clamp01(elapsed / magic.Cooldown);
            float remaining = Mathf.Max(0f, magic.Cooldown - elapsed);

            _magicSlotUIs[i].SetCooldown(progress, remaining);
            _magicSlotUIs[i].SetSelected(i == _magicIndex);
        }
    }

    public async UniTask Melee()
    {
        _hitCheckerObject.SetActive(true);

        AudioManager.Instance.PlaySe("Kinsetu");

        await UniTask.Delay(TimeSpan.FromSeconds(_showCollisionObjectTime), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

        _hitCheckerObject.SetActive(false);
    }
    public void CanIce()
    {
        _canIce = true;
        _currentMagicIndex++;
    }

    public void CamThunder()
    {
        _canThunder = false;
        _currentMagicIndex++;
    }

    public void UpAttack(int skill)
    {
        _magics[skill].LevelUp(LevelType.AttackLevel);
    }

    public void UpRange(int skill)
    {
        _magics[skill].LevelUp(LevelType.RangeLevel);
    }

    public void UpCoolDown(int skill)
    {
        _magics[skill].LevelUp(LevelType.CoolDownLevel);
    }

    public void UpMagicSpeed(int skill)
    {
        _magics[skill].LevelUp(LevelType.MagicSpeedLevel);
    }
}
