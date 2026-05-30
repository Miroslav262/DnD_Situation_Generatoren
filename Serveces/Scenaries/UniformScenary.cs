using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dndsitgen.Serveces.Scenaries
{
    public class UniformScenary: Scenary
    {
        public override float g(int i) { return 1f; }
    }
}
