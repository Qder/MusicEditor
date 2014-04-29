﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBridge.Templates.WebProject.DataEntities.Entities
{
    public class User : EntityBase<User>
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual int ID { get; set; }
    }
}
