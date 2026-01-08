using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (TryHandlePlateInteraction(player)) return;

        // PLACE
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
                player.GetKitchenObject().SetKitchenObjectParent(this);
            return;
        }

        // TAKE
        if (!player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
        }
    }

    private bool TryHandlePlateInteraction(Player player)
    {
        // Player has plate
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

        // Counter has plate
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
}
