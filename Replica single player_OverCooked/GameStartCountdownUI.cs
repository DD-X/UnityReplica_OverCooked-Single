using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start() {
        GameHandler.Instance.OnstateChanged += GameHandler_OnstateChanged;
        Hide();
    }

    private void GameHandler_OnstateChanged(object sender, System.EventArgs e) {
        if (GameHandler.Instance.IsCountdownToActive()) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Update() {
        countdownText.text = Mathf.Ceil(GameHandler.Instance.GetCountdownToStartTimer()).ToString();
        //countdownText.text = GameHandler.Instance.GetCountdownToStartTimer().ToString("#,##");
    }

    private void Show() {
        gameObject.SetActive(true);//? which one
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
