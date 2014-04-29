using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBridge.Templates.WebProject.DataEntities.Entities
{
    public class SoundTracks : EntityBase<SoundTracks>
    {
        public virtual int ID { get; set; }
        public virtual string SoundTrackName { get; set; }
    }
}
