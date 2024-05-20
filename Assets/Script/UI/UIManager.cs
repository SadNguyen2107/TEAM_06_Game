using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Sound Section
    [Header("Sound")]

    [SerializeField] Slider _volumeSlider;
    [SerializeField] Button _soundButton;
    [SerializeField] Sprite _soundButtonUnMuteSprite;
    [SerializeField] Sprite _soundButtonMuteSprite;

    [Header("Pause & Continue")]
    [SerializeField] GameObject _joyStickController;
    [SerializeField] Animator _pauseButtonAnimator;
    [SerializeField] Animator _pausePanelAnimator;
    [SerializeField] GameObject[] _otherObjectsToDisappear;
    public void OnVolumeChange()
    {
        // Change the Volume of the Audio Manager
        AudioManager.Instance.SetVolume(_volumeSlider.value);
    }

    /// <summary>
    ///     Toggle the Sound Button
    /// </summary>
    public void OnSoundButtonClick()
    {
        // Toggle the Mute
        bool isMute = !AudioManager.Instance.Mute;

        // Mute the Sound
        AudioManager.Instance.Mute = isMute;

        // Show the Image
        if (isMute)
        {
            _soundButton.image.sprite = _soundButtonMuteSprite;
        }
        else
        {
            _soundButton.image.sprite = _soundButtonUnMuteSprite;
        }
    }

    public void OnChangeSceneButton(int sceneBuildIndex) => SceneManager.LoadSceneAsync(sceneBuildIndex);
    public void OnPauseButton()
    {
        // Disable the Joystick
        _joyStickController.SetActive(false);

        // Run the Pause Panel Animation
        _pausePanelAnimator.SetBool("IsPause", true);

        // Run that Button Animation
        _pauseButtonAnimator.SetBool("IsPause", true);

        // Dis-Appear these GameObject
        foreach (GameObject gameObject in _otherObjectsToDisappear)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnContinueButton()
    {
        // Active the JoyStick
        _joyStickController.SetActive(true);

        // Run the Pause Panel Animation
        _pausePanelAnimator.SetBool("IsPause", false);

        // Run that Button Animation
        _pauseButtonAnimator.SetBool("IsPause", false);

        // Re-Appear these GameObject
        foreach (GameObject gameObject in _otherObjectsToDisappear)
        {
            gameObject.SetActive(true);
        }
    }
}
