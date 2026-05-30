using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dndsitgen.Serveces.Scenaries
{
    public class BossScenary : Scenary
    {
        public override float g(int i) { return (i+1)*(i+1); }
    }
}
