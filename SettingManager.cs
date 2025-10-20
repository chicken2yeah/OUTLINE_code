using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject setting_back_view;
    public InputManager inputManager;

    public Slider master_vol_slider;
    public Slider bgm_vol_slider;
    public Slider sfx_vol_slider;

    public TMP_Text left_text; // 1
    public TMP_Text right_text; // 2
    public TMP_Text jump_text; // 3
    public TMP_Text break_text; // 4
    public TMP_Text interact_text; // 5
    public TMP_Text parry_text; // 6
    // reset // 7

    public int btn_index = 0;

    float masterVol = 0;
    float bgmVol = 0;
    float sfxVol = 0;

    public bool is_setting = false;

    KeyCode setKey;
    bool keySetted = false;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("MasterVol"))
        {
            PlayerPrefs.SetFloat("MasterVol",0);
        }
        else
        {
            masterVol = PlayerPrefs.GetFloat("MasterVol");
            audioMixer.SetFloat("MASTER_VOL", masterVol);
            master_vol_slider.value = masterVol;
        }
        master_vol_slider.value = masterVol;
        if (!PlayerPrefs.HasKey("BgmVol"))
        {
            PlayerPrefs.SetFloat("BgmVol", 0);
        }
        else
        {
            bgmVol = PlayerPrefs.GetFloat("BgmVol");
            bgm_vol_slider.value = bgmVol;
            audioMixer.SetFloat("BGM_VOL", bgmVol);
        }
        bgm_vol_slider.value = bgmVol;
        if (!PlayerPrefs.HasKey("SfxVol"))
        {
            PlayerPrefs.SetFloat("SfxVol", 0);
        }
        else
        {
            sfxVol = PlayerPrefs.GetFloat("SfxVol");
            sfx_vol_slider.value = sfxVol;
            audioMixer.SetFloat("SFX_VOL", sfxVol);
        }
        sfx_vol_slider.value = sfxVol;
        inputManager.Load_keys();
        left_text.text = inputManager.left_move_key.ToString();
        right_text.text = inputManager.right_move_key.ToString();
        jump_text.text = inputManager.up_move_key.ToString();
        break_text.text = inputManager.break_control_key.ToString();
        interact_text.text = inputManager.interact_key.ToString();
        parry_text.text = inputManager.parry_control_key.ToString();
    }

    public void Resetting_values()
    {
        inputManager.Set_Defalut_input();
        PlayerPrefs.SetFloat("MasterVol", 0);
        PlayerPrefs.SetFloat("BgmVol", 0);
        PlayerPrefs.SetFloat("SfxVol", 0);
        masterVol = 0;
        master_vol_slider.value = masterVol;
        audioMixer.SetFloat("MASTER_VOL", masterVol);
        bgmVol = 0;
        bgm_vol_slider.value = bgmVol;
        audioMixer.SetFloat("BGM_VOL", bgmVol);
        sfxVol = 0;
        sfx_vol_slider.value = sfxVol;
        audioMixer.SetFloat("SFX_VOL", sfxVol);
        left_text.text = "A";
        right_text.text = "D";
        jump_text.text = "Space";
        break_text.text = "S";
        interact_text.text = "F";
        parry_text.text = "W";
    }

    private void OnGUI()
    {
        if (is_setting)
        {
            Event key = Event.current;
            if (key.isKey && !keySetted)
            {
                setKey = key.keyCode;
                keySetted = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !is_setting)
        {
            setting_back_view.SetActive(!setting_back_view.activeSelf);
            if (setting_back_view.activeSelf)
            {
                Time.timeScale = 0;
                masterVol = PlayerPrefs.GetFloat("MasterVol");
                master_vol_slider.value = masterVol;
                audioMixer.SetFloat("MASTER_VOL", masterVol);
                bgmVol = PlayerPrefs.GetFloat("BgmVol");
                bgm_vol_slider.value = bgmVol;
                audioMixer.SetFloat("BGM_VOL", bgmVol);
                sfxVol = PlayerPrefs.GetFloat("SfxVol");
                sfx_vol_slider.value = sfxVol;
                audioMixer.SetFloat("SFX_VOL", sfxVol);
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        if (setting_back_view.activeSelf)
        {
            if (!is_setting)
            {
                if (setting_back_view.activeSelf)
                {
                    if (master_vol_slider.value != masterVol)
                    {
                        masterVol = master_vol_slider.value;
                        PlayerPrefs.SetFloat("MasterVol", masterVol);
                        audioMixer.SetFloat("MASTER_VOL", masterVol);
                    }
                    if (bgm_vol_slider.value != bgmVol)
                    {
                        bgmVol = bgm_vol_slider.value;
                        PlayerPrefs.SetFloat("BgmVol", bgmVol);
                        audioMixer.SetFloat("BGM_VOL", bgmVol);
                    }
                    if (sfx_vol_slider.value != sfxVol)
                    {
                        sfxVol = sfx_vol_slider.value;
                        PlayerPrefs.SetFloat("SfxVol", sfxVol);
                        audioMixer.SetFloat("SFX_VOL", sfxVol);
                    }
                }
            }
        }
    }

    public IEnumerator SetKey()
    {
        is_setting = true;
        switch (btn_index)
        {
            case (1):
                {
                    left_text.text = "";
                    break;
                }
            case (2):
                {
                    right_text.text = "";
                    break;
                }
            case (3):
                {
                    jump_text.text = "";
                    break;
                }
            case (4):
                {
                    break_text.text = "";
                    break;
                }
            case (5):
                {
                    interact_text.text = "";
                    break;
                }
            case (6):
                {
                    parry_text.text = "";
                    break;
                }
        }

        bool allow_key = false;
        while (!allow_key)
        {
            yield return new WaitUntil(()=>(keySetted));
            allow_key = true;
            if (setKey == KeyCode.Escape)
            {
                allow_key = false;
            }
            if (setKey == inputManager.left_move_key && btn_index != 1)
            {
                allow_key = false;
            }
            else if (setKey == inputManager.right_move_key && btn_index != 2)
            {
                allow_key = false;
            }
            else if (setKey == inputManager.up_move_key && btn_index != 3)
            {
                allow_key = false;
            }
            else if (setKey == inputManager.break_control_key && btn_index != 4)
            {
                allow_key = false;
            }
            else if (setKey == inputManager.interact_key && btn_index != 5)
            {
                allow_key = false;
            }
            else if (setKey == inputManager.parry_control_key && btn_index != 6)
            {
                allow_key = false;
            }
            if (allow_key == false)
            {
                keySetted = false;
            }
        }

        switch (btn_index)
        {
            case (1):
                {
                    inputManager.Set_input(ref inputManager.left_move_key,"left_move_key",setKey);
                    left_text.text = setKey.ToString();
                    break;
                }
            case (2):
                {
                    inputManager.Set_input(ref inputManager.right_move_key, "right_move_key", setKey);
                    right_text.text = setKey.ToString();
                    break;
                }
            case (3):
                {
                    inputManager.Set_input(ref inputManager.up_move_key, "up_move_key", setKey);
                    jump_text.text = setKey.ToString();
                    break;
                }
            case (4):
                {
                    inputManager.Set_input(ref inputManager.break_control_key, "break_control_key", setKey);
                    break_text.text = setKey.ToString();
                    break;
                }
            case (5):
                {
                    inputManager.Set_input(ref inputManager.interact_key, "interact_key", setKey);
                    interact_text.text = setKey.ToString();
                    break;
                }
            case (6):
                {
                    inputManager.Set_input(ref inputManager.parry_control_key, "parry_control_key", setKey);
                    parry_text.text = setKey.ToString();
                    break;
                }
        }
        is_setting = false;
        keySetted = false;
        btn_index = 0;
    }
}
