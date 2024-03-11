using Clue_Less.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clue_Less.Models.GameplayObjects
{
    public class ClientMenuOption
    {
        public string Name { get; set; }
        public int MenuSelection { get; set; }
        public bool IsSubMenuOption { get; set; }
    }
}
