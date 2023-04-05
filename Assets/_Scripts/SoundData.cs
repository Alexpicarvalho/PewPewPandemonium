using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/Sound Data", fileName = "SoundData")]
public class SoundData : ScriptableObject
{
    [SerializeField] AudioClip[] _sounds;
    [SerializeField] [Range(0f, 1f)] float[] _soundVolume;
    private int _selectedIndex;

    public AudioClip GetRandomSound()
    {
        if (_sounds.Length == 0) return null;
        _selectedIndex = Random.Range(0, _sounds.Length);
        return _sounds[_selectedIndex];
    }
    public float GetClipVolume()
    {
        if (_soundVolume.Length == 0) return 0;
        if (_selectedIndex >= _soundVolume.Length) return 1;
        return _soundVolume[_selectedIndex];
    }
}
