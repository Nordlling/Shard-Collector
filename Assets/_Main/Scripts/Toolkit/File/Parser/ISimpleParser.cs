namespace Main.Scripts.Infrastructure.Services.GameGrid.Parser
{
    public interface ISimpleParser
    {
        T ParseText<T>(string json) where T : class;
    }
}