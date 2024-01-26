using UnityEngine;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    private Slider slider;

    private float _maxExp;
    public float MaxExp
    {
        get { return _maxExp; }
        set
        {
            _maxExp = value;
            slider.maxValue = value;
        }
    }

    private float _currentExp;
    public float CurrentExp
    {
        get { return _currentExp; }
        set
        {
            _currentExp = value;
            slider.value = value;
        }
    }

    private void Awake()
    {
        slider = transform.GetComponentInChildren<Slider>();
    }
}

