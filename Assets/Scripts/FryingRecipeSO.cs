using UnityEngine;

[CreateAssetMenu(fileName = "NewFryingRecipe", menuName = "Kitchen/FryingRecipe")]
public class FryingRecipeSO : ScriptableObject
{
    [Header("Input -> Fried -> Burned")]
    public KitchenObjectSO input;         // raw
    public KitchenObjectSO output;        // fried
    public KitchenObjectSO burnedOutput;  // burned (after burningTimeMax)

    [Header("Timers (seconds)")]
    public float fryingTimeMax = 4f;      // time to become fried
    public float burningTimeMax = 5f;     // time (after fried) to become burned
}
