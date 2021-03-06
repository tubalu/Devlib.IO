//-----------------------------------------------------------------------
// <copyright file="ZipArchive.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Compression
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represents a package of compressed files in the zip archive format.
    /// </summary>
    public class ZipArchive : IDisposable
    {
        private Stream _archiveStream;
        private ZipArchiveEntry _archiveStreamOwner;
        private BinaryReader _archiveReader;
        private ZipArchiveMode _mode;
        private List<ZipArchiveEntry> _entries;
        private ReadOnlyCollection<ZipArchiveEntry> _entriesCollection;
        private Dictionary<string, ZipArchiveEntry> _entriesDictionary;
        private bool _readEntries;
        private bool _leaveOpen;
        private long _centralDirectoryStart;
        private bool _isDisposed;
        private uint _numberOfThisDisk;
        private long _expectedNumberOfEntries;
        private Stream _backingStream;
        private byte[] _archiveComment;
        private Encoding _entryNameEncoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DevLib.ZipArchive" /> class from the specified stream.
        /// </summary>
        /// <param name="stream">The stream that contains the archive to be read.</param>
        /// <exception cref="T:System.ArgumentException">The stream is already closed or does not support reading.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="stream" /> is null.</exception>
        /// <exception cref="T:System.IO.InvalidDataException">The contents of the stream are not in the zip archive format.</exception>
        public ZipArchive(Stream stream)
            : this(stream, ZipArchiveMode.Read, false, (Encoding)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DevLib.ZipArchive" /> class from the specified stream and with the specified mode.
        /// </summary>
        /// <param name="stream">The input or output stream.</param>
        /// <param name="mode">One of the enumeration values that indicates whether the zip archive is used to read, create, or update entries.</param>
        /// <exception cref="T:System.ArgumentException">The stream is already closed, or the capabilities of the stream do not match the mode.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="stream" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="mode" /> is an invalid value.</exception>
        /// <exception cref="T:System.IO.InvalidDataException">The contents of the stream could not be interpreted as a zip archive.-or-<paramref name="mode" /> is <see cref="F:DevLib.Compression.ZipArchiveMode.Update" /> and an entry is missing from the archive or is corrupt and cannot be read.-or-<paramref name="mode" /> is <see cref="F:DevLib.Compression.ZipArchiveMode.Update" /> and an entry is too large to fit into memory.</exception>
        public ZipArchive(Stream stream, ZipArchiveMode mode)
            : this(stream, mode, false, (Encoding)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DevLib.ZipArchive" /> class on the specified stream for the specified mode, and optionally leaves the stream open.
        /// </summary>
        /// <param name="stream">The input or output stream.</param>
        /// <param name="mode">One of the enumeration values that indicates whether the zip archive is used to read, create, or update entries.</param>
        /// <param name="leaveOpen">true to leave the stream open after the <see cref="T:DevLib.Compression.ZipArchive" /> object is disposed; otherwise, false.</param>
        /// <exception cref="T:System.ArgumentException">The stream is already closed, or the capabilities of the stream do not match the mode.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="stream" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="mode" /> is an invalid value.</exception>
        /// <exception cref="T:System.IO.InvalidDataException">The contents of the stream could not be interpreted as a zip archive.-or-<paramref name="mode" /> is <see cref="F:DevLib.Compression.ZipArchiveMode.Update" /> and an entry is missing from the archive or is corrupt and cannot be read.-or-<paramref name="mode" /> is <see cref="F:DevLib.Compression.ZipArchiveMode.Update" /> and an entry is too large to fit into memory.</exception>
        public ZipArchive(Stream stream, ZipArchiveMode mode, bool leaveOpen)
            : this(stream, mode, leaveOpen, (Encoding)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DevLib.Compression.ZipArchive" /> class on the specified stream for the specified mode, uses the specified encoding for entry names, and optionally leaves the stream open.
        /// </summary>
        /// <param name="stream">The input or output stream.</param>
        /// <param name="mode">One of the enumeration values that indicates whether the zip archive is used to read, create, or update entries.</param>
        /// <param name="leaveOpen">true to leave the stream open after the <see cref="T:DevLib.Compression.ZipArchive" /> object is disposed; otherwise, false.</param>
        /// <param name="entryNameEncoding">The encoding to use when reading or writing entry names in this archive. Specify a value for this parameter only when an encoding is required for interoperability with zip archive tools and libraries that do not support UTF-8 encoding for entry names.</param>
        /// <exception cref="ArgumentNullException">stream</exception>
        /// <exception cref="T:System.ArgumentException">The stream is already closed, or the capabilities of the stream do not match the mode.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="stream" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="mode" /> is an invalid value.</exception>
        /// <exception cref="T:System.IO.InvalidDataException">The contents of the stream could not be interpreted as a zip archive.-or-<paramref name="mode" /> is <see cref="F:DevLib.Compression.ZipArchiveMode.Update" /> and an entry is missing from the archive or is corrupt and cannot be read.-or-<paramref name="mode" /> is <see cref="F:DevLib.Compression.ZipArchiveMode.Update" /> and an entry is too large to fit into memory.</exception>
        public ZipArchive(Stream stream, ZipArchiveMode mode, bool leaveOpen, Encoding entryNameEncoding)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            this.EntryNameEncoding = entryNameEncoding;
            this.Init(stream, mode, leaveOpen);
        }

        /// <summary>
        /// Gets the collection of entries that are currently in the zip archive.
        /// </summary>
        public ReadOnlyCollection<ZipArchiveEntry> Entries
        {
            get
            {
                if (this._mode == ZipArchiveMode.Create)
                {
                    throw new NotSupportedException(CompressionConstants.EntriesInCreateMode);
                }

                this.ThrowIfDisposed();

                this.EnsureCentralDirectoryRead();

                return this._entriesCollection;
            }
        }

        /// <summary>
        /// Gets a value that describes the type of action the zip archive can perform on entries.
        /// </summary>
        public ZipArchiveMode Mode
        {
            get
            {
                return this._mode;
            }
        }

        internal BinaryReader ArchiveReader
        {
            get
            {
                return this._archiveReader;
            }
        }

        internal Stream ArchiveStream
        {
            get
            {
                return this._archiveStream;
            }
        }

        internal uint NumberOfThisDisk
        {
            get
            {
                return this._numberOfThisDisk;
            }
        }

        internal Encoding EntryNameEncoding
        {
            get
            {
                return this._entryNameEncoding;
            }

            private set
            {
                if (value != null && (value.Equals(Encoding.BigEndianUnicode) || value.Equals(Encoding.Unicode) || (value.Equals(Encoding.UTF32) || value.Equals(Encoding.UTF7))))
                {
                    throw new ArgumentException(CompressionConstants.EntryNameEncodingNotSupported, "entryNameEncoding");
                }

                this._entryNameEncoding = value;
            }
        }

        /// <summary>
        /// Creates an empty entry that has the specified path and entry name in the zip archive.
        /// </summary>
        /// <param name="entryName">A path, relative to the root of the archive, that specifies the name of the entry to be created.</param>
        /// <param name="entryAttributes">The entry attributes.</param>
        /// <returns>An empty entry in the zip archive.</returns>
        /// <exception cref="ArgumentNullException">entryName</exception>
        /// <exception cref="ArgumentException">entryName</exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="entryName" /> is <see cref="F:System.String.Empty" />.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entryName" /> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">The zip archive does not support writing.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The zip archive has been disposed.</exception>
        public ZipArchiveEntry CreateEntry(string entryName, FileAttributes entryAttributes=FileAttributes.Normal)
        {
            if (entryName == null)
            {
                throw new ArgumentNullException("entryName");
            }

            if (string.IsNullOrEmpty(entryName))
            {
                throw new ArgumentException(CompressionConstants.CannotBeEmpty, "entryName");
            }

            if (this._mode == ZipArchiveMode.Read)
            {
                throw new NotSupportedException(CompressionConstants.CreateInReadMode);
            }

            this.ThrowIfDisposed();

            ZipArchiveEntry entry = new ZipArchiveEntry(this, entryName, entryAttributes);
            this.AddEntry(entry);

            return entry;
        }

        /// <summary>
        /// Archives a file by compressing it and adding it to the zip archive.
        /// </summary>
        /// <param name="sourceFileName">The path to the file to be archived. You can specify either a relative or an absolute path. A relative path is interpreted as relative to the current working directory.</param>
        /// <param name="entryName">The name of the entry to create in the zip archive.</param>
        /// <returns>A wrapper for the new entry in the zip archive.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="sourceFileName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.-or-<paramref name="entryName" /> is <see cref="F:System.String.Empty" />.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceFileName" /> or <paramref name="entryName" /> is null.</exception>
        /// <exception cref="T:System.IO.PathTooLongException">In <paramref name="sourceFileName" />, the specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must not exceed 248 characters, and file names must not exceed 260 characters.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="sourceFileName" /> is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.IO.IOException">The file specified by <paramref name="sourceFileName" /> cannot be opened.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException"><paramref name="sourceFileName" /> specifies a directory.-or-The caller does not have the required permission to access the file specified by <paramref name="sourceFileName" />.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="sourceFileName" /> is not found.</exception>
        /// <exception cref="T:System.NotSupportedException">The <paramref name="sourceFileName" /> parameter is in an invalid format.-or-The zip archive does not support writing.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The zip archive has been disposed.</exception>
        public ZipArchiveEntry CreateEntryFromFile(string sourceFileName, string entryName)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName");
            }

            if (entryName == null)
            {
                throw new ArgumentNullException("entryName");
            }

            using (Stream stream = File.Open(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ZipArchiveEntry zipArchiveEntry = this.CreateEntry(entryName, File.GetAttributes(sourceFileName));

                DateTime dateTime = File.GetLastWriteTime(sourceFileName);

                if (dateTime.Year < 1980 || dateTime.Year > 2107)
                {
                    dateTime = new DateTime(1980, 1, 1, 0, 0, 0);
                }

                zipArchiveEntry.LastWriteTime = (DateTimeOffset)dateTime;

                using (Stream destination = zipArchiveEntry.Open())
                {
                    ZipHelper.CopyStreamTo(stream, destination);
                }

                return zipArchiveEntry;
            }
        }

        /// <summary>
        /// Archives a stream by compressing it and adding it to the zip archive.
        /// </summary>
        /// <param name="sourceStream">The source stream to compress.</param>
        /// <param name="entryName">The name of the entry to create in the zip archive.</param>
        /// <param name="entryAttributes">The entry attributes.</param>
        /// <returns>A wrapper for the new entry in the zip archive.</returns>
        public ZipArchiveEntry CreateEntryFromStream(Stream sourceStream, string entryName, FileAttributes entryAttributes = FileAttributes.Normal)
        {
            if (sourceStream == null)
            {
                throw new ArgumentNullException("sourceStream");
            }

            if (entryName == null)
            {
                throw new ArgumentNullException("entryName");
            }

            ZipArchiveEntry zipArchiveEntry = this.CreateEntry(entryName, entryAttributes);

            zipArchiveEntry.LastWriteTime = DateTimeOffset.Now;

            using (Stream destination = zipArchiveEntry.Open())
            {
                ZipHelper.CopyStreamTo(sourceStream, destination);
            }

            return zipArchiveEntry;
        }

        /// <summary>
        /// Archives a directory by compressing it and adding it to the zip archive.
        /// </summary>
        /// <param name="sourceDirectoryName">The path to be archived. You can specify either a relative or an absolute path. A relative path is interpreted as relative to the current working directory.</param>
        /// <param name="entryName">The name of the entry to create in the zip archive.</param>
        /// <returns>A wrapper for the new entry in the zip archive.</returns>
        public ZipArchiveEntry CreateEntryFromDirectory(string sourceDirectoryName, string entryName)
        {
            if (sourceDirectoryName == null)
            {
                throw new ArgumentNullException("sourceDirectoryName");
            }

            if (entryName == null)
            {
                throw new ArgumentNullException("entryName");
            }

            ZipArchiveEntry zipArchiveEntry = this.CreateEntry(entryName.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, File.GetAttributes(sourceDirectoryName));

            DateTime dateTime = File.GetLastWriteTime(sourceDirectoryName);

            if (dateTime.Year < 1980 || dateTime.Year > 2107)
            {
                dateTime = new DateTime(1980, 1, 1, 0, 0, 0);
            }

            zipArchiveEntry.LastWriteTime = (DateTimeOffset)dateTime;

            return zipArchiveEntry;
        }

        /// <summary>
        /// Retrieves a wrapper for the specified entry in the zip archive.
        /// </summary>
        /// <param name="entryName">A path, relative to the root of the archive, that identifies the entry to retrieve.</param>
        /// <returns>A wrapper for the specified entry in the archive; null if the entry does not exist in the archive.</returns>
        /// <exception cref="ArgumentNullException">entryName</exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="entryName" /> is <see cref="F:System.String.Empty" />.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entryName" /> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">The zip archive does not support reading.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The zip archive has been disposed.</exception>
        /// <exception cref="T:System.IO.InvalidDataException">The zip archive is corrupt, and its entries cannot be retrieved.</exception>
        public ZipArchiveEntry GetEntry(string entryName)
        {
            if (entryName == null)
            {
                throw new ArgumentNullException("entryName");
            }

            if (this._mode == ZipArchiveMode.Create)
            {
                throw new NotSupportedException(CompressionConstants.EntriesInCreateMode);
            }

            this.EnsureCentralDirectoryRead();
            ZipArchiveEntry zipArchiveEntry;
            this._entriesDictionary.TryGetValue(entryName, out zipArchiveEntry);

            return zipArchiveEntry;
        }

        /// <summary>
        /// Extracts all the files in the zip archive to a directory on the file system.
        /// </summary>
        /// <param name="destinationDirectoryName">The path to the directory to place the extracted files in. You can specify either a relative or an absolute path. A relative path is interpreted as relative to the current working directory.</param>
        /// <param name="overwrite">true to overwrite an existing file that has the same name as the destination file; otherwise, false.</param>
        /// <param name="throwOnError">true to throw any exception that occurs.-or- false to ignore any exception that occurs.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="destinationDirectoryName" /> is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destinationDirectoryName" /> is null.</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path exceeds the system-defined maximum length. For example, on Windows-based platforms, paths must not exceed 248 characters, and file names must not exceed 260 characters.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.IO.IOException">The directory specified by <paramref name="destinationDirectoryName" /> already exists.-or-The name of an entry in the archive is <see cref="F:System.String.Empty" />, contains only white space, or contains at least one invalid character.-or-Extracting an entry from the archive would create a file that is outside the directory specified by <paramref name="destinationDirectoryName" />. (For example, this might happen if the entry name contains parent directory accessors.) -or-Two or more entries in the archive have the same name.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission to write to the destination directory.</exception>
        /// <exception cref="T:System.NotSupportedException"><paramref name="destinationDirectoryName" /> contains an invalid format.</exception>
        /// <exception cref="T:System.IO.InvalidDataException">An archive entry cannot be found or is corrupt.-or-An archive entry was compressed by using a compression method that is not supported.</exception>
        public void ExtractToDirectory(string destinationDirectoryName, bool overwrite, bool throwOnError = false)
        {
            if (destinationDirectoryName == null)
            {
                throw new ArgumentNullException("destinationDirectoryName");
            }

            DirectoryInfo directoryInfo = Directory.CreateDirectory(destinationDirectoryName);
            string directoryFullName = directoryInfo.FullName;

            foreach (ZipArchiveEntry zipArchiveEntry in this.Entries)
            {
                string fullPath = Path.GetFullPath(Path.Combine(directoryFullName, zipArchiveEntry.FullName));

                if (!fullPath.StartsWith(directoryFullName, StringComparison.OrdinalIgnoreCase))
                {
                    if (throwOnError)
                    {
                        throw new IOException(CompressionConstants.ExtractingResultsInOutside);
                    }
                }

                try
                {
                    if (zipArchiveEntry.IsDirectory)
                    {
                        zipArchiveEntry.ExtractToDirectory(fullPath, overwrite);
                    }
                    else
                    {
                        zipArchiveEntry.ExtractToFile(fullPath, overwrite);
                    }
                }
                catch
                {
                    if (throwOnError)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Releases the resources used by the current instance of the <see cref="T:DevLib.Compression.ZipArchive"/> class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void AcquireArchiveStream(ZipArchiveEntry entry)
        {
            if (this._archiveStreamOwner != null)
            {
                if (this._archiveStreamOwner.EverOpenedForWrite)
                {
                    throw new IOException(CompressionConstants.CreateModeCreateEntryWhileOpen);
                }

                this._archiveStreamOwner.WriteAndFinishLocalEntry();
            }

            this._archiveStreamOwner = entry;
        }

        internal bool IsStillArchiveStreamOwner(ZipArchiveEntry entry)
        {
            return this._archiveStreamOwner == entry;
        }

        internal void ReleaseArchiveStream(ZipArchiveEntry entry)
        {
            this._archiveStreamOwner = null;
        }

        internal void RemoveEntry(ZipArchiveEntry entry)
        {
            this._entries.Remove(entry);
            this._entriesDictionary.Remove(entry.FullName);
        }

        internal void ThrowIfDisposed()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        /// <summary>
        /// Called by the <see cref="M:DevLib.Compression.ZipArchive.Dispose" /> and <see cref="M:System.Object.Finalize" /> methods to release the unmanaged resources used by the current instance of the <see cref="T:DevLib.Compression.ZipArchive" /> class, and optionally finishes writing the archive and releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to finish writing the archive and release unmanaged and managed resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this._isDisposed)
            {
                switch (this._mode)
                {
                    case ZipArchiveMode.Read:
                        this.CloseStreams();
                        this._isDisposed = true;
                        break;
                    default:
                        try
                        {
                            this.WriteFile();
                            this.CloseStreams();
                            this._isDisposed = true;
                            break;
                        }
                        catch (InvalidDataException)
                        {
                            this.CloseStreams();
                            this._isDisposed = true;
                            throw;
                        }
                }
            }
        }

        private void AddEntry(ZipArchiveEntry entry)
        {
            this._entries.Add(entry);
            string fullName = entry.FullName;

            if (this._entriesDictionary.ContainsKey(fullName))
            {
                return;
            }

            this._entriesDictionary.Add(fullName, entry);
        }

        private void CloseStreams()
        {
            if (this._leaveOpen)
            {
                if (this._backingStream != null)
                {
                    this._archiveStream.Close();
                }
            }
            else
            {
                this._archiveStream.Close();

                if (this._backingStream != null)
                {
                    this._backingStream.Close();
                }

                if (this._archiveReader != null)
                {
                    this._archiveReader.Close();
                }
            }
        }

        private void EnsureCentralDirectoryRead()
        {
            if (this._readEntries)
            {
                return;
            }

            this.ReadCentralDirectory();
            this._readEntries = true;
        }

        private void Init(Stream stream, ZipArchiveMode mode, bool leaveOpen)
        {
            Stream stream1 = null;

            try
            {
                this._backingStream = null;

                switch (mode)
                {
                    case ZipArchiveMode.Read:
                        if (!stream.CanRead)
                        {
                            throw new ArgumentException(CompressionConstants.ReadModeCapabilities);
                        }

                        if (!stream.CanSeek)
                        {
                            this._backingStream = stream;
                            stream1 = stream = new MemoryStream();
                            ZipHelper.CopyStreamTo(this._backingStream, stream);
                            stream.Seek(0L, SeekOrigin.Begin);
                            break;
                        }

                        break;
                    case ZipArchiveMode.Create:
                        if (!stream.CanWrite)
                        {
                            throw new ArgumentException(CompressionConstants.CreateModeCapabilities);
                        }

                        break;
                    case ZipArchiveMode.Update:
                        if (!stream.CanRead || !stream.CanWrite || !stream.CanSeek)
                        {
                            throw new ArgumentException(CompressionConstants.UpdateModeCapabilities);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException("mode");
                }

                this._mode = mode;
                this._archiveStream = stream;
                this._archiveStreamOwner = null;
                this._archiveReader = mode != ZipArchiveMode.Create ? new BinaryReader(stream) : null;
                this._entries = new List<ZipArchiveEntry>();
                this._entriesCollection = new ReadOnlyCollection<ZipArchiveEntry>(this._entries);
                this._entriesDictionary = new Dictionary<string, ZipArchiveEntry>();
                this._readEntries = false;
                this._leaveOpen = leaveOpen;
                this._centralDirectoryStart = 0L;
                this._isDisposed = false;
                this._numberOfThisDisk = 0U;
                this._archiveComment = null;

                switch (mode)
                {
                    case ZipArchiveMode.Read:
                        this.ReadEndOfCentralDirectory();
                        break;
                    case ZipArchiveMode.Create:
                        this._readEntries = true;
                        break;
                    default:
                        if (this._archiveStream.Length == 0L)
                        {
                            this._readEntries = true;
                            break;
                        }

                        this.ReadEndOfCentralDirectory();
                        this.EnsureCentralDirectoryRead();

                        using (List<ZipArchiveEntry>.Enumerator enumerator = this._entries.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                enumerator.Current.ThrowIfNotOpenable(false, true);
                            }

                            break;
                        }
                }
            }
            catch
            {
                if (stream1 != null)
                {
                    stream1.Close();
                }

                throw;
            }
        }

        private void ReadCentralDirectory()
        {
            try
            {
                this._archiveStream.Seek(this._centralDirectoryStart, SeekOrigin.Begin);
                long num = 0L;
                bool saveExtraFieldsAndComments = this.Mode == ZipArchiveMode.Update;
                ZipCentralDirectoryFileHeader header;

                while (ZipCentralDirectoryFileHeader.TryReadBlock(this._archiveReader, saveExtraFieldsAndComments, out header))
                {
                    this.AddEntry(new ZipArchiveEntry(this, header));
                    ++num;
                }

                if (num != this._expectedNumberOfEntries)
                {
                    throw new InvalidDataException(CompressionConstants.NumEntriesWrong);
                }
            }
            catch (EndOfStreamException ex)
            {
                throw new InvalidDataException(CompressionConstants.CentralDirectoryInvalid, ex);
            }
        }

        private void ReadEndOfCentralDirectory()
        {
            try
            {
                this._archiveStream.Seek(-18L, SeekOrigin.End);

                if (!ZipHelper.SeekBackwardsToSignature(this._archiveStream, 101010256U))
                {
                    throw new InvalidDataException(CompressionConstants.EOCDNotFound);
                }

                long position = this._archiveStream.Position;
                ZipEndOfCentralDirectoryBlock eocdBlock;
                ZipEndOfCentralDirectoryBlock.TryReadBlock(this._archiveReader, out eocdBlock);

                if ((int)eocdBlock.NumberOfThisDisk != (int)eocdBlock.NumberOfTheDiskWithTheStartOfTheCentralDirectory)
                {
                    throw new InvalidDataException(CompressionConstants.SplitSpanned);
                }

                this._numberOfThisDisk = (uint)eocdBlock.NumberOfThisDisk;
                this._centralDirectoryStart = (long)eocdBlock.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber;

                if ((int)eocdBlock.NumberOfEntriesInTheCentralDirectory != (int)eocdBlock.NumberOfEntriesInTheCentralDirectoryOnThisDisk)
                {
                    throw new InvalidDataException(CompressionConstants.SplitSpanned);
                }

                this._expectedNumberOfEntries = (long)eocdBlock.NumberOfEntriesInTheCentralDirectory;

                if (this._mode == ZipArchiveMode.Update)
                {
                    this._archiveComment = eocdBlock.ArchiveComment;
                }

                if ((int)eocdBlock.NumberOfThisDisk == (int)ushort.MaxValue || (int)eocdBlock.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber == -1 || (int)eocdBlock.NumberOfEntriesInTheCentralDirectory == (int)ushort.MaxValue)
                {
                    this._archiveStream.Seek(position - 16L, SeekOrigin.Begin);

                    if (ZipHelper.SeekBackwardsToSignature(this._archiveStream, 117853008U))
                    {
                        Zip64EndOfCentralDirectoryLocator zip64EOCDLocator;
                        Zip64EndOfCentralDirectoryLocator.TryReadBlock(this._archiveReader, out zip64EOCDLocator);

                        if (zip64EOCDLocator.OffsetOfZip64EOCD > 9223372036854775807UL)
                        {
                            throw new InvalidDataException(CompressionConstants.FieldTooBigOffsetToZip64EOCD);
                        }

                        this._archiveStream.Seek((long)zip64EOCDLocator.OffsetOfZip64EOCD, SeekOrigin.Begin);
                        Zip64EndOfCentralDirectoryRecord zip64EOCDRecord;

                        if (!Zip64EndOfCentralDirectoryRecord.TryReadBlock(this._archiveReader, out zip64EOCDRecord))
                        {
                            throw new InvalidDataException(CompressionConstants.Zip64EOCDNotWhereExpected);
                        }

                        this._numberOfThisDisk = zip64EOCDRecord.NumberOfThisDisk;

                        if (zip64EOCDRecord.NumberOfEntriesTotal > 9223372036854775807UL)
                        {
                            throw new InvalidDataException(CompressionConstants.FieldTooBigNumEntries);
                        }

                        if (zip64EOCDRecord.OffsetOfCentralDirectory > 9223372036854775807UL)
                        {
                            throw new InvalidDataException(CompressionConstants.FieldTooBigOffsetToCD);
                        }

                        if ((long)zip64EOCDRecord.NumberOfEntriesTotal != (long)zip64EOCDRecord.NumberOfEntriesOnThisDisk)
                        {
                            throw new InvalidDataException(CompressionConstants.SplitSpanned);
                        }

                        this._expectedNumberOfEntries = (long)zip64EOCDRecord.NumberOfEntriesTotal;
                        this._centralDirectoryStart = (long)zip64EOCDRecord.OffsetOfCentralDirectory;
                    }
                }

                if (this._centralDirectoryStart > this._archiveStream.Length)
                {
                    throw new InvalidDataException(CompressionConstants.FieldTooBigOffsetToCD);
                }
            }
            catch (EndOfStreamException ex)
            {
                throw new InvalidDataException(CompressionConstants.CDCorrupt, ex);
            }
            catch (IOException ex)
            {
                throw new InvalidDataException(CompressionConstants.CDCorrupt, ex);
            }
        }

        private void WriteFile()
        {
            if (this._mode == ZipArchiveMode.Update)
            {
                List<ZipArchiveEntry> list = new List<ZipArchiveEntry>();

                foreach (ZipArchiveEntry zipArchiveEntry in this._entries)
                {
                    if (!zipArchiveEntry.LoadLocalHeaderExtraFieldAndCompressedBytesIfNeeded())
                    {
                        list.Add(zipArchiveEntry);
                    }
                }

                foreach (ZipArchiveEntry zipArchiveEntry in list)
                {
                    zipArchiveEntry.Delete();
                }

                this._archiveStream.Seek(0L, SeekOrigin.Begin);
                this._archiveStream.SetLength(0L);
            }

            foreach (ZipArchiveEntry zipArchiveEntry in this._entries)
            {
                zipArchiveEntry.WriteAndFinishLocalEntry();
            }

            long position = this._archiveStream.Position;

            foreach (ZipArchiveEntry zipArchiveEntry in this._entries)
            {
                zipArchiveEntry.WriteCentralDirectoryFileHeader();
            }

            long sizeOfCentralDirectory = this._archiveStream.Position - position;
            this.WriteArchiveEpilogue(position, sizeOfCentralDirectory);
        }

        private void WriteArchiveEpilogue(long startOfCentralDirectory, long sizeOfCentralDirectory)
        {
            bool flag = false;

            if (startOfCentralDirectory >= (long)uint.MaxValue || sizeOfCentralDirectory >= (long)uint.MaxValue || this._entries.Count >= (int)ushort.MaxValue)
            {
                flag = true;
            }

            if (flag)
            {
                long position = this._archiveStream.Position;
                Zip64EndOfCentralDirectoryRecord.WriteBlock(this._archiveStream, (long)this._entries.Count, startOfCentralDirectory, sizeOfCentralDirectory);
                Zip64EndOfCentralDirectoryLocator.WriteBlock(this._archiveStream, position);
            }

            ZipEndOfCentralDirectoryBlock.WriteBlock(this._archiveStream, (long)this._entries.Count, startOfCentralDirectory, sizeOfCentralDirectory, this._archiveComment);
        }
    }
}
