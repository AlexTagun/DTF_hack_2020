using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private ChildAI _childAI = null;

    public void PlayAnimIdle()
    {
        _animator.Play("idle");
        _childAI.pauseAI = true;
    }
    public void PlayAnimRun()
    {
        _animator.Play("run");
        _childAI.pauseAI = false;
    }

    public void PlayAnimEat()
    {
        _animator.Play("eat");
        _childAI.pauseAI = true;
    }
    public void PlayAnimDrag()
    {
        _animator.Play("drag");
        _childAI.pauseAI = true;
    }

}
