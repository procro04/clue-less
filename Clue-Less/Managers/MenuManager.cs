using Clue_Less.Enums;
using Clue_Less.Models.GameplayObjects;
using Models.GameplayObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Clue_Less.Managers
{
    public class MenuManager
    {
        private static readonly Lazy<MenuManager> lazy = new Lazy<MenuManager>(() => new MenuManager());
        public static MenuManager Instance { get { return lazy.Value; } }
        public MenuManager() { }

        public string GetMenuSelection(int menuSelection)
        {
            switch(menuSelection)
            {
                case (int)MenuOptionEnum.StartNewGame:
                    return "Start New Game?";
                case (int)MenuOptionEnum.JoinGame:
                    return "Join Game?";

                //Should these be their own enumerations
                //that launch the sub-menu instead?
                case (int)MenuOptionEnum.MakeSuggestion:
                    //Add logic to present suggestion options
                    //pehaps return a sub-menu??
                    return "Who would you like to suggest?";
                case (int)MenuOptionEnum.MakeAccusation:
                    //Add logic to present suggestion options
                    //pehaps return a sub-menu??
                    return "Who would you like to accuse?";
                default: 
                    return "Please make a selection.";
            }
        }

        public List<ClientMenuOption> GetMenuOptions(bool freshInstance = false)
        {
            var menuOptions = new List<ClientMenuOption>();

            if (freshInstance)
            {
                var startNewGame = new ClientMenuOption { Name = "Start New Game?", MenuSelection = (int)MenuOptionEnum.StartNewGame };
                var joinGame = new ClientMenuOption { Name = "Join Game?", MenuSelection = (int)MenuOptionEnum.JoinGame };

                menuOptions.Add(startNewGame);
                menuOptions.Add(joinGame);
            }

            var makeSuggestion = new ClientMenuOption { Name = "Make A Suggestion.", MenuSelection = (int)MenuOptionEnum.MakeSuggestion };
            var makeAccusation = new ClientMenuOption { Name = "Make An Accusation.", MenuSelection = (int)MenuOptionEnum.MakeAccusation };
            
            menuOptions.Add(makeSuggestion);
            menuOptions.Add(makeAccusation);   
            
            return menuOptions;
        }
    }
}
