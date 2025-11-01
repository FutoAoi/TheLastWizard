using System.Collections.Generic;
using UnityEngine;


public class MagicShooter : MonoBehaviour
{
    [Header("–‚–@ƒŠƒXƒg")]
    [SerializeField] MagicData[] _magics;

    [SerializeField] Transform _startShootingPosition;
    [SerializeField] GameObject _camera;

    int _magicIndex = 0;

    public void MagicShoot()
    {
        MagicData magic = _magics[_magicIndex];

        if(Time.time - magic.LastShootTime < magic.Cooldown)
        {
            return;
        }
        MagicBehavior shootmagic = Instantiate(magic.MagicPrefab, _startShootingPosition.position, _camera.transform.rotation);
        shootmagic.AddStatus(magic.AttackPower, magic.Range, magic.MagicSpeed);
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
}
