using System.Collections.Generic;

[System.Serializable]
public class SaveObject
{
    public Dictionary<int, ScoreTemplate> scores = new Dictionary<int, ScoreTemplate>();
}

[System.Serializable]
public class ScoreTemplate
{
    public string challengeType;
    public string percScore;
    public string timeScore;
}

