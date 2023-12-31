using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyQuickJoin : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Sprite spinnerSprite;
    [SerializeField] TextMeshProUGUI header;
    [SerializeField] TextMeshProUGUI playerNameInput;
    [SerializeField] GameObject goButtonGO;
    [SerializeField] GameObject backButtonGO;
    GameObject spinner;


    private void Start()
    {
        MenuManager.Singleton.OnLobbyJoinOpened += OnActivated;

        spinner = CreateSpinner();

        Button goButton = goButtonGO.GetComponent<Button>();
        Button backButton = backButtonGO.GetComponent<Button>();

        goButton.onClick.AddListener(async () =>
        {
            if (!ValidateInput()) return;
            spinner.SetActive(true);
            StartCoroutine(RotateSpinner(spinner));
            bool isLobbyFound = await LobbyManager.Singleton.QuickJoinLobby(playerNameInput.text);
            spinner.SetActive(false);

            if (!isLobbyFound) { MenuManager.Singleton.OpenAlert("No open lobbies found"); return; }
            MenuManager.Singleton.OpenLobbyRoom(this, EventArgs.Empty);
        });

        backButton.onClick.AddListener(() => { MenuManager.Singleton.OpenPage(MenuEnums.LobbyMenu); });


    }

    private bool ValidateInput()
    {
        bool isInputValid = false;
        if (playerNameInput.text.Length <= 1 || playerNameInput.text.Length <= 1)
        {
            MenuManager.Singleton.OpenAlert("Fill out all fields");
        }
        else if (playerNameInput.text.Length > 15)
        { MenuManager.Singleton.OpenAlert("Enter a name less than 15 characters"); }
        else { isInputValid = true; }
        return isInputValid;
    }

    void OnActivated(object sender, EventArgs e)
    {
        header.text = "Join Lobby";

    }


    IEnumerator RotateSpinner(GameObject spinner)
    {
        while (spinner.activeSelf)
        {
            // Rotate the spinner by 1 degree per frame
            spinner.transform.Rotate(Vector3.forward);
            yield return null;
        }
    }

    public GameObject CreateSpinner()
    {
        GameObject spinner = new GameObject("Spinner");
        Image spinnerImage = spinner.AddComponent<Image>();
        spinnerImage.sprite = spinnerSprite;
        spinner.transform.SetParent(canvas.transform, false);
        spinner.SetActive(false);

        return spinner;
    }

}
