using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.KinectScripts
{
    public interface UserSubject
    {
         void addObserver(UserObserver newObserver);
         void notifyObserver();
    }
}
