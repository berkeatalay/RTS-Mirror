using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> myUnits = new List<Unit>();

    public List<Unit> GetMyUnits()
    {
        return myUnits;
    }

    #region  Server

    public override void OnStartServer()
    {
        Unit.serverOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.serverOnUnitDespawned += ServerHandleUnitDespawned;
    }

    public override void OnStopServer()
    {
        Unit.serverOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.serverOnUnitDespawned -= ServerHandleUnitDespawned;
    }

    private void ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myUnits.Add(unit);
    }

    private void ServerHandleUnitDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

        myUnits.Remove(unit);
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly) return;

        Unit.authorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.authorityOnUnitDespawned += AuthorityHandleUnitDespawned;

    }

    public override void OnStopClient()
    {
        if (!isClientOnly) return;

        Unit.authorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
        Unit.authorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
    }

    private void AuthorityHandleUnitSpawned(Unit unit)
    {
        if (!hasAuthority) return;

        myUnits.Add(unit);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        if (!hasAuthority) return;

        myUnits.Remove(unit);

    }



    #endregion
}