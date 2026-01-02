using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _interactDistance = 3f;
    [SerializeField] private KeyCode _keyCode = KeyCode.F;

    [Header("UIê›íË")]
    [SerializeField] TMP_Text _interactText;

    IInteractable _target;

    private void Update()
    {
        CheckTarget();
        if(_target != null && Input.GetKeyDown(_keyCode))
        {
            _target.Interact(this);
        }
    }

    private void CheckTarget()
    {
        Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _interactDistance))
        {
            _target = hit.collider.GetComponent<IInteractable>();

            if( _target != null )
            {
                ShowText(_target.GetInteractText());
                return;
            }
        }
        _target = null;
        HideText();
    }

    private void ShowText(string text)
    {
        if( _interactText != null )
        {
            _interactText.text = text;
            _interactText.gameObject.SetActive(true);
        }
    }

    private void HideText()
    {
        if (_interactText != null)
        {
            _interactText.gameObject.SetActive(false);
        }
    }
}
