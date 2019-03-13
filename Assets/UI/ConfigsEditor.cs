using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfigsEditor : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement _player;

    public TMP_InputField moveSpeed;
    public TMP_InputField jmpHeight;
    public TMP_InputField jmpReach;
    public Slider         jmpPeak;
    public Toggle         vhjToggle;
    public TMP_InputField vhjScale;

    void Start()
    {
        moveSpeed.text = _player.footSpeed.ToString();
        jmpHeight.text = _player.jmpHeight.ToString();
        jmpReach.text = _player.jmpReach.ToString();
        jmpPeak.value = _player.jmpPeak;
        vhjToggle.isOn = _player.varHJmp;
        vhjScale.text = _player.vhjGScale.ToString();
    }

    void Update()
    {
        _player.footSpeed = float.Parse(moveSpeed.text);
        _player.jmpHeight = float.Parse(jmpHeight.text);
        _player.jmpReach = float.Parse(jmpReach.text);
        _player.jmpPeak = jmpPeak.value;
        _player.varHJmp = vhjToggle.isOn;
        _player.vhjGScale = Mathf.Clamp(float.Parse(vhjScale.text), 1f, 10f);
    }
}
