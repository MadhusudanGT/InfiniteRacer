using UnityEngine;
using DG.Tweening;

public class ScaleAnimation : MonoBehaviour
{
    [SerializeField] Transform _aniPanel;

    [SerializeField] int _sclaeUpSize = 1;

    [SerializeField] Ease _scaleUpType = Ease.OutBack;
    [SerializeField] Ease _scaleDownType = Ease.InBack;

    [SerializeField] float _scaleUpDuration = 0.5f;

    [SerializeField] bool _awakeOnLoad = false;

    private void OnEnable()
    {
        if (_awakeOnLoad)
        {
            ShowApperAni();
        }
        EventManager.SelectedVehiclePanel += AnimatedThePanel;
    }

    private void OnDisable()
    {
        EventManager.SelectedVehiclePanel -= AnimatedThePanel;
    }

    void AnimatedThePanel(VehicleType type)
    {
        ShowApperAni();
    }

    public void ShowApperAni()
    {
        _aniPanel.localScale = new Vector3(0, 0, 1);

        _aniPanel.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_aniPanel.DOScale(new Vector3(_sclaeUpSize, _sclaeUpSize, 1), _scaleUpDuration).SetEase(_scaleUpType));
    }
}
