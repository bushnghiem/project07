using UnityEngine;

public abstract class AnimatedUI : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    public virtual void Open()
    {
        gameObject.SetActive(true);
        PlayOpenAnimation();
    }

    public virtual void Close()
    {
        PlayCloseAnimation();
    }

    public void DisableAfterClose()
    {
        gameObject.SetActive(false);
    }

    protected abstract void PlayOpenAnimation();
    protected abstract void PlayCloseAnimation();
}

