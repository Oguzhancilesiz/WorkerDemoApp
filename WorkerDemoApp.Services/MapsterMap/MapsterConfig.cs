using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDemoApp.Services.MapsterMap
{
    public static class MapsterConfig
    {
        private static bool _inited;
        public static void Register()
        {
            if (_inited) return;
            _inited = true;
        }
    }
}
