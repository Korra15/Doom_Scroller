using UnityEngine;

public class controlsTutorial : MonoBehaviour
{
    public GameObject canvas;

    public bool on_off = false;

    void Update()
    {
        if(on_off){
            canvas.SetActive(true);
        }else{
            canvas.SetActive(false);
        }
    }
}
