using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCamera : MonoBehaviour
{
    private GameManager11 _gameManager11;

    private void Start()
    {
        _gameManager11 = GameManager11.Instance;
    }
}
