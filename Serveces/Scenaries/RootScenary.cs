using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dndsitgen.Serveces.Scenaries
{
    public class RootScenary : Scenary
    {
        public override float g(int i) { return (float)Math.Sqrt(i+1); }
    }
}
