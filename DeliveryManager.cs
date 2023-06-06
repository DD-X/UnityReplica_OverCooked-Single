using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnrecipeSpawned;
    public event EventHandler OnrecipeCompeleted;
    public event EventHandler OnrecipeSuccess;
    public event EventHandler OnrecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingrecipeSOList;//Customer are waiting for
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipeAmount;


    private void Awake() {
        Instance = this;

        waitingrecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if(waitingrecipeSOList.Count < waitingRecipeMax) { //Count[0,3] Max=4
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                //Debug.Log(waitingRecipeSO.recipeName);
                waitingrecipeSOList.Add(waitingRecipeSO);

                OnrecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for(int i=0; i < waitingrecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingrecipeSOList[i];

            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                //Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    //Cycling through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        //Cycling through all ingredients on the Plate
                        if(plateKitchenObjectSO == recipeKitchenObjectSO) {
                            //Ingredient matches!
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        //This recipe ingredient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe) {
                    //Player deliver the correct recipe
                    //Debug.Log("Player delivered the correct recipe!");

                    successfulRecipeAmount++;

                    waitingrecipeSOList.RemoveAt(i);

                    OnrecipeCompeleted?.Invoke(this, EventArgs.Empty);                 
                    OnrecipeSuccess?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }
        }
        //No matches found!
        //Player did not deliver a correct recipe
        //Debug.Log("Player did not deliver a correct recipe");

        OnrecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingrecipeSOList;
    }

    public int GetSuccessfulRecipeAmount() {
        return successfulRecipeAmount;
    }

}
