using UnityEngine;
using Fusion;
using TMPro;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    public string playerName = null;
    public GameObject uiCanvas;
    public TMP_InputField nameInputField;

    public void PlayerJoined(PlayerRef player)
    {
        if(player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
        }
    }

    public void SaveNameHideCanvas()   //using Btn;
    {
        playerName = nameInputField.text;
        uiCanvas.SetActive(false);
    }
}
