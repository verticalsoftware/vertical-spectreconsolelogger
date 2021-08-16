namespace Vertical.SpectreLogger.Core
{
    public class PositionControl
    {
        public MarginControlMode MarginMode { get; set; }
        
        public int Margin { get; set; }
        
        public bool MarginReset { get; set; }
        
        public NewLineMode NewLineMode { get; set; }
        
        public int NewLineCount { get; set; }
    }
}