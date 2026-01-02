using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マジックの所持・装備管理
/// </summary>
public class MagicInventory : MonoBehaviour
{
    [Header("所持しているマジック")]
    public List<MagicData> ownedMagics = new List<MagicData>();

    [Header("装備しているマジック（最大3つ）")]
    public List<MagicData> equippedMagics = new List<MagicData>();

    public const int MAX_EQUIP = 3;

    /// <summary>
    /// マジックを装備する
    /// </summary>
    public bool Equip(MagicData magic)
    {
        // すでに装備していないか
        if (equippedMagics.Contains(magic))
            return false;

        // 装備数チェック
        if (equippedMagics.Count >= MAX_EQUIP)
            return false;

        // 所持から削除 → 装備に追加
        ownedMagics.Remove(magic);
        equippedMagics.Add(magic);

        return true;
    }

    /// <summary>
    /// マジックを装備解除する
    /// </summary>
    public bool Unequip(MagicData magic)
    {
        if (!equippedMagics.Contains(magic))
            return false;

        equippedMagics.Remove(magic);
        ownedMagics.Add(magic);

        return true;
    }
}