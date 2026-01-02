using UnityEngine;

/// <summary>
/// 所持マジック一覧を表示する
/// </summary>
public class MagicInventoryUI : MonoBehaviour
{
    public MagicInventory inventory;
    public MagicItemUI magicItemPrefab;
    public Transform contentParent;

    void Start()
    {
        foreach (MagicData magic in inventory.ownedMagics)
        {
            MagicItemUI item = Instantiate(magicItemPrefab, contentParent);
            item.Setup(magic);
        }
    }
}