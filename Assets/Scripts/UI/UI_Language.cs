using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UI_Language
{
    public static event Action Changed;
    const string PrefKey = "language";   // 0 = ES, 1 = EN

    public static int Current =>
        PlayerPrefs.GetInt(PrefKey, Application.systemLanguage == SystemLanguage.Spanish ? 0 : 1);

    static readonly Dictionary<string, string[]> table = new()
    {
        ["title"]    = new[] { "VOLANTEANDO", "WHEELIN’" },
        ["crashed"]  = new[] { "¡Chocaste!", "Ya crashed!" },
        ["offscreen"]= new[] { "¡¿Ké hacé?!", "Where u goin'?" },
        ["best"]     = new[] { "RECORD", "BEST" },
        ["diff0"]    = new[] { "FACIL", "EASY" },
        ["diff1"]    = new[] { "NORMAL", "NORMAL" },
        ["diff2"]    = new[] { "DIFICIL", "HARD" },
        ["diff3"]    = new[] { "IMPOSIBLE", "IMPOSSIBLE" },
        ["musicVolume"]    = new[] { "VOLUMEN MUSICA", "MUSIC VOLUME" },
        ["sfxVolume"]    = new[] { "VOLUMEN EFECTOS", "EFFECTS VOLUME" },
        ["curb"]    = new[] { "Chocar con los cordones", "Enable curb collision" },
        ["obstacles"]    = new[] { "Obstaculos en la calle", "Enable road obstacles" },
        ["controls"]    = new[] { "ESPACIO O CLICK", "SPACE OR CLICK" },
    };

    public static string Get(string key) =>
        table.TryGetValue(key, out var v) ? v[Current] : key;

    public static void Set(int lang)
    {
        PlayerPrefs.SetInt(PrefKey, lang);
        PlayerPrefs.Save();
        Changed?.Invoke();
    }
}