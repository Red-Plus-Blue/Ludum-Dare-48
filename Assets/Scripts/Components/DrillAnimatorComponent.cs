using UnityEngine;

using DG.Tweening;
public class DrillAnimatorComponent : MonoBehaviour
{
    [SerializeField]
    protected Transform _drill;

    protected Sequence _animation;

    private void Awake()
    {
        _animation = DOTween.Sequence()
            .Append(_drill.DOLocalMoveY(-0.05f, .25f))
            .Append(_drill.DOLocalMoveY(0.05f, .25f))
            .SetLoops(-1);
    }

    private void OnDestroy()
    {
        _animation.Kill();
    }
}
