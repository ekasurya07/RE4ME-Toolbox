using System.Collections.Generic;
using System.Linq;

namespace BinExtractor.Models
{
    public class BinFile
    {
        public string FileName { get; set; }
        public List<TextEntry> Entries { get; set; } = new List<TextEntry>();

        public IEnumerable<List<TextEntry>> GetEntriesGrouped(int groupSize = 6)
        {
            for (int i = 0; i < Entries.Count; i += groupSize)
            {
                yield return Entries.Skip(i).Take(groupSize).ToList();
            }
        }
    }
}