
namespace _Main.Scripts.Toolkit.File
{
    public interface IStorageService
    {
        string GetString(string key);
        void SetString(string key, string value);

        void SetInt(string key, int value);
        int GetInt(string key, int defaultValue = 0);
        
        bool HasKey(string key);
        void DeleteKey(string key);
        
        void Commit();
    }
}