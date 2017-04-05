using CommandLine;
using CommandLine.Text;

namespace SignalRSample.ClientConsole
{
    public class Options
    {
        [Option('e', "extension", Required = true,
            HelpText = "Extension Id.")]
        public string ExtensionId { get; set; }
        
        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}