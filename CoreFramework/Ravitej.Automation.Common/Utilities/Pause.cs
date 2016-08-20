namespace Ravitej.Automation.Common.Utilities
{
    /// <summary>
    /// Static class to pause the script execution for a given duration
    /// </summary>
    public static class Pause
    {
        /// <summary>
        /// How long should the pause be for
        /// </summary>
        public enum ScriptWaitDuration
        {
            /// <summary>
            /// No delay
            /// </summary>
            None,

            /// <summary>
            /// Suspends the test flow for quarter of a second (250 miliseconds)
            /// </summary>
            Tiny,

            /// <summary>
            /// Suspends the test flow for half a second (500 milliseconds)
            /// </summary>
            VeryShort,

            /// <summary>
            /// Suspends the test flow for one second (1 second)
            /// </summary>
            Short,

            /// <summary>
            /// Suspends the test flow for two seconds (2 seconds)
            /// </summary>
            Medium,

            /// <summary>
            /// Suspends the test flow for just over three seconds (3.33 seconds)
            /// </summary>
            Long,

            /// <summary>
            /// Suspends the test flow for five seconds (5 seconds)
            /// </summary>
            VeryLong
        }

        /// <summary>
        /// Pause the script execution
        /// </summary>
        /// <param name="duration"></param>
        public static void PauseExecution(ScriptWaitDuration duration)
        {
            var pauseDuration = _GetDelayFrom(duration);
            System.Threading.Thread.Sleep(pauseDuration);
        }

        public static void PauseExecution(int customDuration)
        {
            System.Threading.Thread.Sleep(customDuration);
        }

        private static int _GetDelayFrom(ScriptWaitDuration duration)
        {
            const int maxDuration = 5000;
            int pauseDuration;

            switch (duration)
            {
                case ScriptWaitDuration.None:
                {
                    pauseDuration = 0;
                    break;
                }
                case ScriptWaitDuration.Tiny:
                {
                    pauseDuration = maxDuration/20;
                    break;
                }
                case ScriptWaitDuration.VeryShort:
                {
                    pauseDuration = maxDuration/10;
                    break;
                }
                case ScriptWaitDuration.Short:
                {
                    pauseDuration = maxDuration/5;
                    break;
                }
                case ScriptWaitDuration.Medium:
                {
                    pauseDuration = (int) (maxDuration/2.5);
                    break;
                }
                case ScriptWaitDuration.Long:
                {
                    pauseDuration = (int) (maxDuration/1.5);
                    break;
                }
                case ScriptWaitDuration.VeryLong:
                {
                    pauseDuration = maxDuration/1;
                    break;
                }
                default:
                {
                    // Untestable scenario - consider throwing Argument exception instead
                    // throw new System.ArgumentException("Please define ScriptWaitDuration", "duration");
                    pauseDuration = 500;
                    break;
                }
            }
            return pauseDuration;
        }
    }
}
