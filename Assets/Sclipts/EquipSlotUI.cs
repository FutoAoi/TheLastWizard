using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 装備スロット（枠）
/// </summary>
public class EquipSlotUI : MonoBehaviour, IDropHandler
{
    public MagicInventory inventory;
    public EquippedMagicUI equippedMagicUI; // 子UI

    private MagicData equippedMagic;

    /// <summary>
    /// 所持UIからドロップされた（装備）
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        MagicItemUI item =
            eventData.pointerDrag?.GetComponent<MagicItemUI>();

        if (item == null) return;
        if (equippedMagic != null) return;

        if (inventory.Equip(item.magicData))
        {
            equippedMagic = item.magicData;
            equippedMagicUI.SetMagic(equippedMagic, this);
            Destroy(item.gameObject);
        }
    }

    /// <summary>
    /// 装備解除時に呼ばれる
    /// </summary>
    public void ClearSlot()
    {
        equippedMagic = null;
        equippedMagicUI.Clear();
    }

    public MagicData GetMagic() => equippedMagic;
}
