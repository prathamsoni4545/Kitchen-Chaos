using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private KitchenObjectSO[] inputObjects;
    [SerializeField] private KitchenObjectSO[] outputObjects;
    [SerializeField] private int maxCut = 3;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (TryHandlePlateInteraction(player)) return;

        // TAKE
        if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            ResetProgress();
            return;
        }

        // PUT (cuttable only)
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            int index = GetCutIndex(player.GetKitchenObject().GetKitchenObjectSO());
            if (index == -1) return;

            player.GetKitchenObject().SetKitchenObjectParent(this);
            cuttingProgress = 0;
            FireProgress(0f, true);
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (!HasKitchenObject()) return;

        int index = GetCutIndex(GetKitchenObject().GetKitchenObjectSO());
        if (index == -1) return;

        cuttingProgress++;
        FireProgress((float)cuttingProgress / maxCut, true);
        OnCut?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);


        if (cuttingProgress >= maxCut)
        {
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(outputObjects[index], this);
            ResetProgress();
        }
    }

    private bool TryHandlePlateInteraction(Player player)
    {
        // Player plate
        if (player.HasKitchenObject() &&
            player.GetKitchenObject().TryGetPlate(out PlateKitchenObject playerPlate) &&
            HasKitchenObject())
        {
            if (playerPlate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().DestroySelf();
                return true;
            }
        }

        // Counter plate
        if (HasKitchenObject() &&
            GetKitchenObject().TryGetPlate(out PlateKitchenObject counterPlate) &&
            player.HasKitchenObject())
        {
            if (counterPlate.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().DestroySelf();
                return true;
            }
        }

        return false;
    }

    private void ResetProgress()
    {
        cuttingProgress = 0;
        FireProgress(0f, false);
    }

    private void FireProgress(float value, bool show)
    {
        OnProgressChanged?.Invoke(this,
            new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = value,
                showProgress = show
            });
    }

    private int GetCutIndex(KitchenObjectSO input)
    {
        for (int i = 0; i < inputObjects.Length; i++)
            if (inputObjects[i] == input) return i;
        return -1;
    }
}
