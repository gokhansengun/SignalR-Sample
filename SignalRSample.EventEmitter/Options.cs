using CommandLine;
using CommandLine.Text;

namespace SignalRSample.EventEmitter
{
    // Define a class to receive parsed values
    public class Options
    {
        [Option('g', "group", Required = true,
            HelpText = "Group Id.")]
        public string GroupId { get; set; }

        [Option('e', "event", Required = true,
            HelpText = "Event Type, CALL_ARRIVED, CALL_ANSWERED, etc.")]
        public bool EventType { get; set; }

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
