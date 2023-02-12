using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnims : PlayerMain
{
    Animator _anim;



    public override void Initialization()
    {
        base.Initialization();
        _anim = GetComponent<Animator>();
    }

    public override void MainUpdate()
    {
        base.MainUpdate();

        if (!_aktivControl || gm.invManager.OpenWindows())
        {
            _anim.Play("idle");
            return;
        }

        if (_hor == 0f && _ver == 0f)
        {
            _anim.Play("idle");
        }
        else if (_hor > 0f)
        {
            _anim.Play("right");
        }
        else if (_hor < 0f)
        {
            _anim.Play("left");
        }
        else if (_ver > 0f)
        {
            _anim.Play("up");
        }
        else if (_ver < 0f)
        {
            _anim.Play("down");
        }
    }


}
