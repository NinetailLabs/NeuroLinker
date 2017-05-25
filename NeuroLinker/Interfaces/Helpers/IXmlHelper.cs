namespace NeuroLinker.Interfaces.Helpers
{
    /// <summary>
    /// Assist in manipulating XML
    /// </summary>
    public interface IXmlHelper
    {
        #region Public Methods

        /// <summary>
        /// Serialize an object to XML
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="data">Data that should be serialized</param>
        /// <returns>Serialized object</returns>
        string SerializeData<T>(T data);

        #endregion
    }
}