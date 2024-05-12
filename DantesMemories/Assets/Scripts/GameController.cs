using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float wallet;

    public static GameController instance;

    public Text money;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        money.text = "x" + wallet.ToString();
    }

    public void Money(float cash)
    {
        wallet += cash;
        money.text = "x"+wallet.ToString();
    }
}
