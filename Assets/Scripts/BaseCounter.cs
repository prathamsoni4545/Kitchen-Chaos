using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    public virtual void Interact(Player player)
    {
        Debug.Log("BaseCounter.Interact()");
    }
    public virtual void InteractAlternate(Player player)
    {
        //Debug.Log("BaseCounter.InteractAlternate()");
    }

    // Return the configured follow transform, but guard against missing assignment
    public Transform GetKitchenObjectFollowTransform()
    {
        if (counterTopPoint == null)
        {
            Debug.LogWarning($"BaseCounter ({name}): counterTopPoint not assigned — falling back to this.transform.", this);
            return transform;
        }

        return counterTopPoint;
    }

    // Editor helper: ensure a sensible default in the Inspector (runs in editor)
    private void OnValidate()
    {
        if (counterTopPoint == null)
        {
            counterTopPoint = transform;
        }
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}