// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Requirements for an object that uses a discovery trigger.
    /// </summary>
    public interface IDiscovering
    {
        /// <summary>
        /// Collection of types tracked by the DiscoveryTrigger.
        /// </summary>
        public DiscoveryType[] DiscoveryTypes { get; set; }
        
        /// <summary>
        /// Tracking/discovery trigger.
        /// </summary>
        public DiscoveryTrigger DiscoveryTrigger { get; set; }
    }
}