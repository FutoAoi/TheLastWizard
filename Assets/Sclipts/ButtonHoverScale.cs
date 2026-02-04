using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHoverScale : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private Vector3 defaultScale;

    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float duration = 0.2f;

    void Start()
    {
        defaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(defaultScale * hoverScale, duration);
        AudioManager.Instance.PlaySe("Button");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(defaultScale, duration);
    }
}
