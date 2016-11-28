using Microsoft.Win32;

namespace BeamgunApp.Models
{
    public interface IDynamicDictionary
    {
        T GetWithDefault<T>(string key, T defaultValue);
        void Set<T>(string key, T value);
    }

    public class RegistryBackedDictionary : IDynamicDictionary
    {
        public const string BeamgunBaseKey = "HKEY_CURRENT_USER\\SOFTWARE\\Beamgun";

        public T GetWithDefault<T>(string key, T defaultValue)
        {
            return (T)Registry.GetValue(BeamgunBaseKey, key, defaultValue);
        }

        public void Set<T>(string key, T value)
        {
            Registry.SetValue(BeamgunBaseKey, key, value);
        }
    }
}
