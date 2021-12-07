using System;

namespace Portaler.NET.Client.Visualiser
{
    public class Config
    {
        private static Config? _main;
        public static Config Main 
        { 
            get 
            {
                if (_main == null)
                    throw new Exception("Main config has not been set");

                return _main;
            } 
        }

        public float TimeStep { get; set; }
        public float Drag { get; set; }

        public float CenteralForce { get; set; }
        public float RebelForce { get; set; }
        public float LinkForce { get; set; }
        public float LinkLength { get; set; }

        public bool Debug { get; set; }

        public Config(float timeStep, float drag, float centeralForce, float rebelForce, float linkForce, float linkLength, bool debug)
        {
            TimeStep = timeStep;
            Drag = drag;
            CenteralForce = centeralForce;
            RebelForce = rebelForce;
            LinkForce = linkForce;
            LinkLength = linkLength;

            Debug = debug;
        }

        public void SetAsDefault()
        {
            _main = this;
        }
    }
}
