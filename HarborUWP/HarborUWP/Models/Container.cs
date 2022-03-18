using HarborApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborApp.Models
{
    internal class Container
    {
        public int Id { get; set; }
        public ContainerItemType ContainerItemType { get; set; }

        public Container(int id, ContainerItemType containerItemType)
        {
            Id = id;
            ContainerItemType = containerItemType;
        }
    }
}
