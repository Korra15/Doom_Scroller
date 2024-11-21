using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro; // For TextMeshPro

public class GravesiteIntro : MonoBehaviour
{
    public bool startIntro = false;
    public GameObject[] lights;
    public AudioSource introAudio;
    public new BoxCollider2D collider;
    public float lightSpeed = 0.35f;
    public TextMeshProUGUI introText; // Reference to the TextMeshProUGUI component
    public float textDisplayDuration = 5f; // Adjustable text display duration

    private static bool introPlayed = false; // Tracks if the intro has been played
    private static float lastPlaybackPosition = 0f; // Stores the last playback position
    private static bool songHasFinished = false; // Indicates if the song has finished

    void Awake()
    {
        //Debug.Log("Awake: Initializing GravesiteIntro component.");
        if (introText != null)
        {
            introText.text = "";
            //Debug.Log("Intro text initialized as empty.");
        }
        else
        {
            //Debug.LogError("Intro text component not found!");
        }

        if (introPlayed && !songHasFinished && introAudio.clip != null)
        {
            introAudio.time = lastPlaybackPosition;
            introAudio.Play();
            Debug.Log($"Resuming song: {introAudio.clip.name} at {lastPlaybackPosition}s.");
            UpdateIntroText($"Now Playing: {introAudio.clip.name}");
            StartFadeOutText(textDisplayDuration);
        }
    }

    void Update()
    {
        if (startIntro)
        {
            StartCoroutine(MoveLights());
            PulseOn();
            introPlayed = true; // Mark the intro as played
            //Debug.Log("Start intro and moving lights.");

            if (introAudio.clip != null) // Ensure there's a clip to play
            {
                UpdateIntroText($"Now Playing: {introAudio.clip.name}");
            }
            else
            {
                UpdateIntroText("Now Playing: Unknown Intro");
            }
        }
        else if (!startIntro)
        {
            PulseOff();
        }

        if (introPlayed && !introAudio.isPlaying && lastPlaybackPosition > 0 && !songHasFinished)
        {
            songHasFinished = true;
            //Debug.Log("Song has finished playing.");
        }
    }

    void UpdateIntroText(string text)
    {
        if (introText != null)
        {
            introText.text = text;
            introText.color = Color.white; // Reset text color to fully opaque
            StartFadeOutText(textDisplayDuration);
            //Debug.Log($"Updated intro text: {text}");
        }
        else
        {
            //Debug.LogError("Failed to update intro text - component missing.");
        }
    }

    IEnumerator MoveLights()
    {
        collider.enabled = false;
        PulseOn();
        foreach (GameObject lightObject in lights)
        {
            Transform lightTransform = lightObject.transform;
            lightTransform.Translate(Time.deltaTime * lightSpeed, 0, 0, Space.World);
        }

        yield return new WaitForSeconds(16f);
        lightSpeed = 2f;
        yield return new WaitForSeconds(1f);
        lightSpeed = 1.5f;
        yield return new WaitForSeconds(2f);
        lightSpeed = 0.8f;
        yield return new WaitForSeconds(10f);

        startIntro = false;
    }

    IEnumerator FadeOutText(float duration)
    {
        Color originalColor = introText.color;
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            introText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }

    void StartFadeOutText(float duration)
    {
        if (introText != null)
        {
            StartCoroutine(FadeOutText(duration));
        }
    }

    void PulseOff()
    {
        foreach (GameObject lightObject in lights)
        {
            Light2D light = lightObject.GetComponent<Light2D>();
            if (light.intensity > 0.1f)
            {
                light.intensity -= 2f * Time.deltaTime;
            }
        }
    }

    void PulseOn()
    {
        foreach (GameObject lightObject in lights)
        {
            Light2D light = lightObject.GetComponent<Light2D>();
            if (light.intensity < 15f)
            {
                light.intensity += 2f * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !introPlayed && !songHasFinished)
        {
            startIntro = true;
            introAudio.Play();
            //Debug.Log("Player triggered the intro.");
        }
    }

    void OnDisable()
    {
        if (introAudio.isPlaying)
        {
            lastPlaybackPosition = introAudio.time;
            //Debug.Log("Saving last playback position.");
        }
        else if (!songHasFinished && introAudio.clip != null)
        {
            lastPlaybackPosition = introAudio.time;
            //Debug.Log("Song was paused, saving current position.");
        }
    }
}
