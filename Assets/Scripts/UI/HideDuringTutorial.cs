using UnityEngine;

public class HideDuringTutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToHide;

    public void HideObjects()
    {
        SetObjects(false);
    }

    public void ShowObjects()
    {
        SetObjects(true);
    }

    private void SetObjects(bool show)
    {
        for (int i = 0; i < objectsToHide.Length; i++)
        {
            if (objectsToHide[i] != null)
                objectsToHide[i].SetActive(show);
        }
    }
}