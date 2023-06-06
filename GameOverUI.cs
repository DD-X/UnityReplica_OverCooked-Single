using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;

    private void Start() {
        GameHandler.Instance.OnstateChanged += GameHandler_OnstateChanged;
        Hide();
    }

    private void GameHandler_OnstateChanged(object sender, System.EventArgs e) {
        if (GameHandler.Instance.IsGameOver()) {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipeAmount().ToString();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);//? which one
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
