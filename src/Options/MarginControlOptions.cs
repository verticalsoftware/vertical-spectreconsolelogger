using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Options
{
    public class MarginControlOptions
    {
        public const string MarginControlCaptureGroup = "mg";
        public const string MarginSetCaptureGroup = "mgset";
        
        public MarginControlOptions(Match? match = null)
        {
            if (match == null) 
                return;

            Margin = match.Groups[MarginControlCaptureGroup].Success
                ? int.Parse(match.Groups[MarginControlCaptureGroup].Value)
                : null;
            SetMargin = match.Groups[MarginSetCaptureGroup].Success;
        }   
        
        /// <summary>
        /// Gets the margin value.
        /// </summary>
        public int? Margin { get; set; }
        
        /// <summary>
        /// Gets whether the margin should be set.
        /// </summary>
        public bool SetMargin { get; set; }
    }
}