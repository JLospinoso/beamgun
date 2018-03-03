﻿using System;
using Microsoft.Win32;

namespace BeamgunApp.Models
{
    public interface IDynamicDictionary
    {
        Guid GetWithDefault(string key, Guid defaultValue);
        bool GetWithDefault(string key, bool defaultValue);
        string GetWithDefault(string key, string defaultValue);
        uint GetWithDefault(string key, uint defaultValue);
        double GetWithDefault(string key, double defaultValue);
        void Set<T>(string key, T value);
    }

    public class RegistryBackedDictionary : IDynamicDictionary
    {
        public Action<string> BadCastReport;
        public const string BeamgunBaseKey = @"HKEY_CURRENT_USER\SOFTWARE\Beamgun";
        public RegistryBackedDictionary()
        {
          Set(BeamgunBaseKey, ""); // Make sure registry key exists
        }
        public string GetWithDefault(string key, string defaultValue)
        {
            return GetRegistryValue(key, defaultValue) ?? defaultValue;
        }

        public double GetWithDefault(string key, double defaultValue)
        {
            return double.TryParse(GetRegistryValue(key, defaultValue), out var result)
                ? result
                : defaultValue;
        }
        
        public uint GetWithDefault(string key, uint defaultValue)
        {
            return uint.TryParse(GetRegistryValue(key, defaultValue), out var result)
                ? result
                : defaultValue;
        }

        public Guid GetWithDefault(string key, Guid defaultValue)
        {
            return Guid.TryParse(GetRegistryValue(key, defaultValue), out var result)
                ? result
                : defaultValue;
        }

        public bool GetWithDefault(string key, bool defaultValue)
        {
            return (GetRegistryValue(key, defaultValue) ?? "True") == "True";
        }

        public void Set<T>(string key, T value)
        {
            Registry.SetValue(BeamgunBaseKey, key, value);
        }

        private string GetRegistryValue(string key, object defaultValue)
        {
            return Registry.GetValue(BeamgunBaseKey, key, defaultValue)?.ToString();
        }
    }
}
