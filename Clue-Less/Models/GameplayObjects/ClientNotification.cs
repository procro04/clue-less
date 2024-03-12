using Clue_Less.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.GameplayObjects
{
    public class ClientNotification
    {
        public string Message { get; set; }
        public MenuOptionEnum MenuOptions { get; set; }
    }
}
