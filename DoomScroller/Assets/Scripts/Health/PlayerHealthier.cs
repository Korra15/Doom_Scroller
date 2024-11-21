using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthier : MonoBehaviour
{
    public Sprite fullLife, halfLife, emptyLife;

    Image lifeImage;

    private void Awake()
    {
        lifeImage = GetComponent<Image>();
    }

    public void SetLifeImage(LifeStatus status){
        switch(status)
        {
            case LifeStatus.Empty:
                lifeImage.sprite = emptyLife;
                break;
            case LifeStatus.Half:
                lifeImage.sprite = halfLife;
                break;
            case LifeStatus.Full:
                lifeImage.sprite = fullLife;
                break;
        }
    }
}
public enum LifeStatus{
    Empty = 0,
    Half = 1,
    Full = 2
}