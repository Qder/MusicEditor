using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBridge.Templates.WebProject.DataEntities.Entities
{
    public class UsersSongs : EntityBase<UsersSongs>
    {
        public virtual int SongID { get; set; }
        public virtual int UserID { get; set; }
    }
}
