using DevBridge.Templates.WebProject.DataEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBridge.Templates.WebProject.DataEntities.Mappings
{
    public class UsersSongsMap : EntityMapBase<UsersSongs>
    {
        public UsersSongsMap()
        {
            Map(m => m.SongID).Length(50).Not.Nullable();
            Map(m => m.UserID).Length(50).Not.Nullable();
        }
    }
}
