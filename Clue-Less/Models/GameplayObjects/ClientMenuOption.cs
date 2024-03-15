using Greet;

namespace Models.GameplayObjects
{
    public class ClientMenuOption
    {
        public string Name { get; set; }
        public MenuOptionEnum MenuSelection { get; set; }
        public bool IsSubMenuOption { get; set; }
    }
}
