using System;
using System.Linq;
using System.Text;
using CommandLine;
using Localizer.Parsers;

namespace Xaml2Resx
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<SyncOptions>(args)
                .MapResult(
                  (SyncOptions opts) => RunAddAndReturnExitCode(opts),
                  errs => 1);
        }

        private static string GenerateId(string text)
        {
            var sb = new StringBuilder();
            foreach(var ch in text)
            {
                if (('a' <= ch && ch <= 'z') || ('A' <= ch && ch <= 'Z'))
                {
                    sb.Append(ch);
                }
                if(' ' == ch)
                {
                    sb.Append('_');
                }
            }
            if(sb.Length == 0)
            {
                sb.Append("loc");
            }

            return sb.ToString();
        }

        private static int RunAddAndReturnExitCode(SyncOptions opts)
        {
            var file_name = System.IO.Path.GetFileNameWithoutExtension(opts.Resx);
            var resx = ResxParser.Load(opts.Resx);
            var xaml = XamlParser.Load(opts.Xaml);
            foreach(var item in xaml.TextBlocks())
            {
                Console.WriteLine(item.Text);
                string id;
                if (!resx.TryGetText(item.Text, out id))
                {
                    id = GenerateId(item.Text);
                    if(resx.All().Any(x => x.Id == id))
                    {
                        for(int i = 1; i < int.MaxValue; ++i)
                        {
                            var c = id + i.ToString();
                            if (!resx.All().Any(x => x.Id == c))
                            {
                                id = c;
                                resx.AddText(c, item.Text);
                                break;
                            }
                        }
                    }
                    else
                    {
                        resx.AddText(id, item.Text);
                    }
                }
                item.Text = "{x:Static local:" + file_name +  "." + id + "}";
            }

            resx.Save();
            xaml.Save();

            return 0;
        }
    }
}
