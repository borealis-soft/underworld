using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsControls : MonoBehaviour
{
    public Slider SoundSlider;
    public Slider MusicSlider;
    public Toggle FullScreen;
    public Dropdown Quality;
    public Dropdown Resolution;
    [SerializeField]
    private AudioMixer SoundMixer;
    [SerializeField]
    private AudioMixer MusicMixer;

    private void OnEnable()
    {
        SoundMixer.GetFloat("VolumeMaster", out float temp);
        SoundSlider.value = temp;
        MusicMixer.GetFloat("VolumeMaster", out temp);
        MusicSlider.value = temp;
        FullScreen.isOn = Screen.fullScreen;

        Quality.value = QualitySettings.GetQualityLevel();

        Resolution.ClearOptions();
        int resID = -1;
        List<string> resolutions = new List<string>();
        foreach (var resolution in Screen.resolutions)
        {
            resolutions.Add(resolution.width + " X " + resolution.height);
            if (Screen.currentResolution.Equals(resolution))
                resID = resolutions.Count - 1;
        }
        Resolution.AddOptions(resolutions);
        Resolution.value = resID;
    }

    private void OnDisable()
    {
        using StreamWriter writer = new StreamWriter("Settings.conf");
        XmlSerializer serializer = new XmlSerializer(typeof(IntroWait.Settings));
        IntroWait.Settings settings = new IntroWait.Settings()
        {
            MusicVal = MusicSlider.value,
            SoundVal = SoundSlider.value,
            Resolution = Screen.currentResolution
        };
        serializer.Serialize(writer, settings);
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SetSoundVolume(float val)
    {
        SoundMixer.SetFloat("VolumeMaster", val);
    }

    public void SetMusicVolume(float val)
    {
        MusicMixer.SetFloat("VolumeMaster", val);
    }

    public void SetQuality(int lvl)
    {
        QualitySettings.SetQualityLevel(lvl);
    }

    public void SetResolution(int r)
    {
        Resolution[] rsl = Screen.resolutions;
        Screen.SetResolution(rsl[r].width, rsl[r].height, Screen.fullScreen);
    }
}
