namespace BinaryConfig
{
	/// <summary>
	/// The base class of both the <see cref="DataContainer"/> and the <see cref="MiniDataContainer"/>.
	/// </summary>
	public interface IBaseDataContainer
	{
		string ContainerName { get; }
	}
}
