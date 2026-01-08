using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() => kitchenObjectSO;

    public void SetKitchenObjectParent(IKitchenObjectParent parent)
    {
        kitchenObjectParent?.ClearKitchenObject();
        kitchenObjectParent = parent;

        parent.SetKitchenObject(this);
        transform.parent = parent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plate)
    {
        plate = this as PlateKitchenObject;
        return plate != null;
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO so, IKitchenObjectParent parent)
    {
        Transform t = Instantiate(so.prefab);
        KitchenObject obj = t.GetComponent<KitchenObject>();
        obj.SetKitchenObjectParent(parent);
        return obj;
    }
}
