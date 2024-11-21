using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Canvas[] canvases; // Assign your canvases in the inspector
    private int currentCanvasIndex = 0;

    private void Start()
    {
        // Initially, set only the first canvas to be active
        foreach (Canvas canvas in canvases)
        {
            canvas.gameObject.SetActive(false);
        }
        if (canvases.Length > 0)
        {
            canvases[0].gameObject.SetActive(true);
        }
    }

    public void ShowNextCanvas()
    {
        // Disable current canvas
        canvases[currentCanvasIndex].gameObject.SetActive(false);

        // Increment the index to the next canvas
        currentCanvasIndex = (currentCanvasIndex + 1) % canvases.Length;

        // Enable the next canvas
        canvases[currentCanvasIndex].gameObject.SetActive(true);
    }
}
