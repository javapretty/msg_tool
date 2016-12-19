using System;
using System.Collections;
using System.Text; 
namespace Msg_Tool
{
	public class Bit_Buffer
	{
		protected byte[] data_ = new byte[1460];
		protected int byte_size_ = 0;
		protected int w_bit_pos_ = 0;
		protected int w_byte_pos_ = 0;
        protected int r_bit_pos_ = 0;
        protected int r_byte_pos_ = 0;
				
		public Bit_Buffer()
		{

		}

        public Bit_Buffer(byte[] data, int length)
        {
            ensure_space(length);
            Array.Copy(data, 0, data_, 0, length);
            byte_size_ = length;
            w_byte_pos_ = length;
            w_bit_pos_ = 0;
        }

		public byte[] data {
			get {
                return data_;
			}

			set {
                ensure_space(value.Length);
                Array.Copy(value, 0, data_, 0, value.Length);
                byte_size_ = value.Length;
				w_byte_pos_ 	= 0;
				w_bit_pos_ 	= 0;
			}
		}

		public int length {
			get {
				return byte_size_ * 8;
			}
		}

		public int position {
			get {
				return w_byte_pos_ * 8 + w_bit_pos_;
			}

			set {
				w_byte_pos_ = value / 8;
				w_bit_pos_ = value & 0x7;

				if (w_byte_pos_ >= byte_size_) 
					w_bit_pos_ = byte_size_;
			}
		}

        private void ensure_space(int size)
        {
            if ((data_.Length - w_byte_pos_) < size)
            {
                byte[] temp = new byte[w_byte_pos_ + size + 1];
                Array.Copy(data_, 0, temp, 0, w_byte_pos_);
                data_ = temp;
                byte_size_ += size;
            }
        }

        public byte[] rdata()
        {
            byte[] data = new byte[readable_length()];
            Array.Copy(data_, r_byte_pos_, data, 0, readable_length());
            return data;
        }

		public void write_bool(Boolean b)
		{
            ensure_space(1);

			this._write_flag(b);
		}
		
		public void write_int(int i, int bits)
		{
			if(bits == 0) {
				return;
			}

            ensure_space(4);

			if(i < 0) {
				this._write_flag(true);
				this._write_uint32((uint)-i, bits-1);
			}
			else {
				this._write_flag(false);
				this._write_uint32((uint)i, bits-1);
			}
		}
		
		public void write_uint(uint i, int bits)
		{
            ensure_space(4);
			
			this._write_uint32(i, bits);
		}

        public void write_int64(long i)
        {
            ensure_space(8);

            /*byte[] value = BitConverter.GetBytes(i);
            uint high = BitConverter.ToUInt32(value, 0);
            uint low = BitConverter.ToUInt32(value, 4);*/
            uint high = (uint)(i >> 32);
            uint low = (uint)(i & 0xffffffff);
            _write_uint32(high, 32);
            _write_uint32(low, 32);
        }   

        public void write_uint64(ulong i)
        {
            ensure_space(8);

          /*  byte[] value = BitConverter.GetBytes(i);
            uint high = BitConverter.ToUInt32(value, 0);
            uint low = BitConverter.ToUInt32(value, 4);
            */
            uint high = (uint)(i >> 32);
            uint low = (uint)(i & 0xffffffff);
            _write_uint32(high, 32);
            _write_uint32(low, 32);
        }   

		public void write_decimal(float f, int bits)
		{
			this.write_uint((uint)(((f + 1.0f) * 0.5f) * ((1 << bits) - 1)), bits);
		}
		
		public void writeUdecimal(float f, int bits)
		{
			this.write_uint((uint)(f * ((1 << bits) - 1)), bits);
		}
		
		// TO DO : encode str with huffman tree?
		public void write_string(string str)
		{
			
			byte[] charAry = System.Text.Encoding.UTF8.GetBytes(str);

			if(w_bit_pos_ > 0) {
				++w_byte_pos_;
				w_bit_pos_ = 0; // give up rest bits in current byte
			}

            ensure_space(charAry.Length + 1);
			for (int i = 0; i < charAry.Length; ++i)
				data_[w_byte_pos_++] =  charAry[i];

			data_[w_byte_pos_++] = 0;

			if (w_byte_pos_ > byte_size_)
				byte_size_ = w_byte_pos_;
		}
		
        public int readable_length()
        {
            return w_byte_pos_ + (w_bit_pos_ > 0 ? 1 : 0) - r_byte_pos_;
        }

		// notice: some bits may not use in last byte
        public int read_bits_available()
        {
            int available_size = (w_byte_pos_ * 8 + w_bit_pos_) - (r_byte_pos_ * 8 + r_bit_pos_);
            if (available_size < 0)
                available_size = 0;
            return available_size;
		}
		
		public bool read_bool()
		{
			return this._read_flag();
		}
		
		public int read_int(int bits)
		{
			if(bits == 0)
			{
				return 0;
			}
			
			if(this._read_flag())
			{
				return - (int)(this._read_uint32(bits-1));
			}
			else
			{
				return (int)(this._read_uint32(bits-1));
			}
		}
		
		public uint read_uint(int bits)
		{
			return this._read_uint32(bits);
		}
		
        public long read_int64() 
        {
            uint high = _read_uint32(32);
            uint low = _read_uint32(32);
            return (long)high << 32 | low;
        }

        public ulong read_uint64() {
            uint high = _read_uint32(32);
            uint low = _read_uint32(32);
            return (ulong)high << 32 | low;
        }

		public float read_decimal(int bits)
		{
			return (float)(this._read_uint32(bits) * 2.0f / (float)((1 << bits) - 1) - 1.0f);
		}
		
		public float readUdecimal(int bits)
		{
			return (float)this._read_uint32(bits) / (float)((1 << bits) - 1);
		}
		
		public string read_string()
		{
			if (r_byte_pos_ >= byte_size_)
				return null;

			if(r_bit_pos_ > 0)
			{
                ++r_byte_pos_;
				r_bit_pos_ = 0; // give up rest bits in current byte
			}

            int offset = r_byte_pos_;
            while (data_[r_byte_pos_++] != 0) ;
            string ret = System.Text.Encoding.UTF8.GetString(data_, offset, r_byte_pos_ - offset - 1);

			return ret;
		}
		
		private bool _write_flag(bool b)
		{
			if(b)
				data_[w_byte_pos_] |= (byte)(0x1 << w_bit_pos_);
			else
				data_[w_byte_pos_] &= (byte)(~(0x1 << w_bit_pos_));
			
			++w_bit_pos_;
			if(w_bit_pos_ >= 8) {
				++w_byte_pos_;
				w_bit_pos_ &= 0x7;
			}

			if (w_byte_pos_ >= byte_size_) {
				byte_size_ = w_byte_pos_ + (w_bit_pos_ > 0 ? 1 : 0);
			}
			
			return b;
		}

		private bool _read_flag()
		{
			if (r_byte_pos_ >= byte_size_)
				return default(bool);

			bool ret = (data_[r_byte_pos_] & (0x1 << r_bit_pos_)) != 0;
			
			++r_bit_pos_;
			if(r_bit_pos_ >= 8) {
				++r_byte_pos_;
				r_bit_pos_ &= 0x7;
			}
			
			return ret;
		}

		private void _write_uint32(uint i, int bits)
		{
			if(bits > 32) {
				bits = 32;
			}
			
			for(int rest_bits = bits; rest_bits > 0;) {
				int empty_bits = 8 - (int)w_bit_pos_;
				int to_fill_bits = empty_bits > (int)rest_bits ? (int)rest_bits : empty_bits;
				
				data_[w_byte_pos_] = (byte)((uint)(data_[w_byte_pos_] & ((0x1 << w_bit_pos_) - 1)) | (uint)(((i >> (bits-rest_bits)) & ((0x1 << to_fill_bits) - 1)) << w_bit_pos_) );
				
				w_bit_pos_ += to_fill_bits;
				if(w_bit_pos_ >= 8) {
					++w_byte_pos_;
					w_bit_pos_ &= 0x7;
				}
				
				rest_bits -= to_fill_bits;
			}

			if (w_byte_pos_ >= byte_size_) {
				byte_size_ = w_byte_pos_ + (w_bit_pos_ > 0 ? 1 : 0);
			}
		}
		private uint _read_uint32(int bits)
		{
			if (r_byte_pos_ >= byte_size_)
				return default(uint);

			if(bits > 32) {
				bits = 32;
			}
			
			uint ret = 0;
			for(int rest_bits = bits; rest_bits > 0;)
			{
				int next_bits = 8 - r_bit_pos_;
				int to_read_bits = next_bits > rest_bits ? rest_bits : next_bits;
				
				ret |= (uint)(((data_[r_byte_pos_] >> (int)r_bit_pos_) & ((0x1 << to_read_bits) - 1)) << (bits - rest_bits));
				
				r_bit_pos_ += to_read_bits;
				if(r_bit_pos_ >= 8)
				{
					++r_byte_pos_;
					r_bit_pos_ &= 0x7;
				}
				
				rest_bits -= to_read_bits;
			}
			
			return ret;
		}
	}
}

