using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinaryConfig
{
	/// <summary>
	/// Allows for <em><strong>encryption</strong></em> and <em><strong>decryption</strong></em> of the config files generated using the <strong>data containers</strong>.
	/// </summary>
	public class EncryptionHandler
	{
		private List<DataContainer> containers;
		private List<MiniDataContainer> miniContainers;

		/// <summary>
		/// Holds the containers to be encrypted which can then be retrieved later.
		/// </summary>
		[Serializable]
		private struct ContainerHolder
		{
			private List<DataContainer> containers;
			private List<MiniDataContainer> miniContainers;

			/// <summary>
			/// Creates a new <see cref="ContainerHolder"/> object.
			/// </summary>
			/// <param name="Containers">The <em>DataContainers</em> to hold.</param>
			public ContainerHolder(List<DataContainer> Containers)
			{
				containers = Containers;
				miniContainers = new List<MiniDataContainer>();
			}

			/// <summary>
			/// Creates a new <see cref="ContainerHolder"/> object.
			/// </summary>
			/// <param name="MiniContainers">The <em>MiniContainers</em> to hold.</param>
			public ContainerHolder(List<MiniDataContainer> MiniContainers)
			{
				containers = new List<DataContainer>();
				miniContainers = MiniContainers;
			}

			/// <summary>
			/// Retrieves the list of <em>DataContainers</em>.
			/// </summary>
			/// <returns>The list of <em>DataContainers</em>.</returns>
			public List<DataContainer> GetContainers() => containers;

			/// <summary>
			/// Retrieves the list of <em>MiniDataContainers</em>.
			/// </summary>
			/// <returns>The list of <em>MiniDataContainers</em>.</returns>
			public List<MiniDataContainer> GetMiniContainers() => miniContainers;

			/// <summary>
			/// Creates a new <see cref="ContainerHolder"/> object.
			/// </summary>
			/// <param name="Containers">The <em>DataContainers</em> to hold.</param>
			/// <returns>A new <see cref="ContainerHolder"/>.</returns>
			public static ContainerHolder CreateHolder(List<DataContainer> Containers)
			{
				return new ContainerHolder(Containers);
			}

			/// <summary>
			/// Creates a new <see cref="ContainerHolder"/> object.
			/// </summary>
			/// <param name="MiniContainers">The <em>MiniDataContainers</em> to hold.</param>
			/// <returns>A new <see cref="ContainerHolder"/>.</returns>
			public static ContainerHolder CreateHolder(List<MiniDataContainer> MiniContainers)
			{
				return new ContainerHolder(MiniContainers);
			}
		}

		/// <summary>
		/// Creates a new <see cref="EncryptionHandler"/> object.
		/// </summary>
		/// <param name="Container">The <see cref="DataContainer"/> to encrypt.</param>
		public EncryptionHandler(DataContainer Container)
		{
			containers = new List<DataContainer> { Container };
		}

		/// <summary>
		/// Creates a new <see cref="EncryptionHandler"/> object.
		/// </summary>
		/// <param name="MiniContainer">The <see cref="MiniDataContainer"/> to encrypt.</param>
		public EncryptionHandler(MiniDataContainer MiniContainer)
		{
			miniContainers = new List<MiniDataContainer> { MiniContainer };
		}

		/// <summary>
		/// Creates a new <see cref="EncryptionHandler"/> object.
		/// </summary>
		/// <param name="Containers">The <em>DataContainers</em> to encrypt.</param>
		public EncryptionHandler(List<DataContainer> Containers)
		{
			string outputFile = Containers[0].ContainerName;
			List<DataContainer> newList = new List<DataContainer>();

			foreach (DataContainer dc in Containers)
			{
				if (dc.ContainerName != outputFile)
				{
					newList.Add(new DataContainer(outputFile, dc.SubContainers));
				}
			}

			containers = newList;
		}

		/// <summary>
		/// Creates a new <see cref="EncryptionHandler"/> object.
		/// </summary>
		/// <param name="MiniContainers">The <em>MiniDateContainers</em> to encrypt.</param>
		public EncryptionHandler(List<MiniDataContainer> MiniContainers)
		{
			string outputFile = MiniContainers[0].ContainerName;
			List<MiniDataContainer> newList = new List<MiniDataContainer>();

			foreach (MiniDataContainer mdc in MiniContainers)
			{
				if (mdc.ContainerName != outputFile)
				{
					newList.Add(new MiniDataContainer(outputFile, mdc.Value));
				}
			}

			miniContainers = newList;
		}

		/// <summary>
		/// Encrypts/serializese the data containers into a file.
		/// </summary>
		public void Encrypt()
		{
			if (containers.Count == 0)
			{
				using (FileStream encStream = new FileStream(containers[0].ContainerName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
				{
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(encStream, ContainerHolder.CreateHolder(containers));
					encStream.Close();
				}
			}
			else if (miniContainers.Count == 0)
			{
				using (FileStream encStream = new FileStream(miniContainers[0].ContainerName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
				{
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(encStream, ContainerHolder.CreateHolder(miniContainers));
					encStream.Close();
				}
			}
			else throw CannotEncryptException;
		}

		/// <summary>
		/// Decrypts the file encrypted by this <see cref="EncryptionHandler"/> and returns the <em>DataContainers</em> if any were encrypted.
		/// </summary>
		/// <returns>The list of <em>DataContainers</em> encrypted into the file, otherwise <see langword="null"/>.</returns>
		public List<DataContainer> DecryptAndGetContainers()
		{
			List<DataContainer> containers = new List<DataContainer>();
			using (FileStream encStream = new FileStream(this.containers[0].ContainerName, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				BinaryFormatter bf = new BinaryFormatter();
				ContainerHolder holder = ContainerHolderFromObject(bf.Deserialize(encStream));
				if (holder.GetContainers() != null)
				{
					containers = holder.GetContainers();
				}
				else throw CannotDecryptException;
				encStream.Close();
			}
			return containers;
		}

		/// <summary>
		/// Decrypts the file encrypted by this <see cref="EncryptionHandler"/> and returns the <em>MiniDataConainers</em> if any were encrypted.
		/// </summary>
		/// <returns>The list of <em>MiniDataContainers</em> encrypted into the file, otherwise <see langword="null"/>.</returns>
		public List<MiniDataContainer> DecryptAndGetMiniContainers()
		{
			List<MiniDataContainer> miniContainers = new List<MiniDataContainer>();
			using (FileStream encStream = new FileStream(this.miniContainers[0].ContainerName, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				BinaryFormatter bf = new BinaryFormatter();
				ContainerHolder holder = ContainerHolderFromObject(bf.Deserialize(encStream));
				if (holder.GetMiniContainers() != null)
				{
					miniContainers = holder.GetMiniContainers();
				}
				else throw CannotDecryptException;
				encStream.Close();
			}
			return miniContainers;
		}

		/// <summary>
		/// The <see cref="InvalidOperationException"/> thrown when no data containers were specified in the constructor.
		/// </summary>
		private InvalidOperationException CannotEncryptException => new InvalidOperationException("Cannot encrypt data containers because none were given.");

		/// <summary>
		/// The <see cref="InvalidOperationException"/> thrown when no data containers were found from the decryption of the file.
		/// </summary>
		private InvalidOperationException CannotDecryptException => new InvalidOperationException("Decryption failed because no containers were encrypted into the file."); 

		/// <summary>
		/// Converts a basic <see cref="object"/> into a <see cref="ContainerHolder"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns></returns>
		private ContainerHolder ContainerHolderFromObject(object value)
		{
			return (ContainerHolder)value;
		}
	}
}
