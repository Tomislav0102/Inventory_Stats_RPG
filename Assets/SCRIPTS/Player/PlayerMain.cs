using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK;
using TK.Enums;

public class PlayerMain : MonoBehaviour
{
    internal GameManager gm;
    internal float _hor;
    internal float _ver;
    public bool _aktivControl = true;

    private void Awake()
    {
        Initialization();
    }
    public virtual void Initialization()
    {
        gm = GameManager.gm;

    }
    private void OnEnable()
    {
        InventoryUtilities.MainMenuOpen += Method_Aktiv;
    }
    private void OnDisable()
    {
        InventoryUtilities.MainMenuOpen -= Method_Aktiv;
    }
    void Method_Aktiv(bool b)
    {
        _aktivControl = !b;
    }

    public virtual void MainUpdate()
    {

        _hor = Input.GetAxis("Horizontal");
        _ver = Input.GetAxis("Vertical");

    }
    private void Update()
    {
        MainUpdate();
    }


}
