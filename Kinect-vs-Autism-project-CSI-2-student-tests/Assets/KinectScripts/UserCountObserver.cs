using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.KinectScripts
{
   public interface UserObserver
    {
       void updateUserPositions(int userCount);
    }
}
