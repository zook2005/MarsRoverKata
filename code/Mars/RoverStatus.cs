namespace Mars
{
    public class RoverStatus
    {
        public enum RoverStatusCode
        {
            Ok,
            Fail,
            Error
        }

        internal string StatusMessage { get; set; }
        internal RoverStatusCode StatusCode { get; set; }

        public RoverStatus() : this(RoverStatusCode.Ok)
        {
            StatusMessage = "OK";
        }

        public RoverStatus(RoverStatusCode code)
        {
            this.StatusCode = code;
        }
    }
}