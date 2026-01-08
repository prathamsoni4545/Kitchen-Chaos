using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeSO", menuName = "ScriptableObjects/RecipeSO", order = 1)]
public class RecipeSO : ScriptableObject
{
   public List<KitchenObjectSO> kitchenObjectSOList;
    public string recipeName;
}
