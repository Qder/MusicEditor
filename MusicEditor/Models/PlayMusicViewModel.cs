using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicEditor.Models
{
    public class PlayMusicViewModel
    {
        public bool IsPlay { get; set; }
        public long From { get; set; }
        public long To { get; set; }
    }
}