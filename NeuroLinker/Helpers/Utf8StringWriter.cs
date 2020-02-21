using System.IO;
using System.Text;

namespace NeuroLinker.Helpers
{
    /// <summary>
    /// <see cref="StringWriter"/> that uses UTF8
    /// </summary>
    public class Utf8StringWriter : StringWriter
    {
        #region Properties

        /// <summary>
        /// The encoding to use for writing out the strings.
        /// </summary>
        public override Encoding Encoding => Encoding.UTF8;

        #endregion
    }
}