using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models.GameplayObjects
{
    public class ClientWeapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TokenId { get; set; }
        public Vector2 TokenLocation { get; set; }
    }
}
