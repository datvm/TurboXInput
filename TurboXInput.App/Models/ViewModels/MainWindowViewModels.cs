using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboXInput.App.Models.ViewModels
{

    public class GameControllerViewModel
    {


        public uint Id { get; set; }
        public string Name { get; set; }

        public GameControllerViewModel()
        {
        }

        public GameControllerViewModel(uint id, string name)
        {
            this.Id = id;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override string ToString()
        {
            return this.Name;
        }
    }


}
