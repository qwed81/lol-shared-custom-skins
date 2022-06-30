namespace SCSClient.Types
{
    public static class PersonStatus
    {

        public static readonly string Ok = "ok";
        public static readonly string Error = "error";
        public static readonly string Loading = "loading";
        public static readonly string Patching = "patching";

    }

    public struct ReactPerson
    {

        public string? username { get; set; }

        public string? status { get; set; }

        public string? imagePath { get; set; }

        public string uniqueId { get; set; }

        public ReactPerson(string? username, string? status, string? imagePath, string uniqueId)
        {
            this.username = username;
            this.status = status;
            this.imagePath = imagePath;
            this.uniqueId = uniqueId;
        }

    }
}
