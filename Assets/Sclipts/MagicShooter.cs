using System.Collections;
using TMPro;
using UnityEngine;


public class MagicShooter : MonoBehaviour
{
    [Header("魔法リスト")]
    [SerializeField] MagicData[] _magics;

    [SerializeField] Transform _startShootingPosition;
    [SerializeField] GameObject _camera;
    [SerializeField] TMP_Text _text;

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

        magic._lastShootTime = Time.time;
    }

    public void SetMagic()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) _magicIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) _magicIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) _magicIndex = 2;
    }

    public void MagicUpdate()
    {
        for(int i = 0;  i < _magics.Length; i++)
        {
            _magics[i].ApplyParametersFromID();
        }
    }

    IEnumerator MagicCooldownText()
    {
        _text.text = "クールダウン中です";
        yield return new WaitForSeconds(1);
        _text.text = "";
    }
}
