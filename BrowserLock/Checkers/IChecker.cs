using BrowserLock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserLock.Checkers
{
    public interface IChecker
    {
        string ID { get; }
        string FriendlyName { get; }

        //Has a Path value in ExtensionsInfo
        bool HasPath { get; }

        //Has a Extension value ...
        bool HasExtensions { get; }
        RuleInfo GetValues(string path);
        List<string> SupportedExtensions { get; }
        void WriteData(RuleInfo data); //Write to Registry, should be pretty standard?
    }
}
