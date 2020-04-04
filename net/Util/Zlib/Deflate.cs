// Copyright (c) 2006, ComponentAce
// http://www.componentace.com
// All rights reserved.

// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

// Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
// Neither the name of ComponentAce nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

/*
Copyright (c) 2000,2001,2002,2003 ymnk, JCraft,Inc. All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright 
notice, this list of conditions and the following disclaimer in 
the documentation and/or other materials provided with the distribution.

3. The names of the authors may not be used to endorse or promote products
derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL JCRAFT,
INC. OR ANY CONTRIBUTORS TO THIS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
/*
* This program is based on zlib-1.1.3, so all credit should go authors
* Jean-loup Gailly(jloup@gzip.org) and Mark Adler(madler@alumni.caltech.edu)
* and contributors of zlib.
*/
using System;

namespace Util.Zlib
{
    public sealed class Deflate
    {
        private const Int32 MAX_MEM_LEVEL = 9;

        private const Int32 Z_DEFAULT_COMPRESSION = -1;

        private const Int32 MAX_WBITS = 15; // 32K LZ77 window
        private const Int32 DEF_MEM_LEVEL = 8;

        internal class Config
        {
            internal Int32 good_length; // reduce lazy search above this match length
            internal Int32 max_lazy; // do not perform lazy search above this match length
            internal Int32 nice_length; // quit search above this match length
            internal Int32 max_chain;
            internal Int32 func;
            internal Config(Int32 good_length, Int32 max_lazy, Int32 nice_length, Int32 max_chain, Int32 func)
            {
                this.good_length = good_length;
                this.max_lazy = max_lazy;
                this.nice_length = nice_length;
                this.max_chain = max_chain;
                this.func = func;
            }
        }

        private const Int32 STORED = 0;
        private const Int32 FAST = 1;
        private const Int32 SLOW = 2;
        private static Config[] config_table;

        private static readonly System.String[] z_errmsg = new System.String[] { "need dictionary", "stream end", String.Empty, "file error", "stream error", "data error", "insufficient memory", "buffer error", "incompatible version", "" };

        // block not completed, need more input or more output
        private const Int32 NeedMore = 0;

        // block flush performed
        private const Int32 BlockDone = 1;

        // finish started, need only more output at next deflate
        private const Int32 FinishStarted = 2;

        // finish done, accept no more input or output
        private const Int32 FinishDone = 3;

        // preset dictionary flag in zlib header
        private const Int32 PRESET_DICT = 0x20;

        private const Int32 Z_FILTERED = 1;
        private const Int32 Z_HUFFMAN_ONLY = 2;
        private const Int32 Z_DEFAULT_STRATEGY = 0;

        private const Int32 Z_NO_FLUSH = 0;
        private const Int32 Z_PARTIAL_FLUSH = 1;
        private const Int32 Z_SYNC_FLUSH = 2;
        private const Int32 Z_FULL_FLUSH = 3;
        private const Int32 Z_FINISH = 4;

        private const Int32 Z_OK = 0;
        private const Int32 Z_STREAM_END = 1;
        private const Int32 Z_NEED_DICT = 2;
        private const Int32 Z_ERRNO = -1;
        private const Int32 Z_STREAM_ERROR = -2;
        private const Int32 Z_DATA_ERROR = -3;
        private const Int32 Z_MEM_ERROR = -4;
        private const Int32 Z_BUF_ERROR = -5;
        private const Int32 Z_VERSION_ERROR = -6;

        private const Int32 INIT_STATE = 42;
        private const Int32 BUSY_STATE = 113;
        private const Int32 FINISH_STATE = 666;

        // The deflate compression method
        private const Int32 Z_DEFLATED = 8;

        private const Int32 STORED_BLOCK = 0;
        private const Int32 STATIC_TREES = 1;
        private const Int32 DYN_TREES = 2;

        // The three kinds of block type
        private const Int32 Z_BINARY = 0;
        private const Int32 Z_ASCII = 1;
        private const Int32 Z_UNKNOWN = 2;

        private const Int32 Buf_size = 8 * 2;

        // repeat previous bit length 3-6 times (2 bits of repeat count)
        private const Int32 REP_3_6 = 16;

        // repeat a zero length 3-10 times  (3 bits of repeat count)
        private const Int32 REPZ_3_10 = 17;

        // repeat a zero length 11-138 times  (7 bits of repeat count)
        private const Int32 REPZ_11_138 = 18;

        private const Int32 MIN_MATCH = 3;
        private const Int32 MAX_MATCH = 258;
        private static readonly Int32 MIN_LOOKAHEAD = (MAX_MATCH + MIN_MATCH + 1);

        private const Int32 MAX_BITS = 15;
        private const Int32 D_CODES = 30;
        private const Int32 BL_CODES = 19;
        private const Int32 LENGTH_CODES = 29;
        private const Int32 LITERALS = 256;
        private static readonly Int32 L_CODES = (LITERALS + 1 + LENGTH_CODES);
        private static readonly Int32 HEAP_SIZE = (2 * L_CODES + 1);

        private const Int32 END_BLOCK = 256;

        internal ZStream strm; // pointer back to this zlib stream
        internal Int32 status; // as the name implies
        internal Byte[] pending_buf; // output still pending
        internal Int32 pending_buf_size; // size of pending_buf
        internal Int32 pending_out; // next pending Byte to output to the stream
        internal Int32 pending; // nb of Bytes in the pending buffer
        internal Int32 noheader; // suppress zlib header and adler32
        internal Byte data_type; // UNKNOWN, BINARY or ASCII
        internal Byte method; // STORED (for zip only) or DEFLATED
        internal Int32 last_flush; // value of flush param for previous deflate call

        internal Int32 w_size; // LZ77 window size (32K by default)
        internal Int32 w_bits; // log2(w_size)  (8..16)
        internal Int32 w_mask; // w_size - 1

        internal Byte[] window;
        // Sliding window. Input Bytes are read into the second half of the window,
        // and move to the first half later to keep a dictionary of at least wSize
        // Bytes. With this organization, matches are limited to a distance of
        // wSize-MAX_MATCH Bytes, but this ensures that IO is always
        // performed with a length multiple of the block size. Also, it limits
        // the window size to 64K, which is quite useful on MSDOS.
        // To do: use the user input buffer as sliding window.

        internal Int32 window_size;
        // Actual size of window: 2*wSize, except when the user input buffer
        // is directly used as sliding window.

        internal Int16[] prev;
        // Link to older string with same hash index. To limit the size of this
        // array to 64K, this link is maintained only for the last 32K strings.
        // An index in this array is thus a window index modulo 32K.

        internal Int16[] head; // Heads of the hash chains or NIL.

        internal Int32 ins_h; // hash index of string to be inserted
        internal Int32 hash_size; // number of elements in hash table
        internal Int32 hash_bits; // log2(hash_size)
        internal Int32 hash_mask; // hash_size-1

        // Number of bits by which ins_h must be shifted at each input
        // step. It must be such that after MIN_MATCH steps, the oldest
        // Byte no longer takes part in the hash key, that is:
        // hash_shift * MIN_MATCH >= hash_bits
        internal Int32 hash_shift;

        // Window position at the beginning of the current output block. Gets
        // negative when the window is moved backwards.

        internal Int32 block_start;

        internal Int32 match_length; // length of best match
        internal Int32 prev_match; // previous match
        internal Int32 match_available; // set if previous match exists
        internal Int32 strstart; // start of string to insert
        internal Int32 match_start; // start of matching string
        internal Int32 lookahead; // number of valid Bytes ahead in window

        // Length of the best match at previous step. Matches not greater than this
        // are discarded. This is used in the lazy match evaluation.
        internal Int32 prev_length;

        // To speed up deflation, hash chains are never searched beyond this
        // length.  A higher limit improves compression ratio but degrades the speed.
        internal Int32 max_chain_length;

        // Attempt to find a better match only when the current match is strictly
        // smaller than this value. This mechanism is used only for compression
        // levels >= 4.
        internal Int32 max_lazy_match;

        // Insert new strings in the hash table only if the match length is not
        // greater than this length. This saves time but degrades compression.
        // max_insert_length is used only for compression levels <= 3.

        internal Int32 level; // compression level (1..9)
        internal Int32 strategy; // favor or force Huffman coding

        // Use a faster search when the previous match is longer than this
        internal Int32 good_match;

        // Stop searching when current match exceeds this
        internal Int32 nice_match;

        internal Int16[] dyn_ltree; // literal and length tree
        internal Int16[] dyn_dtree; // distance tree
        internal Int16[] bl_tree; // Huffman tree for bit lengths

        internal Tree l_desc = new Tree(); // desc for literal tree
        internal Tree d_desc = new Tree(); // desc for distance tree
        internal Tree bl_desc = new Tree(); // desc for bit length tree

        // number of codes at each bit length for an optimal tree
        internal Int16[] bl_count = new Int16[MAX_BITS + 1];

        // heap used to build the Huffman trees
        internal Int32[] heap = new Int32[2 * L_CODES + 1];

        internal Int32 heap_len; // number of elements in the heap
        internal Int32 heap_max; // element of largest frequency
        // The sons of heap[n] are heap[2*n] and heap[2*n+1]. heap[0] is not used.
        // The same heap array is used to build all trees.

        // Depth of each subtree used as tie breaker for trees of equal frequency
        internal Byte[] depth = new Byte[2 * L_CODES + 1];

        internal Int32 l_buf; // index for literals or lengths */

        // Size of match buffer for literals/lengths.  There are 4 reasons for
        // limiting lit_bufsize to 64K:
        //   - frequencies can be kept in 16 bit counters
        //   - if compression is not successful for the first block, all input
        //     data is still in the window so we can still emit a stored block even
        //     when input comes from standard input.  (This can also be done for
        //     all blocks if lit_bufsize is not greater than 32K.)
        //   - if compression is not successful for a file smaller than 64K, we can
        //     even emit a stored file instead of a stored block (saving 5 Bytes).
        //     This is applicable only for zip (not gzip or zlib).
        //   - creating new Huffman trees less frequently may not provide fast
        //     adaptation to changes in the input data statistics. (Take for
        //     example a binary file with poorly compressible code followed by
        //     a highly compressible string table.) Smaller buffer sizes give
        //     fast adaptation but have of course the overhead of transmitting
        //     trees more frequently.
        //   - I can't count above 4
        internal Int32 lit_bufsize;

        internal Int32 last_lit; // running index in l_buf

        // Buffer for distances. To simplify the code, d_buf and l_buf have
        // the same number of elements. To use different lengths, an extra flag
        // array would be necessary.

        internal Int32 d_buf; // index of pendig_buf

        internal Int32 opt_len; // bit length of current block with optimal trees
        internal Int32 static_len; // bit length of current block with static trees
        internal Int32 matches; // number of string matches in current block
        internal Int32 last_eob_len; // bit length of EOB code for last block

        // Output buffer. bits are inserted starting at the bottom (least
        // significant bits).
        internal Int16 bi_buf;

        // Number of valid bits in bi_buf.  All bits above the last valid bit
        // are always zero.
        internal Int32 bi_valid;

        internal Deflate()
        {
            dyn_ltree = new Int16[HEAP_SIZE * 2];
            dyn_dtree = new Int16[(2 * D_CODES + 1) * 2]; // distance tree
            bl_tree = new Int16[(2 * BL_CODES + 1) * 2]; // Huffman tree for bit lengths
        }

        internal void lm_init()
        {
            window_size = 2 * w_size;

            head[hash_size - 1] = 0;
            for (Int32 i = 0; i < hash_size - 1; i++)
            {
                head[i] = 0;
            }

            // Set the default configuration parameters:
            max_lazy_match = Deflate.config_table[level].max_lazy;
            good_match = Deflate.config_table[level].good_length;
            nice_match = Deflate.config_table[level].nice_length;
            max_chain_length = Deflate.config_table[level].max_chain;

            strstart = 0;
            block_start = 0;
            lookahead = 0;
            match_length = prev_length = MIN_MATCH - 1;
            match_available = 0;
            ins_h = 0;
        }

        // Initialize the tree data structures for a new zlib stream.
        internal void tr_init()
        {

            l_desc.dyn_tree = dyn_ltree;
            l_desc.stat_desc = StaticTree.static_l_desc;

            d_desc.dyn_tree = dyn_dtree;
            d_desc.stat_desc = StaticTree.static_d_desc;

            bl_desc.dyn_tree = bl_tree;
            bl_desc.stat_desc = StaticTree.static_bl_desc;

            bi_buf = 0;
            bi_valid = 0;
            last_eob_len = 8; // enough lookahead for inflate

            // Initialize the first block of the first file:
            init_block();
        }

        internal void init_block()
        {
            // Initialize the trees.
            for (Int32 i = 0; i < L_CODES; i++)
                dyn_ltree[i * 2] = 0;
            for (Int32 i = 0; i < D_CODES; i++)
                dyn_dtree[i * 2] = 0;
            for (Int32 i = 0; i < BL_CODES; i++)
                bl_tree[i * 2] = 0;

            dyn_ltree[END_BLOCK * 2] = 1;
            opt_len = static_len = 0;
            last_lit = matches = 0;
        }

        // Restore the heap property by moving down the tree starting at node k,
        // exchanging a node with the smallest of its two sons if necessary, stopping
        // when the heap property is re-established (each father smaller than its
        // two sons).
        internal void pqdownheap(Int16[] tree, Int32 k)
        {
            Int32 v = heap[k];
            Int32 j = k << 1; // left son of k
            while (j <= heap_len)
            {
                // Set j to the smallest of the two sons:
                if (j < heap_len && smaller(tree, heap[j + 1], heap[j], depth))
                {
                    j++;
                }
                // Exit if v is smaller than both sons
                if (smaller(tree, v, heap[j], depth))
                    break;

                // Exchange v with the smallest son
                heap[k] = heap[j]; k = j;
                // And continue down the tree, setting j to the left son of k
                j <<= 1;
            }
            heap[k] = v;
        }

        internal static Boolean smaller(Int16[] tree, Int32 n, Int32 m, Byte[] depth)
        {
            return (tree[n * 2] < tree[m * 2] || (tree[n * 2] == tree[m * 2] && depth[n] <= depth[m]));
        }

        // Scan a literal or distance tree to determine the frequencies of the codes
        // in the bit length tree.
        internal void scan_tree(Int16[] tree, Int32 max_code)
        {
            Int32 n; // iterates over all tree elements
            Int32 prevlen = -1; // last emitted length
            Int32 curlen; // length of current code
            Int32 nextlen = tree[0 * 2 + 1]; // length of next code
            Int32 count = 0; // repeat count of the current code
            Int32 max_count = 7; // max repeat count
            Int32 min_count = 4; // min repeat count

            if (nextlen == 0)
            {
                max_count = 138; min_count = 3;
            }
            tree[(max_code + 1) * 2 + 1] = (Int16)SupportClass.Identity(0xffff); // guard

            for (n = 0; n <= max_code; n++)
            {
                curlen = nextlen; nextlen = tree[(n + 1) * 2 + 1];
                if (++count < max_count && curlen == nextlen)
                {
                    continue;
                }
                else if (count < min_count)
                {
                    bl_tree[curlen * 2] = (Int16)(bl_tree[curlen * 2] + count);
                }
                else if (curlen != 0)
                {
                    if (curlen != prevlen)
                        bl_tree[curlen * 2]++;
                    bl_tree[REP_3_6 * 2]++;
                }
                else if (count <= 10)
                {
                    bl_tree[REPZ_3_10 * 2]++;
                }
                else
                {
                    bl_tree[REPZ_11_138 * 2]++;
                }
                count = 0; prevlen = curlen;
                if (nextlen == 0)
                {
                    max_count = 138; min_count = 3;
                }
                else if (curlen == nextlen)
                {
                    max_count = 6; min_count = 3;
                }
                else
                {
                    max_count = 7; min_count = 4;
                }
            }
        }

        // Construct the Huffman tree for the bit lengths and return the index in
        // bl_order of the last bit length code to send.
        internal Int32 build_bl_tree()
        {
            Int32 max_blindex; // index of last bit length code of non zero freq

            // Determine the bit length frequencies for literal and distance trees
            scan_tree(dyn_ltree, l_desc.max_code);
            scan_tree(dyn_dtree, d_desc.max_code);

            // Build the bit length tree:
            bl_desc.build_tree(this);
            // opt_len now includes the length of the tree representations, except
            // the lengths of the bit lengths codes and the 5+5+4 bits for the counts.

            // Determine the number of bit length codes to send. The pkzip format
            // requires that at least 4 bit length codes be sent. (appnote.txt says
            // 3 but the actual value used is 4.)
            for (max_blindex = BL_CODES - 1; max_blindex >= 3; max_blindex--)
            {
                if (bl_tree[Tree.bl_order[max_blindex] * 2 + 1] != 0)
                    break;
            }
            // Update opt_len to include the bit length tree and counts
            opt_len += 3 * (max_blindex + 1) + 5 + 5 + 4;

            return max_blindex;
        }


        // Send the header for a block using dynamic Huffman trees: the counts, the
        // lengths of the bit length codes, the literal tree and the distance tree.
        // IN assertion: lcodes >= 257, dcodes >= 1, blcodes >= 4.
        internal void send_all_trees(Int32 lcodes, Int32 dcodes, Int32 blcodes)
        {
            Int32 rank; // index in bl_order

            send_bits(lcodes - 257, 5); // not +255 as stated in appnote.txt
            send_bits(dcodes - 1, 5);
            send_bits(blcodes - 4, 4); // not -3 as stated in appnote.txt
            for (rank = 0; rank < blcodes; rank++)
            {
                send_bits(bl_tree[Tree.bl_order[rank] * 2 + 1], 3);
            }
            send_tree(dyn_ltree, lcodes - 1); // literal tree
            send_tree(dyn_dtree, dcodes - 1); // distance tree
        }

        // Send a literal or distance tree in compressed form, using the codes in
        // bl_tree.
        internal void send_tree(Int16[] tree, Int32 max_code)
        {
            Int32 n; // iterates over all tree elements
            Int32 prevlen = -1; // last emitted length
            Int32 curlen; // length of current code
            Int32 nextlen = tree[0 * 2 + 1]; // length of next code
            Int32 count = 0; // repeat count of the current code
            Int32 max_count = 7; // max repeat count
            Int32 min_count = 4; // min repeat count

            if (nextlen == 0)
            {
                max_count = 138; min_count = 3;
            }

            for (n = 0; n <= max_code; n++)
            {
                curlen = nextlen; nextlen = tree[(n + 1) * 2 + 1];
                if (++count < max_count && curlen == nextlen)
                {
                    continue;
                }
                else if (count < min_count)
                {
                    do
                    {
                        send_code(curlen, bl_tree);
                    }
                    while (--count != 0);
                }
                else if (curlen != 0)
                {
                    if (curlen != prevlen)
                    {
                        send_code(curlen, bl_tree); count--;
                    }
                    send_code(REP_3_6, bl_tree);
                    send_bits(count - 3, 2);
                }
                else if (count <= 10)
                {
                    send_code(REPZ_3_10, bl_tree);
                    send_bits(count - 3, 3);
                }
                else
                {
                    send_code(REPZ_11_138, bl_tree);
                    send_bits(count - 11, 7);
                }
                count = 0; prevlen = curlen;
                if (nextlen == 0)
                {
                    max_count = 138; min_count = 3;
                }
                else if (curlen == nextlen)
                {
                    max_count = 6; min_count = 3;
                }
                else
                {
                    max_count = 7; min_count = 4;
                }
            }
        }

        // Output a Byte on the stream.
        // IN assertion: there is enough room in pending_buf.
        internal void put_Byte(Byte[] p, Int32 start, Int32 len)
        {
            Array.Copy(p, start, pending_buf, pending, len);
            pending += len;
        }

        internal void put_Byte(Byte c)
        {
            pending_buf[pending++] = c;
        }
        internal void put_Int16(Int32 w)
        {
            put_Byte((Byte)(w));
            put_Byte((Byte)(SupportClass.URShift(w, 8)));
        }
        internal void putInt16MSB(Int32 b)
        {
            put_Byte((Byte)(b >> 8));
            put_Byte((Byte)(b));
        }

        internal void send_code(Int32 c, Int16[] tree)
        {
            send_bits((tree[c * 2] & 0xffff), (tree[c * 2 + 1] & 0xffff));
        }

        internal void send_bits(Int32 value_Renamed, Int32 length)
        {
            Int32 len = length;
            if (bi_valid > (Int32)Buf_size - len)
            {
                Int32 val = value_Renamed;
                //      bi_buf |= (val << bi_valid);
                bi_buf = (Int16)((UInt16)bi_buf | (UInt16)(((val << bi_valid) & 0xffff)));
                put_Int16(bi_buf);
                bi_buf = (Int16)(SupportClass.URShift(val, (Buf_size - bi_valid)));
                bi_valid += len - Buf_size;
            }
            else
            {
                //      bi_buf |= (value) << bi_valid;
                bi_buf = (Int16)((UInt16)bi_buf | (UInt16)((((value_Renamed) << bi_valid) & 0xffff)));
                bi_valid += len;
            }
        }

        // Send one empty static block to give enough lookahead for inflate.
        // This takes 10 bits, of which 7 may remain in the bit buffer.
        // The current inflate code requires 9 bits of lookahead. If the
        // last two codes for the previous block (real code plus EOB) were coded
        // on 5 bits or less, inflate may have only 5+3 bits of lookahead to decode
        // the last real code. In this case we send two empty static blocks instead
        // of one. (There are no problems if the previous block is stored or fixed.)
        // To simplify the code, we assume the worst case of last real code encoded
        // on one bit only.
        internal void _tr_align()
        {
            send_bits(STATIC_TREES << 1, 3);
            send_code(END_BLOCK, StaticTree.static_ltree);

            bi_flush();

            // Of the 10 bits for the empty block, we have already sent
            // (10 - bi_valid) bits. The lookahead for the last real code (before
            // the EOB of the previous block) was thus at least one plus the length
            // of the EOB plus what we have just sent of the empty static block.
            if (1 + last_eob_len + 10 - bi_valid < 9)
            {
                send_bits(STATIC_TREES << 1, 3);
                send_code(END_BLOCK, StaticTree.static_ltree);
                bi_flush();
            }
            last_eob_len = 7;
        }


        // Save the match info and tally the frequency counts. Return true if
        // the current block must be flushed.
        internal Boolean _tr_tally(Int32 dist, Int32 lc)
        {

            pending_buf[d_buf + last_lit * 2] = (Byte)(SupportClass.URShift(dist, 8));
            pending_buf[d_buf + last_lit * 2 + 1] = (Byte)dist;

            pending_buf[l_buf + last_lit] = (Byte)lc; last_lit++;

            if (dist == 0)
            {
                // lc is the unmatched char
                dyn_ltree[lc * 2]++;
            }
            else
            {
                matches++;
                // Here, lc is the match length - MIN_MATCH
                dist--; // dist = match distance - 1
                dyn_ltree[(Tree._length_code[lc] + LITERALS + 1) * 2]++;
                dyn_dtree[Tree.d_code(dist) * 2]++;
            }

            if ((last_lit & 0x1fff) == 0 && level > 2)
            {
                // Compute an upper bound for the compressed length
                Int32 out_length = last_lit * 8;
                Int32 in_length = strstart - block_start;
                Int32 dcode;
                for (dcode = 0; dcode < D_CODES; dcode++)
                {
                    out_length = (Int32)(out_length + (Int32)dyn_dtree[dcode * 2] * (5L + Tree.extra_dbits[dcode]));
                }
                out_length = SupportClass.URShift(out_length, 3);
                if ((matches < (last_lit / 2)) && out_length < in_length / 2)
                    return true;
            }

            return (last_lit == lit_bufsize - 1);
            // We avoid equality with lit_bufsize because of wraparound at 64K
            // on 16 bit machines and because stored blocks are restricted to
            // 64K-1 Bytes.
        }

        // Send the block data compressed using the given Huffman trees
        internal void compress_block(Int16[] ltree, Int16[] dtree)
        {
            Int32 dist; // distance of matched string
            Int32 lc; // match length or unmatched char (if dist == 0)
            Int32 lx = 0; // running index in l_buf
            Int32 code; // the code to send
            Int32 extra; // number of extra bits to send

            if (last_lit != 0)
            {
                do
                {
                    dist = ((pending_buf[d_buf + lx * 2] << 8) & 0xff00) | (pending_buf[d_buf + lx * 2 + 1] & 0xff);
                    lc = (pending_buf[l_buf + lx]) & 0xff; lx++;

                    if (dist == 0)
                    {
                        send_code(lc, ltree); // send a literal Byte
                    }
                    else
                    {
                        // Here, lc is the match length - MIN_MATCH
                        code = Tree._length_code[lc];

                        send_code(code + LITERALS + 1, ltree); // send the length code
                        extra = Tree.extra_lbits[code];
                        if (extra != 0)
                        {
                            lc -= Tree.base_length[code];
                            send_bits(lc, extra); // send the extra length bits
                        }
                        dist--; // dist is now the match distance - 1
                        code = Tree.d_code(dist);

                        send_code(code, dtree); // send the distance code
                        extra = Tree.extra_dbits[code];
                        if (extra != 0)
                        {
                            dist -= Tree.base_dist[code];
                            send_bits(dist, extra); // send the extra distance bits
                        }
                    } // literal or match pair ?

                    // Check that the overlay between pending_buf and d_buf+l_buf is ok:
                }
                while (lx < last_lit);
            }

            send_code(END_BLOCK, ltree);
            last_eob_len = ltree[END_BLOCK * 2 + 1];
        }

        // Set the data type to ASCII or BINARY, using a crude approximation:
        // binary if more than 20% of the Bytes are <= 6 or >= 128, ascii otherwise.
        // IN assertion: the fields freq of dyn_ltree are set and the total of all
        // frequencies does not exceed 64K (to fit in an Int32 on 16 bit machines).
        internal void set_data_type()
        {
            Int32 n = 0;
            Int32 ascii_freq = 0;
            Int32 bin_freq = 0;
            while (n < 7)
            {
                bin_freq += dyn_ltree[n * 2]; n++;
            }
            while (n < 128)
            {
                ascii_freq += dyn_ltree[n * 2]; n++;
            }
            while (n < LITERALS)
            {
                bin_freq += dyn_ltree[n * 2]; n++;
            }
            data_type = (Byte)(bin_freq > (SupportClass.URShift(ascii_freq, 2)) ? Z_BINARY : Z_ASCII);
        }

        // Flush the bit buffer, keeping at most 7 bits in it.
        internal void bi_flush()
        {
            if (bi_valid == 16)
            {
                put_Int16(bi_buf);
                bi_buf = 0;
                bi_valid = 0;
            }
            else if (bi_valid >= 8)
            {
                put_Byte((Byte)bi_buf);
                bi_buf = (Int16)(SupportClass.URShift(bi_buf, 8));
                bi_valid -= 8;
            }
        }

        // Flush the bit buffer and align the output on a Byte boundary
        internal void bi_windup()
        {
            if (bi_valid > 8)
            {
                put_Int16(bi_buf);
            }
            else if (bi_valid > 0)
            {
                put_Byte((Byte)bi_buf);
            }
            bi_buf = 0;
            bi_valid = 0;
        }

        // Copy a stored block, storing first the length and its
        // one's complement if requested.
        internal void copy_block(Int32 buf, Int32 len, Boolean header)
        {

            bi_windup(); // align on Byte boundary
            last_eob_len = 8; // enough lookahead for inflate

            if (header)
            {
                put_Int16((Int16)len);
                put_Int16((Int16)~len);
            }

            //  while(len--!=0) {
            //    put_Byte(window[buf+index]);
            //    index++;
            //  }
            put_Byte(window, buf, len);
        }

        internal void flush_block_only(Boolean eof)
        {
            _tr_flush_block(block_start >= 0 ? block_start : -1, strstart - block_start, eof);
            block_start = strstart;
            strm.flush_pending();
        }

        // Copy without compression as much as possible from the input stream, return
        // the current block state.
        // This function does not insert new strings in the dictionary since
        // uncompressible data is probably not useful. This function is used
        // only for the level=0 compression option.
        // NOTE: this function should be optimized to avoid extra copying from
        // window to pending_buf.
        internal Int32 deflate_stored(Int32 flush)
        {
            // Stored blocks are limited to 0xffff Bytes, pending_buf is limited
            // to pending_buf_size, and each stored block has a 5 Byte header:

            Int32 max_block_size = 0xffff;
            Int32 max_start;

            if (max_block_size > pending_buf_size - 5)
            {
                max_block_size = pending_buf_size - 5;
            }

            // Copy as much as possible from input to output:
            while (true)
            {
                // Fill the window as much as possible:
                if (lookahead <= 1)
                {
                    fill_window();
                    if (lookahead == 0 && flush == Z_NO_FLUSH)
                        return NeedMore;
                    if (lookahead == 0)
                        break; // flush the current block
                }

                strstart += lookahead;
                lookahead = 0;

                // Emit a stored block if pending_buf will be full:
                max_start = block_start + max_block_size;
                if (strstart == 0 || strstart >= max_start)
                {
                    // strstart == 0 is possible when wraparound on 16-bit machine
                    lookahead = (Int32)(strstart - max_start);
                    strstart = (Int32)max_start;

                    flush_block_only(false);
                    if (strm.avail_out == 0)
                        return NeedMore;
                }

                // Flush if we may have to slide, otherwise block_start may become
                // negative and the data will be gone:
                if (strstart - block_start >= w_size - MIN_LOOKAHEAD)
                {
                    flush_block_only(false);
                    if (strm.avail_out == 0)
                        return NeedMore;
                }
            }

            flush_block_only(flush == Z_FINISH);
            if (strm.avail_out == 0)
                return (flush == Z_FINISH) ? FinishStarted : NeedMore;

            return flush == Z_FINISH ? FinishDone : BlockDone;
        }

        // Send a stored block
        internal void _tr_stored_block(Int32 buf, Int32 stored_len, Boolean eof)
        {
            send_bits((STORED_BLOCK << 1) + (eof ? 1 : 0), 3); // send block type
            copy_block(buf, stored_len, true); // with header
        }

        // Determine the best encoding for the current block: dynamic trees, static
        // trees or store, and output the encoded block to the zip file.
        internal void _tr_flush_block(Int32 buf, Int32 stored_len, Boolean eof)
        {
            Int32 opt_lenb, static_lenb; // opt_len and static_len in Bytes
            Int32 max_blindex = 0; // index of last bit length code of non zero freq

            // Build the Huffman trees unless a stored block is forced
            if (level > 0)
            {
                // Check if the file is ascii or binary
                if (data_type == Z_UNKNOWN)
                    set_data_type();

                // Construct the literal and distance trees
                l_desc.build_tree(this);

                d_desc.build_tree(this);

                // At this point, opt_len and static_len are the total bit lengths of
                // the compressed block data, excluding the tree representations.

                // Build the bit length tree for the above two trees, and get the index
                // in bl_order of the last bit length code to send.
                max_blindex = build_bl_tree();

                // Determine the best encoding. Compute first the block length in Bytes
                opt_lenb = SupportClass.URShift((opt_len + 3 + 7), 3);
                static_lenb = SupportClass.URShift((static_len + 3 + 7), 3);

                if (static_lenb <= opt_lenb)
                    opt_lenb = static_lenb;
            }
            else
            {
                opt_lenb = static_lenb = stored_len + 5; // force a stored block
            }

            if (stored_len + 4 <= opt_lenb && buf != -1)
            {
                // 4: two words for the lengths
                // The test buf != NULL is only necessary if LIT_BUFSIZE > WSIZE.
                // Otherwise we can't have processed more than WSIZE input Bytes since
                // the last block flush, because compression would have been
                // successful. If LIT_BUFSIZE <= WSIZE, it is never too late to
                // transform a block into a stored block.
                _tr_stored_block(buf, stored_len, eof);
            }
            else if (static_lenb == opt_lenb)
            {
                send_bits((STATIC_TREES << 1) + (eof ? 1 : 0), 3);
                compress_block(StaticTree.static_ltree, StaticTree.static_dtree);
            }
            else
            {
                send_bits((DYN_TREES << 1) + (eof ? 1 : 0), 3);
                send_all_trees(l_desc.max_code + 1, d_desc.max_code + 1, max_blindex + 1);
                compress_block(dyn_ltree, dyn_dtree);
            }

            // The above check is made mod 2^32, for files larger than 512 MB
            // and uLong implemented on 32 bits.

            init_block();

            if (eof)
            {
                bi_windup();
            }
        }

        // Fill the window when the lookahead becomes insufficient.
        // Updates strstart and lookahead.
        //
        // IN assertion: lookahead < MIN_LOOKAHEAD
        // OUT assertions: strstart <= window_size-MIN_LOOKAHEAD
        //    At least one Byte has been read, or avail_in == 0; reads are
        //    performed for at least two Bytes (required for the zip translate_eol
        //    option -- not supported here).
        internal void fill_window()
        {
            Int32 n, m;
            Int32 p;
            Int32 more; // Amount of free space at the end of the window.

            do
            {
                more = (window_size - lookahead - strstart);

                // Deal with !@#$% 64K limit:
                if (more == 0 && strstart == 0 && lookahead == 0)
                {
                    more = w_size;
                }
                else if (more == -1)
                {
                    // Very unlikely, but possible on 16 bit machine if strstart == 0
                    // and lookahead == 1 (input done one Byte at time)
                    more--;

                    // If the window is almost full and there is insufficient lookahead,
                    // move the upper half to the lower one to make room in the upper half.
                }
                else if (strstart >= w_size + w_size - MIN_LOOKAHEAD)
                {
                    Array.Copy(window, w_size, window, 0, w_size);
                    match_start -= w_size;
                    strstart -= w_size; // we now have strstart >= MAX_DIST
                    block_start -= w_size;

                    // Slide the hash table (could be avoided with 32 bit values
                    // at the expense of memory usage). We slide even when level == 0
                    // to keep the hash table consistent if we switch back to level > 0
                    // later. (Using level 0 permanently is not an optimal usage of
                    // zlib, so we don't care about this pathological case.)

                    n = hash_size;
                    p = n;
                    do
                    {
                        m = (head[--p] & 0xffff);
                        head[p] = (Int16)(m >= w_size ? (m - w_size) : 0);
                        //head[p] = (m >= w_size?(Int16) (m - w_size):0);
                    }
                    while (--n != 0);

                    n = w_size;
                    p = n;
                    do
                    {
                        m = (prev[--p] & 0xffff);
                        prev[p] = (Int16)(m >= w_size ? (m - w_size) : 0);
                        //prev[p] = (m >= w_size?(Int16) (m - w_size):0);
                        // If n is not on any hash chain, prev[n] is garbage but
                        // its value will never be used.
                    }
                    while (--n != 0);
                    more += w_size;
                }

                if (strm.avail_in == 0)
                    return;

                // If there was no sliding:
                //    strstart <= WSIZE+MAX_DIST-1 && lookahead <= MIN_LOOKAHEAD - 1 &&
                //    more == window_size - lookahead - strstart
                // => more >= window_size - (MIN_LOOKAHEAD-1 + WSIZE + MAX_DIST-1)
                // => more >= window_size - 2*WSIZE + 2
                // In the BIG_MEM or MMAP case (not yet supported),
                //   window_size == input_size + MIN_LOOKAHEAD  &&
                //   strstart + s->lookahead <= input_size => more >= MIN_LOOKAHEAD.
                // Otherwise, window_size == 2*WSIZE so more >= 2.
                // If there was sliding, more >= WSIZE. So in all cases, more >= 2.

                n = strm.read_buf(window, strstart + lookahead, more);
                lookahead += n;

                // Initialize the hash value now that we have some input:
                if (lookahead >= MIN_MATCH)
                {
                    ins_h = window[strstart] & 0xff;
                    ins_h = (((ins_h) << hash_shift) ^ (window[strstart + 1] & 0xff)) & hash_mask;
                }
                // If the whole input has less than MIN_MATCH Bytes, ins_h is garbage,
                // but this is not important since only literal Bytes will be emitted.
            }
            while (lookahead < MIN_LOOKAHEAD && strm.avail_in != 0);
        }

        // Compress as much as possible from the input stream, return the current
        // block state.
        // This function does not perform lazy evaluation of matches and inserts
        // new strings in the dictionary only for unmatched strings or for Int16
        // matches. It is used only for the fast compression options.
        internal Int32 deflate_fast(Int32 flush)
        {
            //    Int16 hash_head = 0; // head of the hash chain
            Int32 hash_head = 0; // head of the hash chain
            Boolean bflush; // set if current block must be flushed

            while (true)
            {
                // Make sure that we always have enough lookahead, except
                // at the end of the input file. We need MAX_MATCH Bytes
                // for the next match, plus MIN_MATCH Bytes to insert the
                // string following the next match.
                if (lookahead < MIN_LOOKAHEAD)
                {
                    fill_window();
                    if (lookahead < MIN_LOOKAHEAD && flush == Z_NO_FLUSH)
                    {
                        return NeedMore;
                    }
                    if (lookahead == 0)
                        break; // flush the current block
                }

                // Insert the string window[strstart .. strstart+2] in the
                // dictionary, and set hash_head to the head of the hash chain:
                if (lookahead >= MIN_MATCH)
                {
                    ins_h = (((ins_h) << hash_shift) ^ (window[(strstart) + (MIN_MATCH - 1)] & 0xff)) & hash_mask;

                    //	prev[strstart&w_mask]=hash_head=head[ins_h];
                    hash_head = (head[ins_h] & 0xffff);
                    prev[strstart & w_mask] = head[ins_h];
                    head[ins_h] = (Int16)strstart;
                }

                // Find the longest match, discarding those <= prev_length.
                // At this point we have always match_length < MIN_MATCH

                if (hash_head != 0L && ((strstart - hash_head) & 0xffff) <= w_size - MIN_LOOKAHEAD)
                {
                    // To simplify the code, we prevent matches with the string
                    // of window index 0 (in particular we have to avoid a match
                    // of the string with itself at the start of the input file).
                    if (strategy != Z_HUFFMAN_ONLY)
                    {
                        match_length = longest_match(hash_head);
                    }
                    // longest_match() sets match_start
                }
                if (match_length >= MIN_MATCH)
                {
                    //        check_match(strstart, match_start, match_length);

                    bflush = _tr_tally(strstart - match_start, match_length - MIN_MATCH);

                    lookahead -= match_length;

                    // Insert new strings in the hash table only if the match length
                    // is not too large. This saves time but degrades compression.
                    if (match_length <= max_lazy_match && lookahead >= MIN_MATCH)
                    {
                        match_length--; // string at strstart already in hash table
                        do
                        {
                            strstart++;

                            ins_h = ((ins_h << hash_shift) ^ (window[(strstart) + (MIN_MATCH - 1)] & 0xff)) & hash_mask;
                            //	    prev[strstart&w_mask]=hash_head=head[ins_h];
                            hash_head = (head[ins_h] & 0xffff);
                            prev[strstart & w_mask] = head[ins_h];
                            head[ins_h] = (Int16)strstart;

                            // strstart never exceeds WSIZE-MAX_MATCH, so there are
                            // always MIN_MATCH Bytes ahead.
                        }
                        while (--match_length != 0);
                        strstart++;
                    }
                    else
                    {
                        strstart += match_length;
                        match_length = 0;
                        ins_h = window[strstart] & 0xff;

                        ins_h = (((ins_h) << hash_shift) ^ (window[strstart + 1] & 0xff)) & hash_mask;
                        // If lookahead < MIN_MATCH, ins_h is garbage, but it does not
                        // matter since it will be recomputed at next deflate call.
                    }
                }
                else
                {
                    // No match, output a literal Byte

                    bflush = _tr_tally(0, window[strstart] & 0xff);
                    lookahead--;
                    strstart++;
                }
                if (bflush)
                {

                    flush_block_only(false);
                    if (strm.avail_out == 0)
                        return NeedMore;
                }
            }

            flush_block_only(flush == Z_FINISH);
            if (strm.avail_out == 0)
            {
                if (flush == Z_FINISH)
                    return FinishStarted;
                else
                    return NeedMore;
            }
            return flush == Z_FINISH ? FinishDone : BlockDone;
        }

        // Same as above, but achieves better compression. We use a lazy
        // evaluation for matches: a match is finally adopted only if there is
        // no better match at the next window position.
        internal Int32 deflate_slow(Int32 flush)
        {
            //    Int16 hash_head = 0;    // head of hash chain
            Int32 hash_head = 0; // head of hash chain
            Boolean bflush; // set if current block must be flushed

            // Process the input block.
            while (true)
            {
                // Make sure that we always have enough lookahead, except
                // at the end of the input file. We need MAX_MATCH Bytes
                // for the next match, plus MIN_MATCH Bytes to insert the
                // string following the next match.

                if (lookahead < MIN_LOOKAHEAD)
                {
                    fill_window();
                    if (lookahead < MIN_LOOKAHEAD && flush == Z_NO_FLUSH)
                    {
                        return NeedMore;
                    }
                    if (lookahead == 0)
                        break; // flush the current block
                }

                // Insert the string window[strstart .. strstart+2] in the
                // dictionary, and set hash_head to the head of the hash chain:

                if (lookahead >= MIN_MATCH)
                {
                    ins_h = (((ins_h) << hash_shift) ^ (window[(strstart) + (MIN_MATCH - 1)] & 0xff)) & hash_mask;
                    //	prev[strstart&w_mask]=hash_head=head[ins_h];
                    hash_head = (head[ins_h] & 0xffff);
                    prev[strstart & w_mask] = head[ins_h];
                    head[ins_h] = (Int16)strstart;
                }

                // Find the longest match, discarding those <= prev_length.
                prev_length = match_length; prev_match = match_start;
                match_length = MIN_MATCH - 1;

                if (hash_head != 0 && prev_length < max_lazy_match && ((strstart - hash_head) & 0xffff) <= w_size - MIN_LOOKAHEAD)
                {
                    // To simplify the code, we prevent matches with the string
                    // of window index 0 (in particular we have to avoid a match
                    // of the string with itself at the start of the input file).

                    if (strategy != Z_HUFFMAN_ONLY)
                    {
                        match_length = longest_match(hash_head);
                    }
                    // longest_match() sets match_start

                    if (match_length <= 5 && (strategy == Z_FILTERED || (match_length == MIN_MATCH && strstart - match_start > 4096)))
                    {

                        // If prev_match is also MIN_MATCH, match_start is garbage
                        // but we will ignore the current match anyway.
                        match_length = MIN_MATCH - 1;
                    }
                }

                // If there was a match at the previous step and the current
                // match is not better, output the previous match:
                if (prev_length >= MIN_MATCH && match_length <= prev_length)
                {
                    Int32 max_insert = strstart + lookahead - MIN_MATCH;
                    // Do not insert strings in hash table beyond this.

                    //          check_match(strstart-1, prev_match, prev_length);

                    bflush = _tr_tally(strstart - 1 - prev_match, prev_length - MIN_MATCH);

                    // Insert in hash table all strings up to the end of the match.
                    // strstart-1 and strstart are already inserted. If there is not
                    // enough lookahead, the last two strings are not inserted in
                    // the hash table.
                    lookahead -= (prev_length - 1);
                    prev_length -= 2;
                    do
                    {
                        if (++strstart <= max_insert)
                        {
                            ins_h = (((ins_h) << hash_shift) ^ (window[(strstart) + (MIN_MATCH - 1)] & 0xff)) & hash_mask;
                            //prev[strstart&w_mask]=hash_head=head[ins_h];
                            hash_head = (head[ins_h] & 0xffff);
                            prev[strstart & w_mask] = head[ins_h];
                            head[ins_h] = (Int16)strstart;
                        }
                    }
                    while (--prev_length != 0);
                    match_available = 0;
                    match_length = MIN_MATCH - 1;
                    strstart++;

                    if (bflush)
                    {
                        flush_block_only(false);
                        if (strm.avail_out == 0)
                            return NeedMore;
                    }
                }
                else if (match_available != 0)
                {

                    // If there was no match at the previous position, output a
                    // single literal. If there was a match but the current match
                    // is longer, truncate the previous match to a single literal.

                    bflush = _tr_tally(0, window[strstart - 1] & 0xff);

                    if (bflush)
                    {
                        flush_block_only(false);
                    }
                    strstart++;
                    lookahead--;
                    if (strm.avail_out == 0)
                        return NeedMore;
                }
                else
                {
                    // There is no previous match to compare with, wait for
                    // the next step to decide.

                    match_available = 1;
                    strstart++;
                    lookahead--;
                }
            }

            if (match_available != 0)
            {
                bflush = _tr_tally(0, window[strstart - 1] & 0xff);
                match_available = 0;
            }
            flush_block_only(flush == Z_FINISH);

            if (strm.avail_out == 0)
            {
                if (flush == Z_FINISH)
                    return FinishStarted;
                else
                    return NeedMore;
            }

            return flush == Z_FINISH ? FinishDone : BlockDone;
        }

        internal Int32 longest_match(Int32 cur_match)
        {
            Int32 chain_length = max_chain_length; // max hash chain length
            Int32 scan = strstart; // current string
            Int32 match; // matched string
            Int32 len; // length of current match
            Int32 best_len = prev_length; // best match length so far
            Int32 limit = strstart > (w_size - MIN_LOOKAHEAD) ? strstart - (w_size - MIN_LOOKAHEAD) : 0;
            Int32 nice_match = this.nice_match;

            // Stop when cur_match becomes <= limit. To simplify the code,
            // we prevent matches with the string of window index 0.

            Int32 wmask = w_mask;

            Int32 strend = strstart + MAX_MATCH;
            Byte scan_end1 = window[scan + best_len - 1];
            Byte scan_end = window[scan + best_len];

            // The code is optimized for HASH_BITS >= 8 and MAX_MATCH-2 multiple of 16.
            // It is easy to get rid of this optimization if necessary.

            // Do not waste too much time if we already have a good match:
            if (prev_length >= good_match)
            {
                chain_length >>= 2;
            }

            // Do not look for matches beyond the end of the input. This is necessary
            // to make deflate deterministic.
            if (nice_match > lookahead)
                nice_match = lookahead;

            do
            {
                match = cur_match;

                // Skip to next match if the match length cannot increase
                // or if the match length is less than 2:
                if (window[match + best_len] != scan_end || window[match + best_len - 1] != scan_end1 || window[match] != window[scan] || window[++match] != window[scan + 1])
                    continue;

                // The check at best_len-1 can be removed because it will be made
                // again later. (This heuristic is not always a win.)
                // It is not necessary to compare scan[2] and match[2] since they
                // are always equal when the other Bytes match, given that
                // the hash keys are equal and that HASH_BITS >= 8.
                scan += 2; match++;

                // We check for insufficient lookahead only every 8th comparison;
                // the 256th check will be made at strstart+258.
                do
                {
                }
                while (window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && scan < strend);

                len = MAX_MATCH - (Int32)(strend - scan);
                scan = strend - MAX_MATCH;

                if (len > best_len)
                {
                    match_start = cur_match;
                    best_len = len;
                    if (len >= nice_match)
                        break;
                    scan_end1 = window[scan + best_len - 1];
                    scan_end = window[scan + best_len];
                }
            }
            while ((cur_match = (prev[cur_match & wmask] & 0xffff)) > limit && --chain_length != 0);

            if (best_len <= lookahead)
                return best_len;
            return lookahead;
        }

        internal Int32 deflateInit(ZStream strm, Int32 level, Int32 bits)
        {
            return deflateInit2(strm, level, Z_DEFLATED, bits, DEF_MEM_LEVEL, Z_DEFAULT_STRATEGY);
        }
        internal Int32 deflateInit(ZStream strm, Int32 level)
        {
            return deflateInit(strm, level, MAX_WBITS);
        }
        internal Int32 deflateInit2(ZStream strm, Int32 level, Int32 method, Int32 windowBits, Int32 memLevel, Int32 strategy)
        {
            Int32 noheader = 0;
            //    Byte[] my_version=ZLIB_VERSION;

            //
            //  if (version == null || version[0] != my_version[0]
            //  || stream_size != sizeof(z_stream)) {
            //  return Z_VERSION_ERROR;
            //  }

            strm.msg = null;

            if (level == Z_DEFAULT_COMPRESSION)
                level = 6;

            if (windowBits < 0)
            {
                // undocumented feature: suppress zlib header
                noheader = 1;
                windowBits = -windowBits;
            }

            if (memLevel < 1 || memLevel > MAX_MEM_LEVEL || method != Z_DEFLATED || windowBits < 9 || windowBits > 15 || level < 0 || level > 9 || strategy < 0 || strategy > Z_HUFFMAN_ONLY)
            {
                return Z_STREAM_ERROR;
            }

            strm.dstate = (Deflate)this;

            this.noheader = noheader;
            w_bits = windowBits;
            w_size = 1 << w_bits;
            w_mask = w_size - 1;

            hash_bits = memLevel + 7;
            hash_size = 1 << hash_bits;
            hash_mask = hash_size - 1;
            hash_shift = ((hash_bits + MIN_MATCH - 1) / MIN_MATCH);

            window = new Byte[w_size * 2];
            prev = new Int16[w_size];
            head = new Int16[hash_size];

            lit_bufsize = 1 << (memLevel + 6); // 16K elements by default

            // We overlay pending_buf and d_buf+l_buf. This works since the average
            // output size for (length,distance) codes is <= 24 bits.
            pending_buf = new Byte[lit_bufsize * 4];
            pending_buf_size = lit_bufsize * 4;

            d_buf = lit_bufsize;
            l_buf = (1 + 2) * lit_bufsize;

            this.level = level;

            //System.out.println("level="+level);

            this.strategy = strategy;
            this.method = (Byte)method;

            return deflateReset(strm);
        }

        internal Int32 deflateReset(ZStream strm)
        {
            strm.total_in = strm.total_out = 0;
            strm.msg = null; //
            strm.data_type = Z_UNKNOWN;

            pending = 0;
            pending_out = 0;

            if (noheader < 0)
            {
                noheader = 0; // was set to -1 by deflate(..., Z_FINISH);
            }
            status = (noheader != 0) ? BUSY_STATE : INIT_STATE;
            strm.adler = strm._adler.adler32(0, null, 0, 0);

            last_flush = Z_NO_FLUSH;

            tr_init();
            lm_init();
            return Z_OK;
        }

        internal Int32 deflateEnd()
        {
            if (status != INIT_STATE && status != BUSY_STATE && status != FINISH_STATE)
            {
                return Z_STREAM_ERROR;
            }
            // Deallocate in reverse order of allocations:
            pending_buf = null;
            head = null;
            prev = null;
            window = null;
            // free
            // dstate=null;
            return status == BUSY_STATE ? Z_DATA_ERROR : Z_OK;
        }

        internal Int32 deflateParams(ZStream strm, Int32 _level, Int32 _strategy)
        {
            Int32 err = Z_OK;

            if (_level == Z_DEFAULT_COMPRESSION)
            {
                _level = 6;
            }
            if (_level < 0 || _level > 9 || _strategy < 0 || _strategy > Z_HUFFMAN_ONLY)
            {
                return Z_STREAM_ERROR;
            }

            if (config_table[level].func != config_table[_level].func && strm.total_in != 0)
            {
                // Flush the last buffer:
                err = strm.deflate(Z_PARTIAL_FLUSH);
            }

            if (level != _level)
            {
                level = _level;
                max_lazy_match = config_table[level].max_lazy;
                good_match = config_table[level].good_length;
                nice_match = config_table[level].nice_length;
                max_chain_length = config_table[level].max_chain;
            }
            strategy = _strategy;
            return err;
        }

        internal Int32 deflateSetMyDictionary(ZStream strm, Byte[] dictionary, Int32 dictLength)
        {
            Int32 length = dictLength;
            Int32 index = 0;

            if (dictionary == null || status != INIT_STATE)
                return Z_STREAM_ERROR;

            strm.adler = strm._adler.adler32(strm.adler, dictionary, 0, dictLength);

            if (length < MIN_MATCH)
                return Z_OK;
            if (length > w_size - MIN_LOOKAHEAD)
            {
                length = w_size - MIN_LOOKAHEAD;
                index = dictLength - length; // use the tail of the dictionary
            }
            Array.Copy(dictionary, index, window, 0, length);
            strstart = length;
            block_start = length;

            // Insert all strings in the hash table (except for the last two Bytes).
            // s->lookahead stays null, so s->ins_h will be recomputed at the next
            // call of fill_window.

            ins_h = window[0] & 0xff;
            ins_h = (((ins_h) << hash_shift) ^ (window[1] & 0xff)) & hash_mask;

            for (Int32 n = 0; n <= length - MIN_MATCH; n++)
            {
                ins_h = (((ins_h) << hash_shift) ^ (window[(n) + (MIN_MATCH - 1)] & 0xff)) & hash_mask;
                prev[n & w_mask] = head[ins_h];
                head[ins_h] = (Int16)n;
            }
            return Z_OK;
        }

        internal Int32 deflate(ZStream strm, Int32 flush)
        {
            Int32 old_flush;

            if (flush > Z_FINISH || flush < 0)
            {
                return Z_STREAM_ERROR;
            }

            if (strm.next_out == null || (strm.next_in == null && strm.avail_in != 0) || (status == FINISH_STATE && flush != Z_FINISH))
            {
                strm.msg = z_errmsg[Z_NEED_DICT - (Z_STREAM_ERROR)];
                return Z_STREAM_ERROR;
            }
            if (strm.avail_out == 0)
            {
                strm.msg = z_errmsg[Z_NEED_DICT - (Z_BUF_ERROR)];
                return Z_BUF_ERROR;
            }

            this.strm = strm; // just in case
            old_flush = last_flush;
            last_flush = flush;

            // Write the zlib header
            if (status == INIT_STATE)
            {
                Int32 header = (Z_DEFLATED + ((w_bits - 8) << 4)) << 8;
                Int32 level_flags = ((level - 1) & 0xff) >> 1;

                if (level_flags > 3)
                    level_flags = 3;
                header |= (level_flags << 6);
                if (strstart != 0)
                    header |= PRESET_DICT;
                header += 31 - (header % 31);

                status = BUSY_STATE;
                putInt16MSB(header);


                // Save the adler32 of the preset dictionary:
                if (strstart != 0)
                {
                    putInt16MSB((Int32)(SupportClass.URShift(strm.adler, 16)));
                    putInt16MSB((Int32)(strm.adler & 0xffff));
                }
                strm.adler = strm._adler.adler32(0, null, 0, 0);
            }

            // Flush as much pending output as possible
            if (pending != 0)
            {
                strm.flush_pending();
                if (strm.avail_out == 0)
                {
                    //System.out.println("  avail_out==0");
                    // Since avail_out is 0, deflate will be called again with
                    // more output space, but possibly with both pending and
                    // avail_in equal to zero. There won't be anything to do,
                    // but this is not an error situation so make sure we
                    // return OK instead of BUF_ERROR at next call of deflate:
                    last_flush = -1;
                    return Z_OK;
                }

                // Make sure there is something to do and avoid duplicate consecutive
                // flushes. For repeated and useless calls with Z_FINISH, we keep
                // returning Z_STREAM_END instead of Z_BUFF_ERROR.
            }
            else if (strm.avail_in == 0 && flush <= old_flush && flush != Z_FINISH)
            {
                strm.msg = z_errmsg[Z_NEED_DICT - (Z_BUF_ERROR)];
                return Z_BUF_ERROR;
            }

            // User must not provide more input after the first FINISH:
            if (status == FINISH_STATE && strm.avail_in != 0)
            {
                strm.msg = z_errmsg[Z_NEED_DICT - (Z_BUF_ERROR)];
                return Z_BUF_ERROR;
            }

            // Start a new block or continue the current one.
            if (strm.avail_in != 0 || lookahead != 0 || (flush != Z_NO_FLUSH && status != FINISH_STATE))
            {
                Int32 bstate = -1;
                switch (config_table[level].func)
                {

                    case STORED:
                        bstate = deflate_stored(flush);
                        break;

                    case FAST:
                        bstate = deflate_fast(flush);
                        break;

                    case SLOW:
                        bstate = deflate_slow(flush);
                        break;

                    default:
                        break;

                }

                if (bstate == FinishStarted || bstate == FinishDone)
                {
                    status = FINISH_STATE;
                }
                if (bstate == NeedMore || bstate == FinishStarted)
                {
                    if (strm.avail_out == 0)
                    {
                        last_flush = -1; // avoid BUF_ERROR next call, see above
                    }
                    return Z_OK;
                    // If flush != Z_NO_FLUSH && avail_out == 0, the next call
                    // of deflate should use the same flush parameter to make sure
                    // that the flush is complete. So we don't have to output an
                    // empty block here, this will be done at next call. This also
                    // ensures that for a very small output buffer, we emit at most
                    // one empty block.
                }

                if (bstate == BlockDone)
                {
                    if (flush == Z_PARTIAL_FLUSH)
                    {
                        _tr_align();
                    }
                    else
                    {
                        // FULL_FLUSH or SYNC_FLUSH
                        _tr_stored_block(0, 0, false);
                        // For a full flush, this empty block will be recognized
                        // as a special marker by inflate_sync().
                        if (flush == Z_FULL_FLUSH)
                        {
                            //state.head[s.hash_size-1]=0;
                            for (Int32 i = 0; i < hash_size; i++)
                                // forget history
                                head[i] = 0;
                        }
                    }
                    strm.flush_pending();
                    if (strm.avail_out == 0)
                    {
                        last_flush = -1; // avoid BUF_ERROR at next call, see above
                        return Z_OK;
                    }
                }
            }

            if (flush != Z_FINISH)
                return Z_OK;
            if (noheader != 0)
                return Z_STREAM_END;

            // Write the zlib trailer (adler32)
            putInt16MSB((Int32)(SupportClass.URShift(strm.adler, 16)));
            putInt16MSB((Int32)(strm.adler & 0xffff));
            strm.flush_pending();

            // If avail_out is zero, the application will call deflate again
            // to flush the rest.
            noheader = -1; // write the trailer only once!
            return pending != 0 ? Z_OK : Z_STREAM_END;
        }
        static Deflate()
        {
            {
                config_table = new Config[10];
                //                         good  lazy  nice  chain
                config_table[0] = new Config(0, 0, 0, 0, STORED);
                config_table[1] = new Config(4, 4, 8, 4, FAST);
                config_table[2] = new Config(4, 5, 16, 8, FAST);
                config_table[3] = new Config(4, 6, 32, 32, FAST);

                config_table[4] = new Config(4, 4, 16, 16, SLOW);
                config_table[5] = new Config(8, 16, 32, 32, SLOW);
                config_table[6] = new Config(8, 16, 128, 128, SLOW);
                config_table[7] = new Config(8, 32, 128, 256, SLOW);
                config_table[8] = new Config(32, 128, 258, 1024, SLOW);
                config_table[9] = new Config(32, 258, 258, 4096, SLOW);
            }
        }
    }
}