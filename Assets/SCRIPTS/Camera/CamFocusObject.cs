using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamFocusObject : MonoBehaviour
{
    CameraBehaviour _camBehaviour;
    [SerializeField] Transform focusTransform;
    bool _canFocus = true;

    private void Awake()
    {
        _camBehaviour = GameManager.gm.MainCam.GetComponent<CameraBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canFocus)
        {
            return;
        }

        if(collision.TryGetComponent(out PlayerMain pControl))
        {
            StartCoroutine(_camBehaviour.FocusTarget(focusTransform.position));
        }

        _canFocus = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMain pControl))
        {
            _canFocus = true;
        }

    }
}
