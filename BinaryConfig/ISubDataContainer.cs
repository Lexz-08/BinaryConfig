namespace BinaryConfig
{
	/// <summary>
	/// The base class of the <see cref="SubDataContainer"/>.
	/// </summary>
	public interface ISubDataContainer
	{
		DataContainer ParentContainer { get; }
	}
}
