using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform targetPosition;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }

    }
}
