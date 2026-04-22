using UnityEngine;

public class BookPageTurnAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayPageTurn()
    {
        if (animator != null)
        {
            animator.ResetTrigger("PlayTurn");
            animator.SetTrigger("PlayTurn");
        }
    }
}