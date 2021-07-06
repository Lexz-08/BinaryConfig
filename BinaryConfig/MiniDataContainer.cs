using System;

namespace BinaryConfig
{
	/// <summary>
	/// Holds a single string value.<br/><br/>
	/// Use this if you don't want to store multiple string values in a single file.
	/// </summary>
	[Serializable]
	public class MiniDataContainer : IMiniDataContainer, IBaseDataContainer
	{
		private string containerName;
		private string value;

		/// <summary>
		/// Creates a new <strong><see langword="hidden"/></strong> <see cref="MiniDataContainer"/> object.
		/// </summary>
		/// <param name="ContainerName">The file name of the container when serialized.</param>
		/// <param name="Value">The single string value to serialize.</param>
		internal MiniDataContainer(string ContainerName, string Value)
		{
			containerName = ContainerName;
			value = Value;
		}

		/// <summary>
		/// Creates a new <see cref="MiniDataContainer"/> object.
		/// </summary>
		/// <param name="OutputFile">The file to create.</param>
		/// <returns>A new <see cref="MiniDataContainer"/> that can be modified with <see cref="SetDataValue(string)"/>.</returns>
		public static MiniDataContainer CreateContainer(string OutputFile)
		{
			return new MiniDataContainer(OutputFile, string.Empty);
		}

		/// <summary>
		/// Creates a new <see cref="MiniDataContainer"/> object.
		/// </summary>
		/// <param name="OutputFile">The file to create.</param>
		/// <param name="Value">The value to serialize while creating the file.</param>
		/// <returns>A new <see cref="MiniDataContainer"/> that can be modified with <see cref="SetDataValue(string)"/>.</returns>
		public static MiniDataContainer CreateContainer(string OutputFile, string Value)
		{
			return new MiniDataContainer(OutputFile, Value);
		}

		/// <summary>
		/// Modifies the single string data value by replacing it with another.
		/// </summary>
		/// <param name="Value">The new value to replace the old one.</param>
		public void SetDataValue(string Value)
		{
			value = Value;
		}

		/// <summary>
		/// Returns the single string value.
		/// </summary>
		/// <param name="Value"></param>
		public string GetDataValue()
		{
			return value;
		}

		/// <summary>
		/// The name of the <see cref="MiniDataContainer"/> and the file itself in the device.
		/// </summary>
		public string ContainerName => containerName;

		/// <summary>
		/// Returns the string value contained within the <see cref="MiniDataContainer"/>.
		/// </summary>
		/// <returns>The string value of this <see cref="MiniDataContainer"/>.</returns>
		public override string ToString()
		{
			return value;
		}

		/// <summary>
		/// Used for the <see cref="EncryptionHandler"/> to make all of the <em>MiniDataContainers</em> encrypt in the same file.
		/// </summary>
		internal string Value => value;
	}
}
