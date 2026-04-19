using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class Prologue : MonoBehaviour
{
    [SerializeField] private string panelNamePrefix = "panel";
    [SerializeField] private int panelCount = 7;
    [SerializeField] private float secondsPerPanel = 15f;
    [SerializeField] private int nextSceneIndex = 2;

    [SerializeField] private GameObject[] panels;
    private int currentPanelIndex = -1;
    private float panelTimer;
    private bool isPrologueActive;
    private bool hasAnyPanels;

    void Start()
    {
        InitializePanels();
        DestroyUnrelatedCanvas("UI Canvas");
        DestroyUnrelatedCanvas("UICanvas");
        AdvancePanel();
    }

    void Update()
    {
        if (!isPrologueActive)
        {
            return;
        }

        panelTimer += Time.deltaTime;

        bool shouldAdvance = panelTimer >= secondsPerPanel;

        if (!shouldAdvance && Input.GetMouseButtonDown(0))
        {
            shouldAdvance = true;
        }

#if ENABLE_INPUT_SYSTEM
        if (!shouldAdvance && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            shouldAdvance = true;
        }
#endif

        if (shouldAdvance)
        {
            AdvancePanel();
        }
    }

    private void InitializePanels()
    {
        hasAnyPanels = false;

        if (panels == null || panels.Length == 0)
        {
            CachePanelsByName();
            return;
        }

        for (int index = 0; index < panels.Length; index++)
        {
            if (panels[index] == null)
            {
                continue;
            }

            panels[index].SetActive(false);
            hasAnyPanels = true;
        }
    }

    private void CachePanelsByName()
    {
        panels = new GameObject[panelCount];

        for (int index = 0; index < panelCount; index++)
        {
            panels[index] = FindSceneObjectByName($"{panelNamePrefix}{index + 1}");

            if (panels[index] == null)
            {
                panels[index] = FindSceneObjectByName($"Panel{index + 1}");
            }

            if (panels[index] != null)
            {
                panels[index].SetActive(false);
                hasAnyPanels = true;
            }
        }
    }

    private void AdvancePanel()
    {
        if (!hasAnyPanels)
        {
            isPrologueActive = false;
            Debug.LogError("Prologue could not find any comic panels. Verify panel1 to panel7 exist in the active scene.");
            return;
        }

        if (currentPanelIndex >= 0 && currentPanelIndex < panels.Length && panels[currentPanelIndex] != null)
        {
            panels[currentPanelIndex].SetActive(false);
        }

        currentPanelIndex++;

        while (currentPanelIndex < panels.Length && panels[currentPanelIndex] == null)
        {
            currentPanelIndex++;
        }

        if (currentPanelIndex >= panels.Length)
        {
            LoadNextScene();
            return;
        }

        panels[currentPanelIndex].SetActive(true);
        panelTimer = 0f;
        isPrologueActive = true;
    }

    private void LoadNextScene()
    {
        isPrologueActive = false;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private GameObject FindSceneObjectByName(string objectName)
    {
        Transform[] sceneTransforms = Resources.FindObjectsOfTypeAll<Transform>();

        for (int index = 0; index < sceneTransforms.Length; index++)
        {
            Transform candidate = sceneTransforms[index];

            if (candidate.hideFlags != HideFlags.None)
            {
                continue;
            }

            if (candidate.gameObject.scene != gameObject.scene)
            {
                continue;
            }

            if (candidate.name == objectName)
            {
                return candidate.gameObject;
            }
        }

        return null;
    }

    private void DestroyUnrelatedCanvas(string canvasName)
    {
        GameObject canvasObject = FindSceneObjectByName(canvasName);

        if (canvasObject == null)
        {
            return;
        }

        for (int index = 0; index < panels.Length; index++)
        {
            if (panels[index] != null && panels[index].transform.IsChildOf(canvasObject.transform))
            {
                return;
            }
        }

        Destroy(canvasObject);
    }
}
