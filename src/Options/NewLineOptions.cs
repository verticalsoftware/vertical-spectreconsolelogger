using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Options
{
    public class NewLineOptions : MarginControlOptions
    {
        public const string ConditionalCaptureGroup = "lfif";
        public const string NewLineCaptureGroup = "lf";
        
        public NewLineOptions(Match? match = null) : base(match)
        {
            if (match == null)
                return;
            
            NewLineIsConditional = match.Groups[ConditionalCaptureGroup].Success;
            WriteNewLine = match.Groups[NewLineCaptureGroup].Success;
        }
        
        /// <summary>
        /// Whether to write line.
        /// </summary>
        public bool WriteNewLine { get; set; }
        
        /// <summary>
        /// Gets whether the new line is conditional on the write buffer
        /// not being in the margin position.
        /// </summary>
        public bool NewLineIsConditional { get; set; }
    }
}