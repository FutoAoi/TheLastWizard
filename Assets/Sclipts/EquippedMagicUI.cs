using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 装備中マジックUI（ドラッグで解除）
/// </summary>
public class EquippedMagicUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TMP_Text apText;
    [SerializeField] private TMP_Text arText;
    [SerializeField] private TMP_Text cdText;
    [SerializeField] private TMP_Text msText;
    [SerializeField] private TMP_Text levelText;

    private MagicData magic;
    private EquipSlotUI ownerSlot;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetMagic(MagicData magicData, EquipSlotUI slot)
    {
        magic = magicData;
        ownerSlot = slot;

        apText.text = "AP:" + magic.CurrentAttackLevel;
        arText.text = "AR:" + magic.CurrentRangeLevel;
        cdText.text = "CD:" + magic.CurrentCoolDownLevel;
        msText.text = "MS:" + magic.CurrentMagicSpeedLevel;
        levelText.text = magic.MagicLevel.ToString();

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        magic = null;
        ownerSlot = null;
        gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (magic == null) return;

        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // ScrollViewにドロップされなかった → 元に戻す
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public MagicData GetMagic() => magic;
    public EquipSlotUI GetOwnerSlot() => ownerSlot;
}
