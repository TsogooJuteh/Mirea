using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace TrapezoidExample
{
    public static class Program
    {
        public static void Main()
        {
            //window settings.
            var nativeSettings = new NativeWindowSettings
            {
                Size = new Vector2i(1000, 1000),
                Title = "Trapezoid with Lighting",
                APIVersion = new Version(3, 2),
                Profile = ContextProfile.Compatability
            };

            using (var game = new Engine(GameWindowSettings.Default, nativeSettings))
            {
                game.Run();
            }
        }
    }
}
