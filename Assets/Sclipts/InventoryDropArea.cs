using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 装備解除ドロップエリア（ScrollView）
/// </summary>
public class InventoryDropArea : MonoBehaviour, IDropHandler
{
    public MagicInventory inventory;
    public MagicItemUI magicItemPrefab;
    public Transform contentParent;

    public void OnDrop(PointerEventData eventData)
    {
        EquippedMagicUI equippedUI =
            eventData.pointerDrag?.GetComponent<EquippedMagicUI>();

        if (equippedUI == null) return;

        MagicData magic = equippedUI.GetMagic();
        EquipSlotUI slot = equippedUI.GetOwnerSlot();

        if (inventory.Unequip(magic))
        {
            // ScrollViewに戻す
            MagicItemUI item =
                Instantiate(magicItemPrefab, contentParent);
            item.Setup(magic);

            // スロットを空に
            slot.ClearSlot();
        }
    }
}
