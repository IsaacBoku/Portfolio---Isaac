using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Referencias")]
    [SerializeField] private CanvasGroup _fadeGroup;
    [SerializeField] private float _fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _fadeGroup.alpha = 1; // Empezamos en negro
        }
        else { Destroy(gameObject); }
    }

    private void Start() => StartCoroutine(Fade(0));

    public void LoadScene(string sceneName) => StartCoroutine(Transition(sceneName));

    private IEnumerator Transition(string sceneName)
    {
        yield return StartCoroutine(Fade(1));
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return StartCoroutine(Fade(0));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = _fadeGroup.alpha;
        float elapsed = 0;
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / _fadeDuration);
            yield return null;
        }
        _fadeGroup.alpha = targetAlpha;
        _fadeGroup.blocksRaycasts = (targetAlpha == 1);
    }
}