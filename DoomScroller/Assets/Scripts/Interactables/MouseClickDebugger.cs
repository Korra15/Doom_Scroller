using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            // Log the mouse click position
            Debug.Log("Mouse Clicked at: " + Input.mousePosition);

            // Check if the click is over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                // Create a list to receive all results
                var results = new System.Collections.Generic.List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                // Log each UI element hit by the raycast
                foreach (var result in results)
                {
                    Debug.Log("Clicked on UI Element: " + GetFullHierarchy(result.gameObject));
                }
            }
            else
            {
                // Raycast to check for GameObject interaction
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Clicked on GameObject: " + GetFullHierarchy(hit.collider.gameObject));
                }
                else
                {
                    Debug.Log("Clicked on nothing.");
                }
            }
        }
    }

    // Helper method to get the full hierarchy of a GameObject
    private string GetFullHierarchy(GameObject obj)
    {
        string hierarchy = obj.name;
        Transform currentParent = obj.transform.parent;
        while (currentParent != null)
        {
            hierarchy = currentParent.name + "/" + hierarchy;
            currentParent = currentParent.parent;
        }
        return hierarchy;
    }
}
