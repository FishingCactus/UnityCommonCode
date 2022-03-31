using System;

namespace FishingCactus 
{
    public class ArgumentHelpers
    {
        // -- PUBLIC

        public static bool FindArg(
            out string value,
            string name
            )
        {
            value = null;
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, name);
            if (index > -1 && args.Length > index + 1) {
                value = args[index + 1];
            }

            return index > -1;
        }

        public static string GetArg(
            string name
            )
        {
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, name);

            if (index > -1 && args.Length > index + 1) {
                return args[index + 1];
            }
            return null;
        }

        public static bool HasArg(
            string name
            )
        {
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, name);

            return index > -1;
        }
    }
}
