using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State { Idle, Frying, Fried, Burned }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;

    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO recipe;
    private State state = State.Idle;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    private void Update()
    {
        if (!HasKitchenObject() || recipe == null) return;

        if (state == State.Frying)
        {
            fryingTimer += Time.deltaTime;
            FireProgress(fryingTimer / recipe.fryingTimeMax, true);

            if (fryingTimer >= recipe.fryingTimeMax)
            {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(recipe.output, this);
                state = State.Fried;
                burningTimer = 0f;
                FireStateChanged();
            }
        }
        else if (state == State.Fried)
        {
            burningTimer += Time.deltaTime;
            FireProgress(burningTimer / recipe.burningTimeMax, true);

            if (burningTimer >= recipe.burningTimeMax)
            {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(recipe.burnedOutput, this);
                ResetState();
            }
        }
    }

    public override void Interact(Player player)
    {
        if (TryHandlePlateInteraction(player)) return;

        // TAKE
        if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            ResetState();
            return;
        }

        // PUT (fryable only)
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            recipe = GetRecipe(player.GetKitchenObject().GetKitchenObjectSO());
            if (recipe == null) return;

            player.GetKitchenObject().SetKitchenObjectParent(this);
            fryingTimer = 0f;
            state = State.Frying;
            FireProgress(0f, true);
            FireStateChanged();
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
                ResetState();
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

    private void ResetState()
    {
        fryingTimer = 0f;
        burningTimer = 0f;
        recipe = null;
        state = State.Idle;
        FireProgress(0f, false);
        FireStateChanged();
    }

    private FryingRecipeSO GetRecipe(KitchenObjectSO input)
    {
        foreach (var r in fryingRecipeSOArray)
            if (r.input == input) return r;
        return null;
    }

    private void FireProgress(float value, bool show)
    {
        OnProgressChanged?.Invoke(this,
            new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = Mathf.Clamp01(value),
                showProgress = show
            });
    }

    private void FireStateChanged()
    {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
