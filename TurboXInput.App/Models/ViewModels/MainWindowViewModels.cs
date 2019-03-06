using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboXInput.App.Models.ViewModels
{

    public class XBoxControllerViewModel
    {


        public uint Id { get; set; }
        public string Name { get; set; }

        public XBoxControllerViewModel()
        {
        }

        public XBoxControllerViewModel(uint id, string name)
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
