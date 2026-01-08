using UnityEngine;

[CreateAssetMenu(menuName = "Kitchen/KitchenObject", fileName = "KitchenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    // Prefab used by counters/objects. Keep as Transform if other code expects a Transform,
    // otherwise change to GameObject and update call sites.
    public Transform prefab;
    public Sprite sprite;

    // Exposed so designers can give a display name in the inspector
    [SerializeField] private string objectName;

    // Read-only accessor that falls back to the asset name if not provided
    public string ObjectName => string.IsNullOrEmpty(objectName) ? name : objectName;




}
