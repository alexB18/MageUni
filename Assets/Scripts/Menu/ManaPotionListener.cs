using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotionListener : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public PlayerHolder playerHolder;

    void Start()
    {
        playerHolder.Player.GetComponent<PlayerController>().SubscribeToOnManaPotionChange(OnChange);
    }

    private void OnChange(Object[] obj)
    {
        PlayerController pc = obj[0] as PlayerController;
        text.text = pc.ManaPotionCount.ToString();
    }
}
