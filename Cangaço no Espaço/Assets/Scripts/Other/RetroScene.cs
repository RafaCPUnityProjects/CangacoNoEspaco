using UnityEngine;
using System.Collections;

[System.Serializable]
public class RetroScene {

    public string Name;

    public enum RetroSceneType
    {
        Pacman = 1,
        Donkey = 2,
        Duck = 3,
        Galaga = 4
    }

    public RetroSceneType sceneTheme;

    public int number;
    
    public bool boss;

}
