using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] LayerMask layerMask = new LayerMask();
    Camera mainCamera;

    List<Unit> selectedUnits = new List<Unit>();

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            foreach (Unit selectedUnit in selectedUnits)
            {
                selectedUnit.Deselect();
            }

            selectedUnits.Clear();

        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
    }

    private void ClearSelectionArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        // raycasting to mouse position and checking with layermask
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;
        // checking whether it has a Unit component or not
        if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) return;
        // checking if we have authority on that unit as this is an online game
        if (!unit.hasAuthority) return;

        selectedUnits.Add(unit);

        foreach (Unit selectedUnit in selectedUnits)
        {
            selectedUnit.Select();
        }
    }
}
