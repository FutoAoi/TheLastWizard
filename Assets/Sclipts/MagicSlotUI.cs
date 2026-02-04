using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicSlotUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _elementIcon;
    [SerializeField] private Image _TypeIcon;

    [SerializeField] private Image _cooldownMask;
    [SerializeField] private TextMeshProUGUI _cooldownText;

    [SerializeField] private float _cooldownAlpha = 0.3f;


    public void SetIcon(MagicData magic)
    {
        _elementIcon.sprite = GameManager.instance.IconData.GetElementIcon(magic.MagicElement);
        _TypeIcon.sprite = GameManager.instance.IconData.GetTypeIcon(magic.MagicType);
    }

    public void SetCooldown(float progress, float remaining)
    {
        float alpha = Mathf.Lerp(_cooldownAlpha, 1f, progress);
        _canvasGroup.alpha = alpha;

        _cooldownMask.fillAmount = 1f - progress;

        if (remaining > 0f)
        {
            _cooldownText.gameObject.SetActive(true);
            _cooldownText.text = Mathf.CeilToInt(remaining).ToString();
        }
        else
        {
            _cooldownText.gameObject.SetActive(false);
        }
    }

    public void SetSelected(bool isSelected)
    {
        transform.localScale = isSelected ? Vector3.one * 1.2f : Vector3.one;
    }
}