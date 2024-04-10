namespace PoliceCrimeViewer
{
    public class CrimeSummary
    {
        public string Category { get; set; }
        public int Count { get; set; }

        public CrimeSummary(string category, int count)
        {
            Category = category;
            Count = count;
        }
    }
}
