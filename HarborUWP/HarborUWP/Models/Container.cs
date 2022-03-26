﻿using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models
{
    internal class Container
    {
        public int Id { get; set; }
        public int maxSize { get; set; }
        public int curSize { get; set; }
        public ContainerItemType ContainerItemType { get; set; }

        public Container(int id, ContainerItemType containerItemType, int curSize)
        {
            Id = id;
            ContainerItemType = containerItemType;
            maxSize = 4000;
        }
    }
}
