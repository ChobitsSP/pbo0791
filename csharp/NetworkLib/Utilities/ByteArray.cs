namespace NetworkLib.Utilities
{
    using System;
    using System.Text;

    public sealed class ByteArray
    {
        private byte[] _buffer;
        private static int _dangerousSize = 0x1000;
        private static int _defaultSize = 0x200;
        private int _readerIndex;
        public static bool _reuse = false;
        private ValBytes _valBytes;
        private int _writerIndex;
        public const int HEADER_LENGTH = 8;

        public ByteArray() : this(_defaultSize)
        {
        }

        public ByteArray(bool noHeader) : this(_defaultSize, noHeader)
        {
        }

        public ByteArray(int size) : this(size, false)
        {
        }

        public ByteArray(int size, bool noHeader)
        {
            this._valBytes = new ValBytes();
            this._buffer = new byte[size];
            if (!noHeader)
            {
                this.WriteHeader();
            }
        }

        private void AdvanceReader(int steps)
        {
            this._readerIndex += steps;
        }

        private void AdvanceWriter(int steps)
        {
            this._writerIndex += steps;
        }

        public void Append(byte[] buffer, int position, int count)
        {
            this.CheckWriterOverflow(count);
            Array.Copy(buffer, position, this._buffer, this._writerIndex, count);
            this.AdvanceWriter(count);
        }

        public void BypassHeader()
        {
            this.AdvanceReader(8);
        }

        public bool CanRead()
        {
            return (this._readerIndex < this._writerIndex);
        }

        private void CheckReaderOverflow(int steps)
        {
            if (this.WillOverflow(steps))
            {
                throw new BufferOverflowException("Not enough space to read,now {0},advance{1}", new object[] { this._readerIndex, steps });
            }
        }

        private void CheckWriterOverflow(int steps)
        {
            if ((this._writerIndex + steps) > this._buffer.Length)
            {
                try
                {
                    Array.Resize<byte>(ref this._buffer, this._buffer.Length + (_defaultSize * (((this._writerIndex + steps) / _defaultSize) + 1)));
                }
                catch (Exception exception)
                {
                    throw new BufferOverflowException(string.Format("Not enough space to write,now {0},advance {1}", this._writerIndex, steps), exception);
                }
            }
        }

        private void Read1BytesToVar()
        {
            this.CheckReaderOverflow(1);
            this._valBytes.Byte0 = this._buffer[this._readerIndex];
            this.AdvanceReader(1);
        }

        private void Read2BytesToVar()
        {
            this.CheckReaderOverflow(2);
            this._valBytes.Byte0 = this._buffer[this._readerIndex];
            this._valBytes.Byte1 = this._buffer[this._readerIndex + 1];
            this.AdvanceReader(2);
        }

        private void Read4BytesToVar()
        {
            this.CheckReaderOverflow(4);
            this._valBytes.Byte0 = this._buffer[this._readerIndex];
            this._valBytes.Byte1 = this._buffer[this._readerIndex + 1];
            this._valBytes.Byte2 = this._buffer[this._readerIndex + 2];
            this._valBytes.Byte3 = this._buffer[this._readerIndex + 3];
            this.AdvanceReader(4);
        }

        private void Read8BytesToVar()
        {
            this.CheckReaderOverflow(8);
            this._valBytes.Byte0 = this._buffer[this._readerIndex];
            this._valBytes.Byte1 = this._buffer[this._readerIndex + 1];
            this._valBytes.Byte2 = this._buffer[this._readerIndex + 2];
            this._valBytes.Byte3 = this._buffer[this._readerIndex + 3];
            this._valBytes.Byte4 = this._buffer[this._readerIndex + 4];
            this._valBytes.Byte5 = this._buffer[this._readerIndex + 5];
            this._valBytes.Byte6 = this._buffer[this._readerIndex + 6];
            this._valBytes.Byte7 = this._buffer[this._readerIndex + 7];
            this.AdvanceReader(8);
        }

        public bool ReadBoolean()
        {
            this.Read1BytesToVar();
            return (this._valBytes.Byte0 != 0);
        }

        public byte ReadByte()
        {
            this.Read1BytesToVar();
            return this._valBytes.Byte0;
        }

        public ByteArray ReadByteArray()
        {
            if (this.WillOverflow(8))
            {
                return null;
            }
            int size = this.ReadHeader(false);
            if (this.WillOverflow(size - 8))
            {
                this.Rewind(8);
                return null;
            }
            ByteArray array = new ByteArray(size, true);
            array.Append(this._buffer, this._readerIndex - 8, size);
            array.RefreshHeader();
            this.AdvanceReader(size - 8);
            return array;
        }

        public double ReadDouble()
        {
            this.Read8BytesToVar();
            return this._valBytes.DoubleVal;
        }

        public float ReadFloat()
        {
            this.Read4BytesToVar();
            return this._valBytes.FloatVal;
        }

        public int ReadHeader()
        {
            return this.ReadHeader(true);
        }

        public int ReadHeader(bool self)
        {
            this.RefreshHeader();
            int num = this.ReadInt();
            this.AdvanceReader(4);
            return num;
        }

        public int ReadInt()
        {
            this.Read4BytesToVar();
            return this._valBytes.IntVal;
        }

        public short ReadShort()
        {
            this.Read2BytesToVar();
            return this._valBytes.ShortVal;
        }

        public string ReadUTF()
        {
            string outValue = string.Empty;
            short bufferLength = 0;
            if (!this.SafeReadUTF(ref outValue, ref bufferLength))
            {
                throw new BufferOverflowException("Not enough space to read string, len {0}", new object[] { bufferLength });
            }
            return outValue;
        }

        public void RefreshHeader()
        {
            this._valBytes.IntVal = this._writerIndex;
            this._buffer[0] = this._valBytes.Byte0;
            this._buffer[1] = this._valBytes.Byte1;
            this._buffer[2] = this._valBytes.Byte2;
            this._buffer[3] = this._valBytes.Byte3;
        }

        public void Reset()
        {
            this._writerIndex = 0;
            this._readerIndex = 0;
        }

        public void Rewind()
        {
            this._readerIndex = 0;
        }

        public void Rewind(int steps)
        {
            this._readerIndex -= Math.Min(this._readerIndex, steps);
        }

        public bool SafeReadUTF(ref string outValue)
        {
            short bufferLength = 0;
            return this.SafeReadUTF(ref outValue, ref bufferLength);
        }

        public bool SafeReadUTF(ref string outValue, ref short bufferLength)
        {
            bufferLength = this.ReadShort();
            if (this.WillOverflow(bufferLength))
            {
                this.Rewind(2);
                return false;
            }
            if (bufferLength == 0)
            {
                outValue = string.Empty;
                return true;
            }
            outValue = Encoding.UTF8.GetString(this._buffer, this._readerIndex, bufferLength);
            this.AdvanceReader(bufferLength);
            return true;
        }

        public void Shrink()
        {
            if ((this._buffer.Length >= _dangerousSize) && (this._writerIndex >= (this._buffer.Length * 0.5f)))
            {
                int num = Math.Max(((this._buffer.Length - this._readerIndex) / _defaultSize) * _defaultSize, _defaultSize);
                try
                {
                    byte[] destinationArray = new byte[num];
                    Array.Copy(this._buffer, this._readerIndex - 8, destinationArray, 0, (this._writerIndex - this._readerIndex) + 8);
                    this._writerIndex = (this._writerIndex - this._readerIndex) + 8;
                    this._readerIndex = 8;
                    this._buffer = destinationArray;
                    this.RefreshHeader();
                }
                catch (Exception exception)
                {
                    throw new BufferOverflowException(string.Format("ByteArray shrink error,from {0} to {1}", this._buffer.Length, num), exception);
                }
            }
        }

        public bool WillOverflow(int steps)
        {
            return ((this._readerIndex + steps) > this._writerIndex);
        }

        private void Write1ByteFromVar()
        {
            this.CheckWriterOverflow(1);
            this._buffer[this._writerIndex] = this._valBytes.Byte0;
            this.AdvanceWriter(1);
        }

        private void Write2ByteFromVar()
        {
            this.CheckWriterOverflow(2);
            this._buffer[this._writerIndex] = this._valBytes.Byte0;
            this._buffer[this._writerIndex + 1] = this._valBytes.Byte1;
            this.AdvanceWriter(2);
        }

        private void Write4ByteFromVar()
        {
            this.CheckWriterOverflow(4);
            this._buffer[this._writerIndex] = this._valBytes.Byte0;
            this._buffer[this._writerIndex + 1] = this._valBytes.Byte1;
            this._buffer[this._writerIndex + 2] = this._valBytes.Byte2;
            this._buffer[this._writerIndex + 3] = this._valBytes.Byte3;
            this.AdvanceWriter(4);
        }

        private void Write8ByteFromVar()
        {
            this.CheckWriterOverflow(8);
            this._buffer[this._writerIndex] = this._valBytes.Byte0;
            this._buffer[this._writerIndex + 1] = this._valBytes.Byte1;
            this._buffer[this._writerIndex + 2] = this._valBytes.Byte2;
            this._buffer[this._writerIndex + 3] = this._valBytes.Byte3;
            this._buffer[this._writerIndex + 4] = this._valBytes.Byte4;
            this._buffer[this._writerIndex + 5] = this._valBytes.Byte5;
            this._buffer[this._writerIndex + 6] = this._valBytes.Byte6;
            this._buffer[this._writerIndex + 7] = this._valBytes.Byte7;
            this.AdvanceWriter(8);
        }

        public void WriteBoolean(bool bv)
        {
            if (bv)
            {
                this._valBytes.Byte0 = 1;
            }
            else
            {
                this._valBytes.Byte0 = 0;
            }
            this.Write1ByteFromVar();
        }

        public void WriteByte(byte bv)
        {
            this._valBytes.Byte0 = bv;
            this.Write1ByteFromVar();
        }

        public void WriteByteArray(ByteArray byteArray)
        {
            this.CheckWriterOverflow(byteArray.Length);
            this.Append(byteArray.Buffer, 0, byteArray.Length);
        }

        public void WriteDouble(double dv)
        {
            this._valBytes.DoubleVal = dv;
            this.Write8ByteFromVar();
        }

        public void WriteFloat(float fv)
        {
            this._valBytes.FloatVal = fv;
            this.Write4ByteFromVar();
        }

        public void WriteHeader()
        {
            this.RefreshHeader();
            this.AdvanceWriter(8);
        }

        public void WriteInt(int iv)
        {
            this._valBytes.IntVal = iv;
            this.Write4ByteFromVar();
        }

        public void WriteShort(short sv)
        {
            this._valBytes.ShortVal = sv;
            this.Write2ByteFromVar();
        }

        public void WriteUTF(string str)
        {
            if ((str == null) || (str.Length == 0))
            {
                this.WriteShort(0);
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                short length = (short) bytes.Length;
                this.WriteShort(length);
                if (length != 0)
                {
                    this.CheckWriterOverflow(length);
                    bytes.CopyTo(this._buffer, this._writerIndex);
                    this.AdvanceWriter(length);
                }
            }
        }

        public byte[] Buffer
        {
            get
            {
                this.RefreshHeader();
                return this._buffer;
            }
        }

        public int Length
        {
            get
            {
                return this._writerIndex;
            }
        }
    }
}

