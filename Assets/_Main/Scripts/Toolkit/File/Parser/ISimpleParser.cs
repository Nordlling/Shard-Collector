namespace _Main.Scripts.Toolkit.File.Parser
{
    public interface ISimpleParser
    {
        T ParseFromText<T>(string json) where T : class;
        string ParseToText<T>(T value) where T : class;
    }
}