using System;

namespace BinaryConfig
{
	/// <summary>
	/// A sub-version of the <see cref="DataContainer"/> struct.<br/>
	/// Only type of data container that can be stored in a list of data containers in the <see cref="DataContainer"/>.<br/><br/>
	/// Use this if you want to store multiple values in a single file.
	/// </summary>
	[Serializable]
	public class SubDataContainer : ISubDataContainer
	{
		private DataContainer parentContainer;
		private string subValue;

		/// <summary>
		/// Creates a new <strong><see langword="hidden"/></strong> <see cref="SubDataContainer"/> object.
		/// </summary>
		/// <param name="ParentContainer">The parent <see cref="DataContainer"/> of this <see cref="SubDataContainer"/>.</param>
		/// <param name="SubValue">The single string value to serialize.</param>
		internal SubDataContainer(string SubValue)
		{
			parentContainer = DataContainer.Empty;
			subValue = SubValue;
		}

		/// <summary>
		/// Creates a new <see cref="SubDataContainer"/> object.
		/// </summary>
		/// <param name="ParentContainer">The parent <see cref="DataContainer"/> of this <see cref="SubDataContainer"/>.</param>
		/// <returns>A new <see cref="SubDataContainer"/> contained in a <strong>parent <see cref="DataContainer"/></strong> -> <paramref name="ParentContainer"/>.</returns>
		public static SubDataContainer CreateContainer()
		{
			return new SubDataContainer(string.Empty);
		}

		/// <summary>
		/// Creates a new <see cref="SubDataContainer"/> object.
		/// </summary>
		/// <param name="ParentContainer">The parent <see cref="DataContainer"/> of this <see cref="SubDataContainer"/>.</param>
		/// <param name="SubValue">The value to serialize while putting in the <strong>parent <see cref="DataContainer"/></strong> -> <paramref name="ParentContainer"/>.</param>
		/// <returns>A new <see cref="SubDataContainer"/> contained in a <strong>parent <see cref="DataContainer"/></strong> -> <paramref name="ParentContainer"/>.</returns>
		public static SubDataContainer CreateContainer(string SubValue)
		{
			return new SubDataContainer(SubValue);
		}

		/// <summary>
		/// Modifies the single string data value by replacing it with another.
		/// </summary>
		/// <param name="SubValue">The new sub value to replace the old one.</param>
		public void SetSubDataValue(string SubValue)
		{
			subValue = SubValue;
		}

		/// <summary>
		/// Returns the single string value.
		/// </summary>
		/// <returns></returns>
		public string GetSubDataValue()
		{
			return subValue;
		}

		/// <summary>
		/// The parent <see cref="DataContainer"/> of this <see cref="SubDataContainer"/>.
		/// </summary>
		public DataContainer ParentContainer => parentContainer;

		/// <summary>
		/// A settable property allowing a <see cref="DataContainer"/> to modify the current <see cref="ParentContainer"/> of this <see cref="SubDataContainer"/>.
		/// </summary>
		internal DataContainer ParentDataContainer
		{
			get { return parentContainer; }
			set { parentContainer = value; }
		}

		/// <summary>
		/// Returns the string value contained within the <see cref="SubDataContainer"/>.
		/// </summary>
		/// <returns>The string value of this <see cref="SubDataContainer"/>.</returns>
		public override string ToString()
		{
			return subValue;
		}
	}
}
