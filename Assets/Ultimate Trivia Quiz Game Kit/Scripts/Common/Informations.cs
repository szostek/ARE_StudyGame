using UnityEngine;

static class Informations
{
    public static int StageIndex
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.CurrentStageIndexKey, 0);
        }
        set
        {
            PlayerPrefs.SetInt(Constants.CurrentStageIndexKey, value);
        }
    }

    public static int HeartAmount
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.CurrentHeartAmountKey, 3);
        }
        set
        {
            PlayerPrefs.SetInt(Constants.CurrentHeartAmountKey, value);
        }
    }

    public static bool IsNoAds
    {
        get
        {
            if (PlayerPrefs.GetInt(Constants.IsNoAdsKey, 0) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        set
        {
            if (value == true)
            {
                PlayerPrefs.SetInt(Constants.IsNoAdsKey, 1);
            }
            else
            {
                PlayerPrefs.SetInt(Constants.IsNoAdsKey, 0);
            }
        }
    }

    public static bool IsPlayBGM
    {
        get
        {
            if (PlayerPrefs.GetInt(Constants.IsPlayBGMKey, 1) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        set
        {
            if (value == true)
            {
                PlayerPrefs.SetInt(Constants.IsPlayBGMKey, 1);
            }
            else
            {
                PlayerPrefs.SetInt(Constants.IsPlayBGMKey, 0);
            }
        }
    }

    public static bool IsPlaySFX
    {
        get
        {
            if (PlayerPrefs.GetInt(Constants.IsPlaySFXKey, 1) == 1)
            {
                return true;
            }
            else
            {
                return false;

            }
        }
        set
        {
            if (value == true)
            {
                PlayerPrefs.SetInt(Constants.IsPlaySFXKey, 1);
            }
            else
            {
                PlayerPrefs.SetInt(Constants.IsPlaySFXKey, 0);
            }
        }
    }
}
