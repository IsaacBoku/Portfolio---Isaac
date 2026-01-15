using UnityEngine;

public class FixWebGLCanvas : MonoBehaviour
{
    void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        UnityEngine.WebGLInput.captureAllKeyboardInput = false;
        // Fuerza al navegador a usar la densidad de píxeles real (Retina/4K)
        Screen.fullScreen = true; 
#endif
    }
}
