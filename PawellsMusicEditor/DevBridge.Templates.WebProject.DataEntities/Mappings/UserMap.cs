using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevBridge.Templates.WebProject.DataEntities.Entities;

namespace DevBridge.Templates.WebProject.DataEntities.Mappings
{
    public class UserMap : EntityMapBase<User>
    {
        public UserMap()
        {
            Map(m => m.FirstName).Length(100).Not.Nullable();
            Map(m => m.LastName).Length(50).Not.Nullable();
            Map(m => m.Email).Length(100).Not.Nullable();
            Map(m => m.UserName).Length(50).Not.Nullable();
            Map(m => m.Password).Length(50).Not.Nullable();

            HasManyToMany(m => m.Tracks)
                .Cascade.AllDeleteOrphan();

        }
    }
}