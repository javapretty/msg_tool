/*
 * Byte_Buffer.cs
 *
 *  Created on: Dec 12, 2016
 *      Author: zhangyalei
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msg_Tool
{
    class Byte_Buffer
    {
        private const int TCP_PACKET_SIZE = 1460;
        private byte[] data_ = null;
        private int w_pos_ = 0;
        private int r_pos_ = 0;
        public static System.Text.ASCIIEncoding strcover_ = new System.Text.ASCIIEncoding();

        public Byte_Buffer()
        {
            data_ = new byte[TCP_PACKET_SIZE];
        }

        public int readable_length() 
        {
            return w_pos_ - r_pos_;
        }

        public int size()
        {
            return data_.Length;
        }

        public byte[] data()
        {
            return data_;
        }

        public int rpos
        {
            get { return r_pos_; }
            set { r_pos_ = value; }
        }

        public int wpos
        {
            get { return w_pos_; }
            set { w_pos_ = value; }
        }

        public byte[] rdata()
        {
            byte[] data = new byte[readable_length()];
            Array.Copy(data_, r_pos_, data, 0, readable_length());
            return data;
        }

        public void reset()
        {
            data_ = null;
            w_pos_ = 0;
            r_pos_ = 0;
        }

        public void make_tcp_begin()
        {
            write_uint16(0);
        }

        public void make_tcp_end()
        {
            uint len = (UInt16)(readable_length() - 2);
            UInt16 length = (UInt16)make_rpc_pkg_header(len, false);
            byte[] data = BitConverter.GetBytes(length);
            for (int i = 0; i < 2; i++)
            {
                data_[i] = data[i];
            }
        }

        public void copy(Byte_Buffer buffer)
        {
            copy(buffer.rdata(), buffer.readable_length());
        }

        public void copy(Bit_Buffer buffer)
        {
            copy(buffer.rdata(), buffer.readable_length());
        }

        public void copy(byte[] data, int length)
        {
            write(data, length);
        }

        public void read_complete()
        {
            rpos = wpos;
        }

        private uint make_rpc_pkg_header(uint length, bool comp)
        {
            uint com = (uint)(comp == true ? 1 : 0);
            return (0x1 << 6 | ((com & 0x1) << 5) | ((length & 0x1f00) >> 8) | ((length & 0x00ff) << 8));
        }

        private void write(byte[] data, int length)
        {
            write(data, 0, length);
        }

        private void write(byte[] data, int start_index, int length)
        {
            ensure_space(length);
            for (int i = 0; i < length; i++)
            {
                data_[w_pos_] = data[start_index + i];
                w_pos_++;
            }
        }

        public void write_int8(int value)
        {
            byte[] data = {(byte)value};
            write(data, 1);
        }

        public void write_int16(int value)
        {
            Int16 v = (Int16)value;
            write(BitConverter.GetBytes(v), 2);
        }

        public void write_int32(int value)
        {
            Int32 v = (Int32)value;
            write(BitConverter.GetBytes(v), 4);
        }

        public void write_int64(long value)
        {
            Int64 v = (Int64)value;
            write(BitConverter.GetBytes(v), 8);
        }

        public void write_uint8(uint value)
        {
            byte[] data = { (byte)value };
            write(data, 1);
        }

        public void write_uint16(uint value)
        {
            UInt16 v = (UInt16)value;
            write(BitConverter.GetBytes(v), 2);
        }

        public void write_uint32(uint value)
        {
            UInt32 v = (UInt32)value;
            write(BitConverter.GetBytes(v), 4);
        }

        public void write_uint64(ulong value)
        {
            UInt64 v = (UInt64)value;
            write(BitConverter.GetBytes(v), 8);
        }

        public void write_double(double value)
        {
            write(BitConverter.GetBytes(value), 8);
        }

        public void write_bool(bool value)
        {
            write(BitConverter.GetBytes(value), 1);
        }

        public void write_string(string value)
        {
            write_uint16((UInt16)value.Length);
            write(strcover_.GetBytes(value), value.Length);
        }

        public int read_int8()
        {
            r_pos_ += 1;
            return (int)data_[r_pos_ - 1];
        }

        public int read_int16()
        {
            r_pos_ += 2;
            return BitConverter.ToInt16(data_, r_pos_ - 2);
        }

        public int read_int32()
        {
            r_pos_ += 4;
            return BitConverter.ToInt32(data_, r_pos_ - 4);
        }

        public long read_int64()
        {
            r_pos_ += 8;
            return BitConverter.ToInt64(data_, r_pos_ - 8);
        }

        public uint read_uint8()
        {
            r_pos_ += 1;
            return (uint)data_[r_pos_ - 1];
        }

        public uint read_uint16()
        {
            r_pos_ += 2;
            return BitConverter.ToUInt16(data_, r_pos_ - 2);
        }

        public uint read_uint32()
        {
            r_pos_ += 4;
            return BitConverter.ToUInt32(data_, r_pos_ - 4);
        }

        public ulong read_uint64()
        {
            r_pos_ += 8;
            return BitConverter.ToUInt64(data_, r_pos_ - 8);
        }

        public double read_double()
        {
            r_pos_ += 8;
            return BitConverter.ToDouble(data_, r_pos_ - 8);
        }

        public bool read_bool()
        {
            r_pos_ += 1;
            return BitConverter.ToBoolean(data_, r_pos_ - 1);
        }

        public string read_string()
        {
            uint length = read_uint16();
            r_pos_ += (int)length;
            Log.debug_log("length is "+length.ToString());
            return strcover_.GetString(data_, r_pos_ - (int)length, (int)length);
        }

        private void ensure_space(int size)
        {
            if ((data_.Length - w_pos_) < size)
            {
                byte[] temp = new byte[w_pos_ + size + 1];
                Array.Copy(data_, 0, temp, 0, w_pos_);
                data_ = temp;
            }
        }
    }
}
