using HarborApp.Models.ShipTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarborApp.Models
{
    internal class Controller
    {
        public Harbor Harbor { get; set; }
        public List<Ship> Ships { get; set; }

        public Controller(Harbor harbor)
        {
            Harbor = harbor;
            Ships = new();
        }

        public void UpdateShips()
        {
            throw new NotImplementedException();
        }
    }
}
