using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Frontend.Environment
{
  public interface IEnvironmentHandler
  {
    void Attach(IEnvironmentManager environmentManager);
    void Detach(IEnvironmentManager environmentManager);
  }
}
