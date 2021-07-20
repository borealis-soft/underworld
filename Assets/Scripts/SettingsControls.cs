using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsControls : MonoBehaviour
{
	[SerializeField]
	private Slider SoundSlider;
	[SerializeField]
	private Slider MusicSlider;
	[SerializeField]
	private Toggle FullScreen;
	[SerializeField]
	private Dropdown Quality;
	[SerializeField]
	private Dropdown Resolution;
	[SerializeField]
	private AudioMixer SoundMixer;
	[SerializeField]
	private AudioMixer MusicMixer;

	private void Awake()
	{
		SoundMixer.GetFloat("VolumeMaster", out float temp);
		SoundSlider.value = temp;
		MusicMixer.GetFloat("VolumeMaster", out temp);
		MusicSlider.value = temp;
		FullScreen.isOn = Screen.fullScreen;

		Quality.value = QualitySettings.GetQualityLevel();

		Resolution.ClearOptions();
		int resID = 0;
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
