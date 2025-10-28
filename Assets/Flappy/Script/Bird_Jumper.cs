using UnityEngine;
using UnityEngine.UI;

public class BirdJumper : MonoBehaviour
{
    [SerializeField] private Button button; // Assign in Inspector or let script find it

    void Start()
    {
        Debug.Log("Start called.");

        // If button is not assigned in Inspector, try to get Button component on same GameObject
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        // If button is found, add click listener
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Button is not assigned or found!");
        }
    }

    private void OnButtonClicked()
    {
        Debug.Log("Button was clicked!");
    }
}
