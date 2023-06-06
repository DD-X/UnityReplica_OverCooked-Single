using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagementUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake() {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        DeliveryManager.Instance.OnrecipeSpawned += DeliveryManager_OnrecipeSpawned;
        DeliveryManager.Instance.OnrecipeCompeleted += DeliveryManager_OnrecipeCompeleted;
        UpdateVisual();//The previous one did not show up
    }

    private void DeliveryManager_OnrecipeCompeleted(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void DeliveryManager_OnrecipeSpawned(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach(Transform child in container) {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList()) {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
