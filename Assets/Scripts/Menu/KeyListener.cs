using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyListener : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public PlayerHolder playerHolder;

    void Start()
    {
        playerHolder.Player.GetComponent<PlayerController>().SubscribeToOnKeyChange(OnChange);
    }

    private void OnChange(Object[] obj)
    {
        PlayerController pc = obj[0] as PlayerController;
        text.text = pc.NumKeys.ToString();
    }
}
