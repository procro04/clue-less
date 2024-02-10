using Models.GameplayObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clue_Less.Managers.Interfaces
{
    public interface ITokenManager
    {
        List<Player> InitializePlayers();
        List<Weapon> InitializeWeapons();

    }
}

