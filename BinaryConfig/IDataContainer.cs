namespace BinaryConfig
{
	/// <summary>
	/// The base class of the <see cref="MiniDataContainer"/>.
	/// </summary>
	public interface IMiniDataContainer
	{
		void SetDataValue(string Value);

		string GetDataValue();
	}
}
