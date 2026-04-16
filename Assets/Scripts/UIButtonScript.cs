using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalSize;
    private Vector3 hoveredSize;
    private bool isHoveredOn;

    private LevelManager levelManager;
    private Button button;

    void Start()
    {
        // Store the original size of the element
        originalSize = transform.localScale;
        hoveredSize = originalSize * 1.5f;

        levelManager = FindAnyObjectByType<LevelManager>();
        button = GetComponent<Button>();
    }

    private void Update()
    {
        // Detect controller selection
        if (EventSystem.current.currentSelectedGameObject == gameObject && !isHoveredOn)
        {
            OnControllerSelect();
        }
        else if (EventSystem.current.currentSelectedGameObject != gameObject && isHoveredOn)
        {
            OnControllerDeselect();
        }

        // Button selection with Submit (A on Xbox, X on PlayStation)
        if (isHoveredOn && (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")))
        {
            MenuAudioManager.Instance.PlaySound("Selected");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            levelManager.MainMenu();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Application.Quit();
        }
    }

    // Mouse hover event
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnControllerSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnControllerDeselect();
    }

    private void OnControllerSelect()
    {
        transform.localScale = hoveredSize;
        MenuAudioManager.Instance.PlaySound("Hover");
        isHoveredOn = true;
    }

    private void OnControllerDeselect()
    {
        transform.localScale = originalSize;
        isHoveredOn = false;
    }

    public void Continue() { }

    public void NewGame()
    {
        levelManager.NewGame();
    }

    public void Options()
    {
        levelManager.Options();
    }

    public void Statistics() { }

    public void Quit()
    {
        levelManager.Quit();
    }

    public void Credits() { }
}
