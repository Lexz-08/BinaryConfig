using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinaryConfig
{
	/// <summary>
	/// Allows for <em><strong>encryption</strong></em> and <em><strong>decryption</strong></em> of the config files generated using the <strong>data containers</strong>.
	/// </summary>
	public class EncryptionHandler
	{
		/// <summary>
		/// Holds the container to be encrypted which can then be retrieved later.
		/// </summary>
		[Serializable]
		private struct ContainerHolder
		{
			private DataContainer container;
			private MiniDataContainer miniContainer;

			/// <summary>
			/// Creates a new <see cref="ContainerHolder"/> object.
			/// </summary>
			/// <param name="Containers">The <em>DataContainers</em> to hold.</param>
			public ContainerHolder(DataContainer Container)
			{
				container = Container;
				miniContainer = null;
			}

			/// <summary>
			/// Creates a new <see cref="ContainerHolder"/> object.
			/// </summary>
			/// <param name="MiniContainers">The <em>MiniContainers</em> to hold.</param>
			public ContainerHolder(MiniDataContainer MiniContainer)
			{
				container = null;
				miniContainer = MiniContainer;
			}

			/// <summary>
			/// Retrieves the list of <em>DataContainers</em>.
			/// </summary>
			/// <returns>The list of <em>DataContainers</em>.</returns>
			public DataContainer GetContainer() => container;

			/// <summary>
			/// Retrieves the list of <em>MiniDataContainers</em>.
			/// </summary>
			/// <returns>The list of <em>MiniDataContainers</em>.</returns>
			public MiniDataContainer GetMiniContainer() => miniContainer;

			/// <summary>
			/// Creates a new <see cref="ContainerHolder"/> object.
			/// </summary>
			/// <param name="Containers">The <em>DataContainers</em> to hold.</param>
			/// <returns>A new <see cref="ContainerHolder"/>.</returns>
			public static ContainerHolder CreateHolder(DataContainer Container)
			{
				return new ContainerHolder(Container);
			}

			/// <summary>
			/// Creates a new <see cref="ContainerHolder"/> object.
			/// </summary>
			/// <param name="MiniContainers">The <em>MiniDataContainers</em> to hold.</param>
			/// <returns>A new <see cref="ContainerHolder"/>.</returns>
			public static ContainerHolder CreateHolder(MiniDataContainer MiniContainer)
			{
				return new ContainerHolder(MiniContainer);
			}
		}

		/// <summary>
		/// Encrypts the given <paramref name="Container"/> into a file that can later be decrypted.
		/// </summary>
		/// <param name="Container">The <see cref="DataContainer"/> to encrypt.</param>
		public static void Encrypt(DataContainer Container)
		{
			if (Container != null)
			{
				using (FileStream encStream = new FileStream(Container.ContainerName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
				{
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(encStream, ContainerHolder.CreateHolder(Container));
					encStream.Close();
				}
			}
			else throw CannotEncryptException;
		}

		/// <summary>
		/// Encrypts the given <paramref name="MiniContainer"/> into a file that can later be decrypted.
		/// </summary>
		/// <param name="MiniContainer">The <see cref="MiniDataContainer"/> to encrypt.</param>
		public static void Encrypt(MiniDataContainer MiniContainer)
		{
			if (MiniContainer != null)
			{
				using (FileStream encStream = new FileStream(MiniContainer.ContainerName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
				{
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(encStream, ContainerHolder.CreateHolder(MiniContainer));
					encStream.Close();
				}
			}
			else throw CannotEncryptException;
		}

		/// <summary>
		/// Attempts to encrypt the given <paramref name="Container"/> into a file that can be later decrypted.
		/// </summary>
		/// <param name="Container">The <see cref="DataContainer"/> to attempt to encrypt.</param>
		/// <returns>
		/// <see langword="true"/>, If the <paramref name="Container"/> was successfully encrypted.<br/>
		/// <see langword="false"/>, If the <paramref name="Container"/> failed to get encrypted.
		/// </returns>
		public static bool TryEncrypt(DataContainer Container)
		{
			if (Container != null)
			{
				using (FileStream encStream = new FileStream(Container.ContainerName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
				{
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(encStream, ContainerHolder.CreateHolder(Container));
					encStream.Close();
					return true;
				}
			}
			else return false;
		}

		/// <summary>
		/// Attempts to encrypt the given <paramref name="MiniContainer"/> into a file that can be later decrypted.
		/// </summary>
		/// <param name="MiniContainer">The <see cref="MiniDataContainer"/> to attempt to encrypt.</param>
		/// <returns>
		/// <see langword="true"/>, If the <paramref name="MiniContainer"/> was successfully encrypted.<br/>
		/// <see langword="false"/>, If the <paramref name="MiniContainer"/> failed to get encrypted.
		/// </returns>
		public static bool TryEncrypt(MiniDataContainer MiniContainer)
		{
			if (MiniContainer != null)
			{
				using (FileStream encStream = new FileStream(MiniContainer.ContainerName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
				{
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(encStream, ContainerHolder.CreateHolder(MiniContainer));
					encStream.Close();
					return true;
				}
			}
			else return false;
		}

		/// <summary>
		/// Decrypts the specified <paramref name="EncryptedFile"/> and returns a <see cref="DataContainer"/> stored within a <see cref="ContainerHolder"/> for encryption safety.
		/// </summary>
		/// <param name="EncryptedFile">The file to decrypt and get a <see cref="DataContainer"/> from.</param>
		/// <returns>A <see cref="DataContainer"/> retrieved from the encrypted file.</returns>
		public static DataContainer DecryptContainer(string EncryptedFile)
		{
			if (File.Exists(EncryptedFile))
			{
				DataContainer container = null;
				using (FileStream decStream = new FileStream(EncryptedFile, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					BinaryFormatter bf = new BinaryFormatter();
					ContainerHolder holder = ContainerHolderFromObject(bf.Deserialize(decStream));
					if (holder.GetContainer() != null)
					{
						container = holder.GetContainer();
					}
					else throw CannotDecryptException;
					decStream.Close();
				}
				return container;
			}
			else throw EncryptedFileNotFound;
		}

		/// <summary>
		/// Decrypts the specified <paramref name="EncryptedFile"/> and returns a <see cref="MiniDataContainer"/> stored within a <see cref="ContainerHolder"/> for encryption safety.
		/// </summary>
		/// <param name="EncryptedFile">The file to decrypt and get a <see cref="MiniDataContainer"/> from.</param>
		/// <returns>A <see cref="MiniDataContainer"/> retrieved from the encrypted file.</returns>
		public static MiniDataContainer DecryptMiniContainer(string EncryptedFile)
		{
			if (File.Exists(EncryptedFile))
			{
				MiniDataContainer miniContainer = null;
				using (FileStream decStream = new FileStream(EncryptedFile, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					BinaryFormatter bf = new BinaryFormatter();
					ContainerHolder holder = ContainerHolderFromObject(bf.Deserialize(decStream));
					if (holder.GetMiniContainer() != null)
					{
						miniContainer = holder.GetMiniContainer();
					}
					else throw CannotDecryptException;
					decStream.Close();
				}
				return miniContainer;
			}
			else throw EncryptedFileNotFound;
		}

		/// <summary>
		/// Attempts to decrypt the specified <paramref name="EncryptedFile"/> and returns a <see cref="DataContainer"/> if it was a success.
		/// </summary>
		/// <param name="EncryptedFile">The file to decrypt and get a <see cref="DataContainer"/> from.</param>
		/// <param name="Container">The <see cref="DataContainer"/> to return.</param>
		/// <returns>
		/// <see langword="true"/>, If the decryption succeeded.
		/// <see langword="false"/>, If the decryption failed.
		/// </returns>
		public static bool TryDecryptContainer(string EncryptedFile, out DataContainer Container)
		{
			if (File.Exists(EncryptedFile))
			{
				using (FileStream decStream = new FileStream(EncryptedFile, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					BinaryFormatter bf = new BinaryFormatter();
					ContainerHolder holder = ContainerHolderFromObject(bf.Deserialize(decStream));
					if (holder.GetContainer() != null)
					{
						Container = holder.GetContainer();
					}
					else throw CannotDecryptException;
					decStream.Close();
					return true;
				}
			}
			else
			{
				Container = null;
				return false;
			}
		}

		/// <summary>
		/// Attempts to decrypt the specified <paramref name="EncryptedFile"/> and returns a <see cref="MiniDataContainer"/> if it was a success.
		/// </summary>
		/// <param name="EncryptedFile">The file to decrypt and get a <see cref="DataContainer"/> from.</param>
		/// <param name="MiniContainer">The <see cref="MiniDataContainer"/> to return.</param>
		/// <returns>
		/// <see langword="true"/>, If the decryption succeeded.<br/>
		/// <see langword="false"/>, If the decryption failed.
		/// </returns>
		public static bool TryDecryptMiniContainer(string EncryptedFile, out MiniDataContainer MiniContainer)
		{
			if (File.Exists(EncryptedFile))
			{
				using (FileStream decStream = new FileStream(EncryptedFile, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					BinaryFormatter bf = new BinaryFormatter();
					ContainerHolder holder = ContainerHolderFromObject(bf.Deserialize(decStream));
					if (holder.GetMiniContainer() != null)
					{
						MiniContainer = holder.GetMiniContainer();
					}
					else throw CannotDecryptException;
					decStream.Close();
					return true;
				}
			}
			else
			{
				MiniContainer = null;
				return false;
			}
		}

		/// <summary>
		/// The <see cref="InvalidOperationException"/> thrown when no data containers were specified in the constructor.
		/// </summary>
		private static InvalidOperationException CannotEncryptException => new InvalidOperationException("Cannot encrypt data containers because none were given.");

		/// <summary>
		/// The <see cref="InvalidOperationException"/> thrown when no data containers were found from the decryption of the file.
		/// </summary>
		private static InvalidOperationException CannotDecryptException => new InvalidOperationException("Decryption failed because no containers were encrypted into the file.");

		/// <summary>
		/// The <see cref="FileNotFoundException"/> thrown when the specified encrypted file was not found in the device.
		/// </summary>
		private static FileNotFoundException EncryptedFileNotFound => new FileNotFoundException("Couldn't decrypt the file because there is no file.");

		/// <summary>
		/// Converts a basic <see cref="object"/> into a <see cref="ContainerHolder"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns></returns>
		private static ContainerHolder ContainerHolderFromObject(object value)
		{
			return (ContainerHolder)value;
		}
	}
}
