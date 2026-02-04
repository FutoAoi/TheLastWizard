using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _popupText;
    [SerializeField] float _floatSpeed = 1f;
    [SerializeField] float _duration = 0.5f;

    public static void Create(Vector3 position, float danage)
    {
        var resourcePrefab = Resources.Load<GameObject>("DamagePopup");
        GameObject popup = Instantiate(resourcePrefab, position, Quaternion.identity);
        popup.transform.LookAt(Camera.main.transform);
        popup.transform.Rotate(0, 180f, 0);
        popup.GetComponent<DamagePopup>().SetDamage(danage);
    }
    private void SetDamage(float danage)
    {
        _popupText.text = danage.ToString();
        Destroy(gameObject, _duration);
    }
    private void Update()
    {
        transform.position += Vector3.up * _floatSpeed * Time.deltaTime;
    }
}
