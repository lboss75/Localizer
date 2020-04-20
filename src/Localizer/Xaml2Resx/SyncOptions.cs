using CommandLine;

namespace Xaml2Resx
{
    [Verb("sync", HelpText = "Sync XAML file to the RESX file.")]
    internal class SyncOptions
    {
        [Value(0, HelpText = "path to XAML file")]
        public string Xaml { get; set; }

        [Value(1, HelpText = "path to Resx file")]
        public string Resx { get; set; }
    }
}