using System;
using UnityEngine;

public class GlobalvariableContainer : MonoBehaviour
{
    public static GlobalvariableContainer Instance;

    public Sprite[] ballIcons;
    public Sprite[] powerupIcons;
    public Sprite allIncomeIcon, ringHealthReducedIcon;

    private void Awake()
    {
        Instance = this;
    }
    
    
}
