﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Movie247.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
