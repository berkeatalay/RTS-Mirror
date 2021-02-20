using System;
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

    public static event Action<Unit> serverOnUnitSpawned;
    public static event Action<Unit> serverOnUnitDespawned;

    public static event Action<Unit> authorityOnUnitSpawned;
    public static event Action<Unit> authorityOnUnitDespawned;

    public UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }
    #region Server
    public override void OnStartServer()
    {
        serverOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        serverOnUnitDespawned?.Invoke(this);
    }
    #endregion

    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly) return;
        if (!hasAuthority) return;

        authorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly) return;
        if (!hasAuthority) return;

        authorityOnUnitDespawned?.Invoke(this);
    }

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
