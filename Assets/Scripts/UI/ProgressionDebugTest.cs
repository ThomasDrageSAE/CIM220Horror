using UnityEngine;

public class ProgressionDebugTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (NetworkProgressionManager.Instance != null)
            {
                Debug.Log("[ProgressionDebugTest] Advancing progression.");
                NetworkProgressionManager.Instance.AdvanceProgression();
            }
            else
            {
                Debug.LogWarning("[ProgressionDebugTest] No NetworkProgressionManager found.");
            }
        }
    }
}