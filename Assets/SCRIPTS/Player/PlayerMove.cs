using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerMain
{
    Rigidbody2D _r2D;
    [SerializeField] float _speed = 10f;
    Vector2 _direction;
    Transform _myTransform;
    float _disIncremental;
    /*[HideInInspector]*/ public float disTotal;
    Vector2 _prevPos, _currPos;
    [SerializeField] LayerMask l_world;

    private void Start()
    {
        _prevPos = _currPos = _myTransform.position;
    }
    public override void Initialization()
    {
        base.Initialization();
        _r2D = GetComponent<Rigidbody2D>();
        _myTransform = transform;
    }
    public override void MainUpdate()
    {
        base.MainUpdate();
        if (!_aktivControl || gm.invManager.OpenWindows())
        {
            return;
        }

        _direction = new Vector2(_hor, _ver).normalized;

        _currPos = _myTransform.position;
        _disIncremental = Vector2.Distance(_prevPos, _currPos);
        if (!Mathf.Approximately(_disIncremental, 0f))
        {
            _prevPos = _currPos;
        }
        disTotal += _disIncremental;
    }


    private void FixedUpdate()
    {
        if (!_aktivControl || gm.invManager.OpenWindows())
        {
            _r2D.velocity = Vector2.zero;
            return;
        }

        _r2D.velocity = _speed * _direction;
    }



}
