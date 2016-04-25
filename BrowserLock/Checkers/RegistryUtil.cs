using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrowserLock.Models;
using Microsoft.Win32;

namespace BrowserLock.Checkers
{
    public class RegistryUtil
    {
        static RegistryUtil()
        {
        }

        public static List<IData> GetData(RegistryKey key)
        {
            List<IData> data = new List<IData>();

            var subkeys = key.GetSubKeyNames();
            var values = key.GetValueNames();

            foreach (var name in values)
            {
                object value = key.GetValue(name);
                var kind = key.GetValueKind(name);

                Value v = new Value(name, kind, value);
                data.Add(v);
            }

            foreach (var sk in subkeys)
            {
                var children = GetData(key.OpenSubKey(sk));
                Folder folder = new Folder(sk, children);
                data.Add(folder);
            }

            return data;
        }

        private const string HKEY_CLASSES_ROOT = "HKEY_CLASSES_ROOT";
        private const string HKEY_CURRENT_USER = "HKEY_CURRENT_USER";
        private const string HKEY_LOCAL_MACHINE = "HKEY_LOCAL_MACHINE";
        private const string HKEY_USERS = "HKEY_USERS";
        private const string HKEY_CURRENT_CONFIG = "HKEY_CURRENT_CONFIG";

        public static RegistryKey GetKey(string path)
        {
            int splitAt = path.IndexOf("\\");
            string head = path.Substring(0, splitAt);
            string tail = path.Substring(splitAt + 1);

            RegistryKey key;

            switch (head)
            {
                case HKEY_CLASSES_ROOT:
                    key = Registry.ClassesRoot;
                    break;
                case HKEY_CURRENT_CONFIG:
                    key = Registry.CurrentConfig;
                    break;
                case HKEY_CURRENT_USER:
                    key = Registry.CurrentUser;
                    break;
                case HKEY_LOCAL_MACHINE:
                    key = Registry.LocalMachine;
                    break;
                case HKEY_USERS:
                    key = Registry.Users;
                    break;
                default:
                    throw new NotSupportedException("Key not supported: " + head);
            }

            return key.OpenSubKey(tail);            
        }

        internal static bool IsValid(string path)
        {
            throw new NotImplementedException();
        }
    }
}
