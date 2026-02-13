using System;
using UnityEngine;

public class GlobalvariableContainer : MonoBehaviour
{
    public static GlobalvariableContainer Instance;

    public Sprite[] ballIcons, ballGreyIcons;
    public Sprite[] powerupIcons;
    public Sprite allIncomeIcon, ringHealthReducedIcon;
    public string[] ballNames;
    
    public Sprite enableSprite, disableSprite;

    private void Awake()
    {
        Instance = this;
    }

    public string GetBallName(int ballIndex)
    {
        return ballNames[ballIndex];
    }
}
