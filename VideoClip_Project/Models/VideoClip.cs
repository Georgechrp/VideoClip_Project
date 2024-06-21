using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoClip_Project.Models
{
    public class VideoClip
    {
        public string Title { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
