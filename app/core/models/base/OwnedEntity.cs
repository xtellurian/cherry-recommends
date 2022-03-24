namespace SignalBox.Core
{
    /// <summary>
    /// The base class for all Owned Entities in the database
    /// Independant Entities should use Entity as the base class.
    /// </summary>
    public abstract class OwnedEntity
    {
        protected OwnedEntity()
        { }

        /// <summary>
        /// The primary key.
        /// </summary>
        public long Id { get; set; } // set when created
    }
}