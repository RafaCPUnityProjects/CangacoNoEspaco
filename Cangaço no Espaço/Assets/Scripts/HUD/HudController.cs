using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{

    public Text coins;
    public Text tries;

    public GameObject[] bits;
    public Image[] shields;
    public Image[] weaponsSlot1;
    //public Image[] weaponsSlot2;
    //public Image[] weaponsSlot3;
    //public Image[] weaponsSlot4;

    Color transparent = new Color(0, 0, 0, 0);

    const int MAXCOINS = 999999;

    void Update()
    {
        UpdateBits(GameSave.totalBitsCollected);
        UpdateHealth(NimHealth.instance.lives);
        UpdateCoins(GameSave.instance.coins);
        UpdateWeapon(GameSave.nimWeapon);
        tries.text = GameSave.instance.tries.ToString();
    }

    void UpdateBits(int currentBits)
    {
        for (int i = 0; i < bits.Length; i++)
        {
            if (i > currentBits - 1)
                bits[i].SetActive(false);
            else
                bits[i].SetActive(true);
        }
    }

    void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < shields.Length; i++)
        {
            if (i > currentHealth - 1)
                shields[i].color = transparent;
            else
                shields[i].color = Color.white;
        }
    }

    void UpdateCoins(int currentCoins)
    {
        coins.text = Mathf.Clamp(currentCoins, 0, MAXCOINS).ToString();
    }

    void UpdateWeapon(CurrentWeapon cw)
    {
        for (int i = 0; i < weaponsSlot1.Length; i++)
        {
            weaponsSlot1[i].enabled = false;
        }

        if (cw == CurrentWeapon.None)
            weaponsSlot1[0].enabled = true;
        if (cw == CurrentWeapon.WatermelonGun)
            weaponsSlot1[1].enabled = true;
        if (cw == CurrentWeapon.PineappleGun)
            weaponsSlot1[2].enabled = true;
        if (cw == CurrentWeapon.BananaGun)
            weaponsSlot1[3].enabled = true;
    }
}
