using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using Ro.FusionBites;
using System;
using UnityEditor;

public class PlayerStats : NetworkBehaviour
{
    [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> PlayerName { get; set; }

    [Networked(OnChanged = nameof(UpdateHat))] public int hatIndex { get; set; }

    [SerializeField] TextMeshPro playerNameLabel;

    public static PlayerStats instance;

    private GameObject currentHat = null;

    [SerializeField] Transform playerHead;
    
    private void Start()
    {
        if (this.HasStateAuthority)
        {
            PlayerName = FusionConnection.instance._playerName;
            if (instance == null) { instance = this; } 
        }
        
    }

    // protected static �� ����ؾ� ��. ������ ����.
    protected static void UpdatePlayerName(Changed<PlayerStats> changed) // need change class
    {
        changed.Behaviour.playerNameLabel.text = changed.Behaviour.PlayerName.ToString();
    }

    protected static void UpdateHat(Changed<PlayerStats> changed) // need change class
    {
        int _hatIndex = changed.Behaviour.hatIndex;
        GameObject _currentHat = changed.Behaviour.currentHat;
        GameObject hat = Hats.hats[_hatIndex];

        if(_currentHat != null)
        {
            Destroy(_currentHat);
        }
        GameObject newHat = GameObject.Instantiate(hat); // Spawn �� �ʿ� ����. Instantiate �ص� ��� �÷��̾�鿡�� �����ȴ�.
        
        newHat.transform.parent = changed.Behaviour.playerHead;
        newHat.transform.localPosition = Vector3.zero;
        newHat.transform.localEulerAngles = Vector3.zero;
        newHat.GetComponent<Collider>().enabled = false;

        changed.Behaviour.currentHat = newHat;
    }

}
