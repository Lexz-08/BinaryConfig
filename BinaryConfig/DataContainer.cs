using System;
using System.Collections.Generic;

namespace BinaryConfig
{
	/// <summary>
	/// The <em><strong>main data container</strong></em> in this assembly.
	/// </summary>
	[Serializable]
	public class DataContainer : IBaseDataContainer
	{
		/// <summary>
		/// Represents an empty <see cref="DataContainer"/>.
		/// </summary>
		public static readonly DataContainer Empty;

		private string containerName;
		private List<SubDataContainer> subContainers;

		/// <summary>
		/// Creates a new <strong><see langword="hidden"/></strong> <see cref="DataContainer"/> object.
		/// </summary>
		/// <param name="ContainerName">The file name of the container when serialized.</param>
		/// <param name="SubContainers">The sub containers that contain the actual values to be serialized.</param>
		internal DataContainer(string ContainerName, List<SubDataContainer> SubContainers)
		{
			containerName = ContainerName;

			if (SubContainers != null)
			{
				if (SubContainers.Count == 0)
				{
					List<SubDataContainer> newList = new List<SubDataContainer>();
					foreach (SubDataContainer sdc in SubContainers)
					{
						SubDataContainer newSdc = new SubDataContainer(sdc.GetSubDataValue());
						newSdc.ParentDataContainer = this;
						newList.Add(newSdc);
					}
					subContainers = newList;
				}
				else throw EmptySubContainerListException;
			}
			else throw EmptySubContainerListException;
		}

		/// <summary>
		/// Creates a new <see cref="DataContainer"/> object.
		/// </summary>
		/// <param name="OutputFile">The file to create.</param>
		/// <returns>A new <see cref="DataContainer"/> without any <em>SubDataContainers</em>.</returns>
		public static DataContainer CreateContainer(string OutputFile)
		{
			return new DataContainer(OutputFile, new List<SubDataContainer>
			{
				SubDataContainer.CreateContainer(),
			});
		}

		/// <summary>
		/// Creates a new <see cref="DataContainer"/> object.
		/// </summary>
		/// <param name="OutputFile">The file to create.</param>
		/// <param name="SubContainers">The <em>SubDataContainers</em> within this <see cref="DataContainer"/>.</param>
		/// <returns>A new <see cref="DataContainer"/> with <em>SubDataContainers</em>.</returns>
		public static DataContainer CreateContainer(string OutputFile, List<SubDataContainer> SubContainers)
		{
			return new DataContainer(OutputFile, SubContainers);
		}

		/// <summary>
		/// Modifies the sub value of the <see cref="SubDataContainer"/> at the <paramref name="SubContainerIndex"/> position.
		/// </summary>
		/// <param name="SubContainerIndex">The zero-based index position of the <see cref="SubDataContainer"/>.</param>
		/// <param name="Value">The new sub value of the <see cref="SubDataContainer"/>.</param>
		public void SetDataValue(int SubContainerIndex, string Value)
		{
			if (subContainers.Count > 0)
			{
				if (subContainers.Count - 1 >= SubContainerIndex)
				{
					subContainers[SubContainerIndex].SetSubDataValue(Value);
				}
				else throw ContainerTooSmallException;
			}
			else throw EmptyContainerException;
		}

		/// <summary>
		/// Retrieves the <see cref="SubDataContainer"/> at the specified <paramref name="SubContainerIndex"/> position.
		/// </summary>
		/// <param name="SubContainerIndex">The zero-based index position of the <see cref="SubDataContainer"/>.</param>
		/// <returns>The <see cref="SubDataContainer"/> at the <paramref name="SubContainerIndex"/> position.</returns>
		public SubDataContainer GetDataContainer(int SubContainerIndex)
		{
			if (subContainers.Count > 0)
			{
				if (subContainers.Count - 1 >= SubContainerIndex)
				{
					return subContainers[SubContainerIndex];
				}
				else throw ContainerTooSmallException;
			}
			else throw EmptyContainerException;
		}

		/// <summary>
		/// Retrieves the string value of the <see cref="SubDataContainer"/> at the specified <paramref name="SubContainerIndex"/> position.
		/// </summary>
		/// <param name="SubContainerIndex">The zero-based index position of the <see cref="SubDataContainer"/> of which to retrieve the string value.</param>
		/// <returns>The string value of the <see cref="SubDataContainer"/> at the specified <paramref name="SubContainerIndex"/> position.</returns>
		public string GetDataValue(int SubContainerIndex)
		{
			if (subContainers.Count > 0)
			{
				if (subContainers.Count - 1 >= SubContainerIndex)
				{
					return subContainers[SubContainerIndex].GetSubDataValue();
				}
				else throw ContainerTooSmallException;
			}
			else throw EmptyContainerException;
		}

		/// <summary>
		/// The name of the <see cref="DataContainer"/> and the file itself in the device.
		/// </summary>
		public string ContainerName => containerName;

		/// <summary>
		/// The <see cref="IndexOutOfRangeException"/> that is thrown if the current <see cref="DataContainer"/> is empty.
		/// </summary>
		private IndexOutOfRangeException EmptyContainerException => new IndexOutOfRangeException("Cannot access SubDataContainer because parent DataContainer is empty.");

		/// <summary>
		/// The <see cref="IndexOutOfRangeException"/> that is thrown if the current <see cref="DataContainer"/>'s list of <em>SubDataContainers</em> is too small.
		/// </summary>
		private IndexOutOfRangeException ContainerTooSmallException => new IndexOutOfRangeException("Cannot access SubDataContainer because the size of the SubDataContainer list is too small.");

		/// <summary>
		/// The <see cref="ArgumentNullException"/> that is thrown when the given <em>SubDataContainers</em> in the constructor don't actually exist.
		/// </summary>
		private ArgumentNullException EmptySubContainerListException => new ArgumentNullException("SubContainers", "Cannot add new SubDataContainers to DataContainers because there are no SubDataContainers to add.");

		/// <summary>
		/// Used for the <see cref="EncryptionHandler"/> to make all of the <em>DataContainers</em> encrypt in the same file.
		/// </summary>
		internal List<SubDataContainer> SubContainers => subContainers;

		/// <summary>
		/// Converts this <see cref="DataContainer"/> to a list of <em>MiniDataContainers</em>.
		/// </summary>
		/// <returns>The converted list of <em>MiniDataContainers</em>.</returns>
		public List<MiniDataContainer> ToMDCList()
		{
			List<MiniDataContainer> mdcs = new List<MiniDataContainer>();
			foreach (SubDataContainer sdc in subContainers)
			{
				mdcs.Add(MiniDataContainer.CreateContainer(containerName, sdc.GetSubDataValue()));
			}
			return mdcs;
		}
	}
}
