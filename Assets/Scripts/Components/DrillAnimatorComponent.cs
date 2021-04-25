using UnityEngine;

using System.Linq;
using System.Collections.Generic;

using DG.Tweening;

using UniRx;

public class DrillAnimatorComponent : MonoBehaviour
{
    [SerializeField]
    protected Transform _drill;

    [SerializeField]
    protected List<Sprite> _drillFrames;
    [SerializeField]
    protected SpriteRenderer _drillSprite;
    [SerializeField]
    protected List<Sprite> _trackFrames;
    [SerializeField]
    protected List<SpriteRenderer> _trackRenderers;

    protected Sequence _animation;

    protected int Frame => (int)(0.5 + Mathf.Cos(Time.time * 5.0f));

    private void Awake()
    {
        _animation = DOTween.Sequence()
            .Append(_drill.DOLocalMoveY(-0.03f, .25f))
            .Append(_drill.DOLocalMoveY(0.03f, .25f))
            .SetLoops(-1);

        transform
            .ObserveEveryValueChanged(transform => transform.position)
            .Subscribe(_ => UpdateTracks());

        transform
            .ObserveEveryValueChanged(transform => transform.rotation)
            .Subscribe(_ => UpdateTracks());
    }

    private void Update()
    {
        _drillSprite.sprite = _drillFrames[Frame];
    }
    
    protected void UpdateTracks()
    {
        _trackRenderers
            .ForEach(track => track.sprite = _trackFrames[Frame]);
    }

    private void OnDestroy()
    {
        _animation.Kill();
    }
}
