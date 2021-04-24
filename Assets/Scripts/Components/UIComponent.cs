using UnityEngine;

using TMPro;

public class UIComponent : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text _money;
    [SerializeField]
    protected RectTransform _needle;

    protected float _fuelLevel;

    public void SetFuelLevel(float level)
    {
        _fuelLevel = level;
        _needle.rotation = Quaternion.Euler(0f, 0f, 80f + (-160 * level));
    }

    public void SetMoney(int amount)
    {
        _money.text = amount.ToString("###,###,##0");
    }
}
