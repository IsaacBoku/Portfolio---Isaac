using UnityEngine;

[CreateAssetMenu(fileName = "SoundsScriptable", menuName = "Scriptable Sounds/SoundsType")]
public class SoundsScriptable : ScriptableObject
{
    public SoundSettings[] arraySonidos;

    public Texture2D[] textureSonidos;
}
