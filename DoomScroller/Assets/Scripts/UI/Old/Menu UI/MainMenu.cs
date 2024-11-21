using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject saveManager;
    public GameObject loadMenu;
    public GameObject mainMenu;
    public GameObject volumeSetting;
    public GameObject brightnessSetting;
    public GameObject languageSetting;
    public GameObject backGround;
    public GameObject otherBackGround;

    public EventSystem eventSystem;

    public GameObject MasterVolumeSlider;
    public GameObject BrightnessSlider;
    public GameObject EnglishButton;
    public GameObject LoadButton;

    public GameObject Load1;
    public GameObject Load2;
    public GameObject Load3;

    public GameObject StartNew1;
    public GameObject StartNew2;
    public GameObject StartNew3;

    public Canvas studioNameCanvas; // Canvas for "Studio Name Presents"
    public Animator studioNameAnimator; // Animator for the "Studio Name Presents"

    public GameObject mainCamera; //declare the mainCamera variable to control it

    // This is called when the scene is loaded
    void Start()
{

    EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

    // Disable the main camera at the start
    if (mainCamera != null)
        mainCamera.SetActive(false);

    // Ensure the mainMenu is disabled
    if (mainMenu != null)
        mainMenu.SetActive(false);

    // Ensure the studio name canvas is active to play the animation
    if (studioNameCanvas != null)
        studioNameCanvas.gameObject.SetActive(true);

    // Start the "Studio Name Presents" animation coroutine on an active GameObject
    // You can start this coroutine on this script's GameObject as it should be active
    StartCoroutine(TransitionAfterAnimation());
}

private IEnumerator TransitionAfterAnimation()
{
    // Make sure the animator is ready and play the animation
    if (studioNameAnimator != null && studioNameAnimator.gameObject.activeInHierarchy)
    {
        // If the animation clip is attached to an inactive object, it won't play
        // so we make sure the studioNameCanvas is active
        studioNameAnimator.Play("StudioNameFade");

        // Wait for the animation to finish
        yield return new WaitUntil(() => studioNameAnimator.GetCurrentAnimatorStateInfo(0).IsName("StudioNameFade") &&
                                        studioNameAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
    }

    // After the animation, deactivate the studio name canvas
    if (studioNameCanvas != null)
        studioNameCanvas.gameObject.SetActive(false);

    // Re-enable the main camera after the animation
    if (mainCamera != null)
        mainCamera.SetActive(true);

    // Now activate the main menu
    if (mainMenu != null)
        mainMenu.SetActive(true);

    // And the background
    if (backGround != null)
        backGround.SetActive(true);
}

    public void DisplayLoadMenu() 
    {
        SaveManager save = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        if(save.FileExists(4)){
            save.InstantLoad();
        }else{
            mainMenu.SetActive(false);
            loadMenu.SetActive(true);
            backGround.SetActive(false);
            otherBackGround.SetActive(true);
            volumeSetting.SetActive(false);
            brightnessSetting.SetActive(false);
            languageSetting.SetActive(false);



            if(LoadButton == null){
                LoadButton = GameObject.Find("LoadMenuBack");
            }

            eventSystem.SetSelectedGameObject(LoadButton);
        }
    }

    public void DisplayLoadMenuDefault() 
    {
        mainMenu.SetActive(false);
        loadMenu.SetActive(true);
        backGround.SetActive(false);
        otherBackGround.SetActive(true);
        volumeSetting.SetActive(false);
        brightnessSetting.SetActive(false);
        languageSetting.SetActive(false);



        if(LoadButton == null){
            LoadButton = GameObject.Find("LoadMenuBack");
        }

        eventSystem.SetSelectedGameObject(LoadButton); 
    }

    public void DisplayMainMenu()
    {
        mainMenu.SetActive(true);
        loadMenu.SetActive(false);
        backGround.SetActive(true);
        otherBackGround.SetActive(false);
        volumeSetting.SetActive(false);
        brightnessSetting.SetActive(false);
        languageSetting.SetActive(false);

        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        GameObject PlayButton = GameObject.Find("PlayButton");
        eventSystem.SetSelectedGameObject(PlayButton);

    }

    public void DisplayVolumeSetting()
    {
        mainMenu.SetActive(false);
        loadMenu.SetActive(false);
        backGround.SetActive(false);
        otherBackGround.SetActive(false);
        volumeSetting.SetActive(true);
        brightnessSetting.SetActive(false);
        languageSetting.SetActive(false);
        eventSystem.SetSelectedGameObject(MasterVolumeSlider);
    }

    public void DisplayBrightnessSetting()
    {
        mainMenu.SetActive(false);
        loadMenu.SetActive(false);
        backGround.SetActive(false);
        otherBackGround.SetActive(false);
        volumeSetting.SetActive(false);
        brightnessSetting.SetActive(true);
        languageSetting.SetActive(false);
        eventSystem.SetSelectedGameObject(BrightnessSlider);
    }

    public void DisplayLanguageSetting()
    {
        mainMenu.SetActive(false);
        loadMenu.SetActive(false);
        backGround.SetActive(false);
        otherBackGround.SetActive(false);
        volumeSetting.SetActive(false);
        brightnessSetting.SetActive(false);
        languageSetting.SetActive(true);
        eventSystem.SetSelectedGameObject(EnglishButton);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void SetStartNew1(){
        eventSystem.SetSelectedGameObject(Load1);
    }

    public void SetDelete1(){
        eventSystem.SetSelectedGameObject(StartNew1);
    }

    public void SetStartNew2(){
        eventSystem.SetSelectedGameObject(Load2);
    }

    public void SetDelete2(){
        eventSystem.SetSelectedGameObject(StartNew2);
    }

    public void SetStartNew3(){
        eventSystem.SetSelectedGameObject(Load3);
    }

    public void SetDelete3(){
        eventSystem.SetSelectedGameObject(StartNew3);
    }
}
