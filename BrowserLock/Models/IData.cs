using Newtonsoft.Json.Linq;
using System;

namespace BrowserLock.Models
{
    public interface IData
    {
        string Name { get; }
        JObject ToJSON();
        string ClassType { get; } //Used to tell apart Folder from Value during deserialization
    }
}