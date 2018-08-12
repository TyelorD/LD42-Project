using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// This is a fairly generic OptionsController that can be used to standardize the way to load valuetypes from PlayerPrefs.
public static class OptionsController {

    public const string SFX_VOLUME_KEY = "sfxVolume", MUSIC_VOLUME_KEY = "musicVolume", SHOW_SCORE_KEY = "showScore",
        SOUND_MUTE_KEY = "soundMuted", FULLSCREEN_KEY = "fullscreen", QUALITY_KEY = "quality", COLOR_BLIND_KEY = "colorBlind";

    private const string OPTIONS_KEY = "gameOptions";

    private enum OptionType { Float, Int, Bool }

    private static Dictionary<string, OptionData> options = new Dictionary<string, OptionData>();

    public static float SFXVolume
    {
        get { return LoadFloat(SFX_VOLUME_KEY, 1f); }
        set
        {
            SetFloat(SFX_VOLUME_KEY, value);

            GameController.Mixer.SetFloat("SFX Volume", (value - 1f) * 80f);
        }
    }

    public static float MusicVolume
    {
        get { return LoadFloat(MUSIC_VOLUME_KEY, 1f); }
        set
        {
            SetFloat(MUSIC_VOLUME_KEY, value);

            GameController.Mixer.SetFloat("Music Volume", (value - 1f) * 80f);
        }
    }

    public static int Quality
    {
        get { return LoadInt(QUALITY_KEY, 2); }
        set { SetInt(QUALITY_KEY, value); }
    }

    public static bool ShowScore
    {
        get { return LoadBool(SHOW_SCORE_KEY, true); }
        set { SetBool(SHOW_SCORE_KEY, value); }
    }

    public static bool SoundMuted
    {
        get { return LoadBool(SOUND_MUTE_KEY, false); }
        set { SetBool(SOUND_MUTE_KEY, value); }
    }

    public static bool Fullscreen
    {
        get { return LoadBool(FULLSCREEN_KEY, false); }
        set { SetBool(FULLSCREEN_KEY, value); }
    }

    public static bool ColorBlind
    {
        get { return LoadBool(COLOR_BLIND_KEY, false); }
        set { SetBool(COLOR_BLIND_KEY, value); }
    }

    public static float LoadFloat(string key, float defaultValue)
    {
        return (float)LoadValue(key, defaultValue, OptionType.Float);
    }

    public static int LoadInt(string key, int defaultValue)
    {
        return (int)LoadValue(key, defaultValue, OptionType.Int);
    }

    public static bool LoadBool(string key, bool defaultValue)
    {
        return (bool)LoadValue(key, defaultValue, OptionType.Bool);
    }

    public static void SetFloat(string key, float value)
    {
        SetValue(key, value);
    }

    public static void SetInt(string key, int value)
    {
        SetValue(key, value);
    }

    public static void SetBool(string key, bool value)
    {
        SetValue(key, value);
    }

    public static void SaveOptions()
    {
        string serializedOptions = SerializeOptions();

        PlayerPrefs.SetString(OPTIONS_KEY, serializedOptions);
    }

    public static void LoadOptions()
    {
        if(PlayerPrefs.HasKey(OPTIONS_KEY))
        {
            options = DeserializeOptions();

            MainMenu.optionsMenu.OnOptionsLoaded();
        }
    }

    public static void ClearOptions()
    {
        options.Clear();

        PlayerPrefs.DeleteKey(OPTIONS_KEY);
    }

    private static ValueType LoadValue(string key, ValueType defaultValue, OptionType type)
    {
        if (options.ContainsKey(key))
        {
            if (options[key].Type != type)
                Debug.Log("Option Value for " + key + " is not a " + type.ToString() + "!");

            return options[key].Value;
        }
        else
        {
            options.Add(key, new OptionData(key, defaultValue));

            return defaultValue;
        }

        
    }

    private static void SetValue(string key, ValueType value)
    {
        OptionData option;

        if (options.ContainsKey(key))
        {
            option = options[key];
            option.Value = value;
        }
        else
            option = new OptionData(key, value);

        options[key] = option;
    }

    private static string SerializeOptions()
    {
        StringBuilder builder = new StringBuilder("{");

        foreach(var kvp in options)
        {
            builder.Append(kvp.Value.ToJSON());
            builder.Append(";");
        }

        builder.Append("}");

        return builder.ToString();
    }

    private static Dictionary<string, OptionData> DeserializeOptions()
    {
        Dictionary<string, OptionData> ret = new Dictionary<string, OptionData>();

        if (PlayerPrefs.HasKey(OPTIONS_KEY))
        {
            string optionsString = PlayerPrefs.GetString(OPTIONS_KEY);

            optionsString = optionsString.Replace("{", "");
            optionsString = optionsString.Replace("}", "");
            string[] jsonParms = optionsString.Split(';');

            foreach(string parm in jsonParms)
            {
                OptionData option;

                if (OptionData.FromJSON(parm, out option))
                    ret.Add(option.Key, option);

                //Debug.Log(parm);
            }
        }

        return ret;
    }

    #region Unit Tests

    public static void OptionsTest1()
    {
        ClearOptions();

        LoadOptions();

        Debug.Log("Load Empty Options Passed");
    }

    public static void OptionsTest2()
    {
        ClearOptions();

        SaveOptions();

        Debug.Log("Save Empty Options Passed");
    }

    public static void OptionsTest3()
    {
        ClearOptions();

        float value = LoadFloat("notExists", 1f);

        Debug.Log("Non Existant Float Value = " + value);
    }

    public static void OptionsTest4()
    {
        ClearOptions();

        LoadFloat("exists", 3f);

        SaveOptions();
        LoadOptions();

        float value = LoadFloat("exists", 2f);

        Debug.Log("Existant Float Value: " + (value == 3f));
    }

    public static void OptionsTest5()
    {
        LoadOptions();

        float value = LoadFloat("exists", 2f);

        Debug.Log("Existant Float Value: " + (value == 3f));
    }

    #endregion
    
    [Serializable]
    private struct OptionData {

        public string Key { get; private set; }
        public OptionType Type { get; private set; }

        private ValueType _value { get; set; }
        public ValueType Value
        {
            get { return _value; }

            set
            {
                if (value is int)
                    Type = OptionType.Int;
                else if (value is bool)
                    Type = OptionType.Bool;
                else
                    Type = OptionType.Float;

                _value = value;
            }
        }

        public OptionData(string key, ValueType value)
        {
            Key = key;
            Type = OptionType.Float;
            _value = value;
            Value = value;
        }

        public string ToJSON()
        {
            StringBuilder builder = new StringBuilder("{");

            builder.Append(Key);
            builder.Append(",");
            builder.Append(Type);
            builder.Append(",");
            builder.Append(Value);

            builder.Append("}");

            return builder.ToString();
        }

        public static bool FromJSON(string json, out OptionData option)
        {
            option = new OptionData();

            try
            {
                json = json.Replace("{", "");
                json = json.Replace("}", "");
                string[] jsonParms = json.Split(',');

                if (jsonParms.Length == 3)
                {
                    option.Key = jsonParms[0];
                    option.Value = TryParseValue(jsonParms[2], TypeFromString(jsonParms[1]));

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private static ValueType TryParseValue(string value, OptionType type)
        {
            float retFloat = 0f;
            int retInt = 0;
            bool retBool = false;
            
            switch(type)
            {
                case OptionType.Int:
                int.TryParse(value, out retInt);
                return retInt;

                case OptionType.Bool:
                bool.TryParse(value, out retBool);
                return retBool;

                default:
                float.TryParse(value, out retFloat);
                return retFloat;
            }
        }

        private static OptionType TypeFromString(string type)
        {
            switch(type)
            {
                case "Int":
                return OptionType.Int;

                case "Bool":
                return OptionType.Bool;

                default:
                return OptionType.Float;
            }
        }
    }

}
