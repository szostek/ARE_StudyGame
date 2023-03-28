using System.Collections.Generic;

[System.Serializable]
public class SaveObject
{
    public Dictionary<int, ScoreTemplate> scores = new Dictionary<int, ScoreTemplate>();
    public List<int> struggleList = new List<int>();
}

[System.Serializable]
public class ScoreTemplate
{
    public string challengeType;
    public string percScore;
    public string timeScore;
}

