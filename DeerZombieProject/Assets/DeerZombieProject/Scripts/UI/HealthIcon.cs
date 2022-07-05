using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIcon : MonoBehaviour
{
    [SerializeField]
    private Sprite emptyHeartSprite;
    [SerializeField]
    private Sprite halfHeartSprite;
    [SerializeField]
    private Sprite fullHeartSprite;
    [SerializeField]
    private Image heartImage;

    public void UpdateSprite(int healthAmount)
    {
        if(healthAmount <= 0)
        {
            heartImage.sprite = emptyHeartSprite;
            return;
        }

        if(healthAmount == 1)
        {
            heartImage.sprite = halfHeartSprite;
            return;
        }

        heartImage.sprite = fullHeartSprite;
    }
}
