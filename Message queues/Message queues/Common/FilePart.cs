using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FilePart
    {
        public Guid Id { get; set; }

        public byte[] Buffer { get; set; }

        public string FileName { get; set; }

        public int PartPosition { get; set; }

        public int PartsCount { get; set; }

        public int FileSize { get; set; }
    }
}
