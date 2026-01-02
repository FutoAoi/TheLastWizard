using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

/// <summary>
/// マジック1つ分のUI
/// </summary>
public class MagicItemUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public MagicData magicData;
    [SerializeField] private Image _elementImage;
    [SerializeField] private Image _typeImage;
    [SerializeField] private TMP_Text _apText;
    [SerializeField] private TMP_Text _arText;
    [SerializeField] private TMP_Text _cdText;
    [SerializeField] private TMP_Text _msText;
    [SerializeField] private TMP_Text _magicLevelText;

    private Transform originalParent;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        // RectTransform を取得
        rectTransform = GetComponent<RectTransform>();

        // 一番上の Canvas を取得
        canvas = GetComponentInParent<Canvas>();

        // CanvasGroup（Raycast制御用）
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Setup(MagicData data)
    {
        magicData = data;
        _apText.text = "AP:" + data.CurrentAttackLevel;
        _arText.text = "AR:" + data.CurrentRangeLevel;
        _cdText.text = "CD:" + data.CurrentCoolDownLevel;
        _msText.text = "MS:" + data.CurrentMagicSpeedLevel;
        _magicLevelText.text = $"{data.MagicLevel}";
    }

    // ドラッグ開始
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        // 一番上の Canvas の子にする（座標ズレ防止）
        transform.SetParent(canvas.transform);

        // ドロップ先を貫通させる
        canvasGroup.blocksRaycasts = false;
    }

    // ドラッグ中
    public void OnDrag(PointerEventData eventData)
    {
        // Canvas座標に変換して追従
        rectTransform.anchoredPosition +=
        eventData.delta / canvas.scaleFactor;
    }

    // ドラッグ終了
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // 装備されなかったら元に戻る
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
