using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public EventHandler OnRecipeSpawned;
    public EventHandler OnRecipeCompleted;
    public EventHandler OnRecipeSuccess;
    public EventHandler OnRecipeFailed;
    



    public static DeliveryManager Instance { get; private set; }


    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingrecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;


    private void Awake()
    {
        Instance = this;
        waitingrecipeSOList = new List<RecipeSO>();
    }



    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (KitchenGameManager.Instance.IsGamePlaying() && waitingrecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                
                waitingrecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingrecipeSOList.Count; ++i)
        {
            RecipeSO waitingrecipeSO = waitingrecipeSOList[i];

            if (waitingrecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetAddedKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipekitchenObjectSO in waitingrecipeSO.kitchenObjectSOList)
                {
                    bool ingreientFound = false;
                    foreach (KitchenObjectSO platekitchenObjectSO in plateKitchenObject.GetAddedKitchenObjectSOList())
                    {
                        if (platekitchenObjectSO == recipekitchenObjectSO)
                        {
                            ingreientFound = true;
                            break;

                        }
                    }
                    if (!ingreientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    successfulRecipesAmount++;

                    waitingrecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }

            }
        }
        Debug.Log("DeliveryManager: delivery not correct");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingrecipeSOList;
    }
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
