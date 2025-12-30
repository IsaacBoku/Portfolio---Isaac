using UnityEngine;
using System.Collections;

public class ObjectScaler : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _targetObject;
    [SerializeField] private Vector3 _smallScale = Vector3.one;
    [SerializeField] private Vector3 _bigScale = new Vector3(2.5f, 2.5f, 2.5f);
    [SerializeField] private float _duration = 1f;

    private bool _isBig = false;
    private Coroutine _scaleCoroutine;

    public string GetInteractionText() => _isBig ? "Encoger Monumento" : "Agrandar Monumento";

    public void Interact()
    {
        _isBig = !_isBig;
        if (_scaleCoroutine != null) StopCoroutine(_scaleCoroutine);
        _scaleCoroutine = StartCoroutine(ScaleRoutine(_isBig ? _bigScale : _smallScale));
    }

    private IEnumerator ScaleRoutine(Vector3 target)
    {
        float elapsed = 0;
        Vector3 startScale = _targetObject.localScale;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            _targetObject.localScale = Vector3.Lerp(startScale, target, elapsed / _duration);
            yield return null;
        }
        _targetObject.localScale = target;
    }
}
