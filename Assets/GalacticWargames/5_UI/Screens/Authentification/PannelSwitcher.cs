using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public List<Transform> panels;

    public Transform leftSlot;
    public Transform centerSlot;
    public Transform rightSlot;

    public float moveDuration = 0.3f;

    private int currentIndex = 0;
    private bool isMoving = false;

    void Start()
    {
        UpdatePanelsInstant();
    }

    public void Next()
    {
        if (isMoving) return;

        currentIndex = (currentIndex + 1) % panels.Count;
        StartCoroutine(AnimatePanels());
    }

    public void Previous()
    {
        if (isMoving) return;

        currentIndex = (currentIndex - 1 + panels.Count) % panels.Count;
        StartCoroutine(AnimatePanels());
    }

    void UpdatePanelsInstant()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            int relativeIndex = GetRelativeIndex(i);

            if (relativeIndex == 0)
                panels[i].position = centerSlot.position;
            else if (relativeIndex == 1)
                panels[i].position = rightSlot.position;
            else
                panels[i].position = leftSlot.position;

            SetSorting(panels[i], relativeIndex);
        }
    }

    IEnumerator AnimatePanels()
    {
        isMoving = true;

        float time = 0f;

        Dictionary<Transform, Vector3> startPos = new();
        Dictionary<Transform, Vector3> targetPos = new();

        foreach (var panel in panels)
        {
            startPos[panel] = panel.position;
        }

        for (int i = 0; i < panels.Count; i++)
        {
            int relativeIndex = GetRelativeIndex(i);

            if (relativeIndex == 0)
                targetPos[panels[i]] = centerSlot.position;
            else if (relativeIndex == 1)
                targetPos[panels[i]] = rightSlot.position;
            else
                targetPos[panels[i]] = leftSlot.position;

            SetSorting(panels[i], relativeIndex);
        }

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = time / moveDuration;

            foreach (var panel in panels)
            {
                panel.position = Vector3.Lerp(startPos[panel], targetPos[panel], t);
            }

            yield return null;
        }

        isMoving = false;
    }

    int GetRelativeIndex(int panelIndex)
    {
        int diff = panelIndex - currentIndex;

        if (diff == 0) return 0;
        if (diff == 1 || diff == -2) return 1;
        return -1;
    }

    void SetSorting(Transform panel, int relativeIndex)
    {
        var canvas = panel.GetComponent<Canvas>();
        CanvasGroup group = panel.GetComponent<CanvasGroup>();

        if (canvas == null || group == null) return;

        if (relativeIndex == 0)
        {
            canvas.sortingOrder = 2; // devant
            group.alpha= 1.0f;
            group.interactable = true;
        }
        else
        {
            canvas.sortingOrder = 1; // derrière
            group.alpha = 0.25f;
            group.interactable = false;
        }
    }
}