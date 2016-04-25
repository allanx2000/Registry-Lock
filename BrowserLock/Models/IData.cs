using Newtonsoft.Json.Linq;
using System;

namespace BrowserLock.Models
{
    public interface IData
    {
        string Name { get; }
        JObject ToJSON();
        string ClassType { get; }
    }
}