using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TAW_VR.Runtime.Core.Tools
{
  [RequireComponent(typeof(Collider), typeof(Rigidbody))]
  public class LongPressButton : MonoBehaviour
  {
    [Title("Settings")]
    public float duration = 0.5f;
    public UnityEvent callback;
    [Tooltip("TAW-VR/Runtime/Core/Prefabs/LoadingPanel.prefab")] public GameObject loadingBarPrefab;
    public Vector3 loadingPositionOffset = new Vector3(0, 0, -0.51f);
    public Vector3 loadingRotationOffset;
    
    [Title("Privates")]
    public float Timer { get; private set; }
    private bool _isInitialized;
    private bool _toggled;
    private GameObject _loadingCanvas;

    [Title("Loading bar")] 
    private Slider _loadingSlider;

    private void OnTriggerStay(Collider other)
    {
      if (!_isInitialized)
      {
        _loadingCanvas = Instantiate(loadingBarPrefab, transform);
        _loadingSlider = _loadingCanvas.GetComponentInChildren<Slider>();
        Transform loadingBarTransform = _loadingCanvas.transform;
        loadingBarTransform.localPosition = loadingPositionOffset;
        loadingBarTransform.localRotation = Quaternion.Euler(loadingRotationOffset);
        _loadingSlider.value = 0;
        _loadingSlider.maxValue = duration;
        _isInitialized = true;
      }
      
      _loadingCanvas.gameObject.SetActive(true);
      
      
      if (callback == null || _toggled) return;
      
      Timer += Time.deltaTime;
      _loadingSlider.value = Timer;
      if (Timer >= duration)
      {
        callback.Invoke();
        _toggled = true;
        _loadingCanvas.gameObject.SetActive(false);
        Timer = 0;
      }
    }

    private void OnTriggerExit(Collider other)
    {
      _toggled = false;
      _loadingCanvas.gameObject.SetActive(false);
      Timer = 0;
    }
  }
}
