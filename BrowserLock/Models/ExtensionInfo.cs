using System.Collections.Generic;

namespace BrowserLock.Models
{
    public class ExtensionInfo
    {
        public string CheckerId { get; set; }

        //First is Folder with full path
        public List<IData> Data { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }

        public ExtensionInfo(string checkerId, string name, List<IData> data)
        {
            this.CheckerId = checkerId;
            this.Data = data;
            this.Name = name;
        }
    }
}