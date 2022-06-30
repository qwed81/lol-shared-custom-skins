namespace SCSClient.Types
{
    public struct ReactMod
    {

        public string description { get; set; }

        public string name { get; set; }

        public string author { get; set; }

        public string providerName { get; set; }

        public string imagePath { get; set; }

        public string fileHash { get; set; }

        public int downloadingPercentage { get; set; }

        public bool active { get; set; }

        public ReactMod(string description, string name, string author, string providerName,
            string imagePath, string fileHash, int downloadingPercentage, bool active)
        {
            this.description = description;
            this.name = name;
            this.author = author;
            this.providerName = providerName;
            this.imagePath = imagePath;
            this.fileHash = fileHash;
            this.downloadingPercentage = downloadingPercentage;
            this.active = active;
        }

    }
}
