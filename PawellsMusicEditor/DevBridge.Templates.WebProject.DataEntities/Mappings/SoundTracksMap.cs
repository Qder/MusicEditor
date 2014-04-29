using DevBridge.Templates.WebProject.DataEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBridge.Templates.WebProject.DataEntities.Mappings
{
    public class SoundTracksMap : EntityMapBase<SoundTracks>
    {
        public SoundTracksMap()
        {
            Map(m => m.ID).Length(50).Not.Nullable();
            Map(m => m.SoundTrackName).Length(50).Not.Nullable();
        }
    }
}
