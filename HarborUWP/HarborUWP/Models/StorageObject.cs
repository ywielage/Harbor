using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborUWP.Models
{
    public class StorageObject
    {
        public int Amount { get; set; }

        public StorageObject(int amount)
        {
            Amount = amount;
        }
    }
}
