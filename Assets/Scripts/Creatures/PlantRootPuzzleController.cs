using UnityEngine;

public class PlantRootPuzzleController : MonoBehaviour
{
    [Header("Monster System")]
    [SerializeField] private MonsterEncounterManager encounterManager;

    [Header("Puzzle Visual Root")]
    [SerializeField] private GameObject rootVisuals;

    [Header("Roots")]
    [SerializeField] private PlantRootWeakPoint[] roots;

    [Header("Settings")]
    [SerializeField] private bool disableAfterSolved = true;

    private int clickedRoots;
    private bool solved;
    private bool currentlyShowing;

    private void Start()
    {
        UpdateVisibility();
    }

    private void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (encounterManager == null || rootVisuals == null)
            return;

        bool shouldShow =
            encounterManager.CurrentMonster != null &&
            encounterManager.CurrentMonster.defeatType == MonsterDefeatType.WeakPoints &&
            !solved;

        if (currentlyShowing == shouldShow)
            return;

        currentlyShowing = shouldShow;
        rootVisuals.SetActive(shouldShow);

        if (shouldShow)
            ResetPuzzle();
    }

    public void RootClicked(PlantRootWeakPoint root)
    {
        if (solved)
            return;

        clickedRoots++;

        Debug.Log("Plant root clicked: " + clickedRoots + "/" + roots.Length);

        if (clickedRoots >= roots.Length)
        {
            solved = true;

            if (encounterManager != null)
            {
                bool defeated = encounterManager.TryDefeatMonster(MonsterDefeatType.WeakPoints);

                Debug.Log(defeated
                    ? "[PlantRootPuzzle] Plant defeated."
                    : "[PlantRootPuzzle] Wrong monster. Battery drained.");
            }

            if (disableAfterSolved && rootVisuals != null)
                rootVisuals.SetActive(false);
        }
    }

    public void ResetPuzzle()
    {
        solved = false;
        clickedRoots = 0;

        for (int i = 0; i < roots.Length; i++)
        {
            if (roots[i] != null)
                roots[i].ResetRoot();
        }
    }
}