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

        public override Encoding Encoding => Encoding.UTF8;

        #endregion
    }
}