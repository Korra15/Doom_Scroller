using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class LocalizationSelector : MonoBehaviour
{
    private bool active = false;


    public void ChangeLocale(int localID)
    {
        if (active == true)
        {
            return;
        }

        StartCoroutine(SetLocale(localID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];

    }


}