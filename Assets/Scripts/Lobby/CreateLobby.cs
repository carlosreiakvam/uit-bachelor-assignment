using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobby : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameInput;
    [SerializeField] TextMeshProUGUI lobbyNameInput;
    [SerializeField] GameObject scopeButtonGO;
    [SerializeField] GameObject createButtonGO;
    [SerializeField] GameObject backButtonGO;
    [SerializeField] GameObject lobbyPreGameGO;
    [SerializeField] GameObject lobbyManagerGO;
    [SerializeField] GameObject menuManagerGO;
    LobbyManager lobbyManager;
    MenuManager menuManager;
    bool isPrivate = true;

    private void Start()
    {
        menuManager = menuManagerGO.GetComponent<MenuManager>();
        lobbyManager = lobbyManagerGO.GetComponent<LobbyManager>();


        Button scopeButton = scopeButtonGO.GetComponent<Button>();
        Button createButton = createButtonGO.GetComponent<Button>();
        Button backButton = backButtonGO.GetComponent<Button>();
        TextMeshProUGUI scopeText = scopeButton.GetComponentInChildren<TextMeshProUGUI>();

        backButton.onClick.AddListener(() =>
        {
            menuManager.OpenPage(MenuEnums.LobbyStart);
        });

        createButton.onClick.AddListener(async () =>
        {
            if (lobbyNameInput.text.Length <= 1 || playerNameInput.text.Length <= 1)
            { menuManager.OpenAlert("Fill out all fields"); }
            else if (playerNameInput.text.Length > 15)
            { menuManager.OpenAlert("Enter a name less than 15 characters"); }
            else if (lobbyNameInput.text.Length > 15)
            { menuManager.OpenAlert("Enter a lobby name less than 15 characters"); }
            else
            { await lobbyManager.CreateLobby(lobbyNameInput.text, isPrivate, playerNameInput.text); }

        });

        scopeButton.onClick.AddListener(() =>
        {
            isPrivate = !isPrivate;
            scopeText.text = isPrivate ? "Private" : "Public";
        });
    }
}
