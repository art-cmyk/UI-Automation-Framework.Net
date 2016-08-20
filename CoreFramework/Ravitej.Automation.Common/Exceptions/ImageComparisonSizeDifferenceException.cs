using System;
using System.Runtime.Serialization;

namespace Ravitej.Automation.Common.Exceptions
{
    public class ImageComparisonSizeDifferenceException : Exception
    {
        public ImageComparisonSizeDifferenceException()
        {
        }

        public ImageComparisonSizeDifferenceException(string message) : base(message)
        {
        }

        public ImageComparisonSizeDifferenceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ImageComparisonSizeDifferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ImageComparisonSizeDifferenceException(string sourceImagePath, string targetImagePath) : base("Images are of different sizes")
        {

        }
    }
}
