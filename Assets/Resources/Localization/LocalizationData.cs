using System;
using System.Collections.Generic;

[Serializable]
public class LocalizationLanguage
{
    public List<LocalizationEntry> entries;
}

[Serializable]
public class LocalizationEntry
{
    public string key;
    public string value;
}

[Serializable]
public class LocalizationRoot
{
    public LocalizationLanguage English;
    public LocalizationLanguage Russian;
}