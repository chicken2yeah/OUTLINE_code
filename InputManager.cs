using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    public bool left_move = false;

    public bool right_move = false;

    public bool up_move = false;

    public bool interact = false;

    public bool nomal_atk = false;

    public bool dash_atk = false;

    public bool parry_control = false;

    public bool break_control = false;

    enum ControlKeys
    {
        left_move = 0,
        right_move = 1,
        up_move = 2,
        interact = 3,
        parry_control = 4
    }

    public KeyCode left_move_key;
    public KeyCode right_move_key;
    public KeyCode up_move_key;
    public KeyCode interact_key;
    public KeyCode parry_control_key;
    public KeyCode break_control_key;

    public static byte[] KeyCodeToByteArray(KeyCode obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static KeyCode ByteArrayToKeyCode(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return (KeyCode)obj;
        }
    }

    void Start()
    {
        Load_keys();
    }

    public void Load_keys()
    {
        bool is_empty = false;

        // 저장 : keycode -> byte[] -> base64string
        // 불러오기 : 저장의 역순

        // 하나라도 비어있을 경우 전체 초기화
        if (PlayerPrefs.HasKey("left_move_key"))
        {
            left_move_key = ByteArrayToKeyCode(Convert.FromBase64String(PlayerPrefs.GetString("left_move_key")));
        }
        else
        {
            is_empty = true;
        }


        if (PlayerPrefs.HasKey("right_move_key"))
        {
            right_move_key = ByteArrayToKeyCode(Convert.FromBase64String(PlayerPrefs.GetString("right_move_key")));
        }
        else
        {
            is_empty = true;
        }

        if (PlayerPrefs.HasKey("up_move_key"))
        {
            up_move_key = ByteArrayToKeyCode(Convert.FromBase64String(PlayerPrefs.GetString("up_move_key")));
        }
        else
        {
            is_empty = true;
        }

        if (PlayerPrefs.HasKey("interact_key"))
        {
            interact_key = ByteArrayToKeyCode(Convert.FromBase64String(PlayerPrefs.GetString("interact_key")));
        }
        else
        {
            is_empty = true;
        }

        if (PlayerPrefs.HasKey("parry_control_key"))
        {
            parry_control_key = ByteArrayToKeyCode(Convert.FromBase64String(PlayerPrefs.GetString("parry_control_key")));
        }
        else
        {
            is_empty = true;
        }

        if (PlayerPrefs.HasKey("break_control_key"))
        {
            break_control_key = ByteArrayToKeyCode(Convert.FromBase64String(PlayerPrefs.GetString("break_control_key")));
        }
        else
        {
            is_empty = true;
        }

        if (is_empty)
        {
            Set_Defalut_input();
        }
    }

    public void Set_Defalut_input()
    {
        Set_input(ref left_move_key,"left_move_key",KeyCode.A);
        Set_input(ref right_move_key,"right_mov_key",KeyCode.D);
        Set_input(ref up_move_key, "up_move_key", KeyCode.Space);
        Set_input(ref interact_key, "interact_key", KeyCode.F);
        Set_input(ref parry_control_key, "parry_control_key", KeyCode.W);
        Set_input(ref break_control_key, "break_control_key", KeyCode.S);
    }

    public void Set_input(ref KeyCode key, string keyName, KeyCode new_key)
    {
        key = new_key;
        PlayerPrefs.SetString(keyName, Convert.ToBase64String(KeyCodeToByteArray(new_key)));
    }

    // Update is called once per frame
    void Update()
    {
        left_move = Input.GetKey(left_move_key);
        right_move = Input.GetKey(right_move_key);
        up_move = Input.GetKey(up_move_key);
        break_control = Input.GetKey(break_control_key);
        if (Input.GetKeyDown(parry_control_key))
        {
            parry_control = true;
        }
        if (Input.GetKeyUp(parry_control_key))
        {
            parry_control = false;
        }

        if (Input.GetKeyDown(interact_key))
        {
            interact = true;
        }
        if (Input.GetKeyUp(interact_key))
        {
            interact = false;
        }

        nomal_atk = Input.GetMouseButton((int)MouseButton.LeftMouse);
        dash_atk = Input.GetMouseButton((int)MouseButton.RightMouse);
    }
}
