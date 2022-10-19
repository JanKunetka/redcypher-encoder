namespace RedCipher.Models.Coder
{
    /// <summary>
    /// The different states of the coder proccess.
    /// </summary>
    public enum CoderState
    {
        /// <summary>
        /// The state of hiding specified information.
        /// </summary>
        Hiding,
        /// <summary>
        /// The state of adding extra 0s to comply with 
        /// </summary>
        FillingWithZeros
    }
}