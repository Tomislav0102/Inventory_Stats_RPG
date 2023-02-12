using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK;

public class CameraBehaviour : MonoBehaviour
{
    GameManager gm;
    Transform _myTransform;
    float _camPosX, _camPosY;
    Vector3 dest;
    const float CONSTANT_FOLLOWSPEED = 2f;
    float _timer;
    Camera cam;
    Vector2 camZoomBoundaries = new Vector2(3f, 8f);

    private void Awake()
    {
        gm = GameManager.gm;
        _myTransform = transform;
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void LateUpdate()
    {
        _camPosX = gm.PlTransform.position.x;
        _camPosX = Mathf.Clamp(_camPosX, -1f, 19f);
        _camPosY = gm.PlTransform.position.y;
        _camPosY = Mathf.Clamp(_camPosY, 0f, 23.5f);
        if (gm.PlayerMove._aktivControl) dest = new Vector3(_camPosX, _camPosY, -10f);

        if (dest != _myTransform.position)
        {
            _timer += Time.deltaTime * CONSTANT_FOLLOWSPEED;
            _myTransform.position = Vector3.Lerp(_myTransform.position, dest, _timer);
        }
        _timer = 0f;
    }


    void Zoom(float increment)
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, camZoomBoundaries.x, camZoomBoundaries.y);
    }
    public IEnumerator FocusTarget(Vector3 tar)
    {
        gm.PlayerMove._aktivControl = false;
        dest = tar;
        yield return new WaitForSeconds(2f);
        gm.PlayerMove._aktivControl = true;
    }
}
