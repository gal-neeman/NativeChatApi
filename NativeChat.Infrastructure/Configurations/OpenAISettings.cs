namespace NativeChat;

public class OpenAISettings
{
    public string Model { get; set; } = null!;

    public float FrequencyPenalty { get; set; }

    public int MaxOutputTokenCount { get; set; }

    public float PresencePenalty { get; set; }

    public float Temperature { get; set; }

    public string SystemMessage { get; set; } = null!;

    public string GetSystemMessage(string language, string name)
    {
        return SystemMessage + ".Your name is " + name + ". You are a native " + language + " speaker helping the user practice natural conversation. Speak fluent, casual " + language + " unless you are told otherwise";
    }
}
