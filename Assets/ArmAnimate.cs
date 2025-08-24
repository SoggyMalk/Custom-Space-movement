using UnityEngine;
using System.Collections;

public class ArmToggleAnimator : MonoBehaviour
{
    public Animator armAnimator;

    public string moveUpClip = "MoveUp";
    public string idleUpClip = "IdleUp";
    public string moveDownClip = "MoveDown";
    public string idleDownClip = "IdleDown";

    private bool handIsUp = false;

    void Start()
    {
        armAnimator.Play(idleDownClip);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!handIsUp)
            {
                StartCoroutine(PlayAnimation(moveUpClip, idleUpClip));
                handIsUp = true;
            }
            else
            {
                StartCoroutine(PlayAnimation(moveDownClip, idleDownClip));
                handIsUp = false;
            }
        }
    }

    IEnumerator PlayAnimation(string moveClip, string idleClip)
    {
        armAnimator.Play(moveClip);
        // Wait for the clip length before switching to idle
        yield return new WaitForSeconds(GetClipLength(moveClip));
        armAnimator.Play(idleClip);
    }

    float GetClipLength(string clipName)
    {
        foreach (var clip in armAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName) return clip.length;
        }
        return 0f;
    }
}
