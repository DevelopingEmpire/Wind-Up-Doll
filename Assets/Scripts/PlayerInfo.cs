using UnityEngine;
using Fusion;
using TMPro;
using JetBrains.Annotations;
using UnityEditor.Build;

public class PlayerInfo : NetworkBehaviour
{
    public MeshRenderer meshRenderer;
    public TMP_Text nameDisplayTMP;

    [Networked]
    public Color NetworkedColor { get; set; }

    [Networked]
    public NetworkString<_64> PlayerName { get; set; }
    [Networked]
    public NetworkBool EisPressed { get; set; }

    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        if (HasStateAuthority)
        {
            PlayerName = Runner.GetComponent<PlayerSpawner>().playerName;
        }

        nameDisplayTMP.text = PlayerName.ToString();
        transform.gameObject.name = nameDisplayTMP.text;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            NetworkedColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            EisPressed = true;
        }

        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(NetworkedColor):
                    meshRenderer.material.color = NetworkedColor;
                    break;
                case nameof(PlayerName):
                    nameDisplayTMP.text = PlayerName.ToString(); // NetworkString to String
                    transform.gameObject.name = nameDisplayTMP.text;
                    break;
                case nameof(EisPressed):
                    Debug.Log("EIsPressed = " + EisPressed);
                    //EisPressed = false;
                    //Debug.Log("EIsPressed = " + EisPressed);
                    break;
            }
        }
    }

}