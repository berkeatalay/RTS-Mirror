using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] UnitMovement unitMovement = null;
    [SerializeField] UnityEvent onSelected = null;
    [SerializeField] UnityEvent onDeSelected = null;

    public UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }
    #region Server

    #endregion

    #region Client

    [Client]
    public void Select()
    {
        if (!hasAuthority) return;

        onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if (!hasAuthority) return;

        onDeSelected?.Invoke();
    }

    #endregion
}
