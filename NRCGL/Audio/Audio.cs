#region License
//
// The NRCGL License.
//
// The MIT License (MIT)
//
// Copyright (c) 2015 Nuno Ramalho da Costa
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
#endregion

using OpenTK;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_NRCGL.NRCGL.Audio
{   
    /// <summary>
    /// Audio Class.
    /// 
    /// Process:
    /// 1. Create Audio Context
    /// 2. Generate Buffers
    /// 3. Load a wav file into the Buffer
    /// 4. Generate a source and attach the buffer
    /// 5. Play
    /// </summary>
    class Audio
    {

        private int[] myBuffers;
        private int[] mySources;


        public Audio(string[] wavFilesNames)
        {
            var AC = new AudioContext();
            var XRam = new XRamExtension(); // must be instantiated per used Device if X-Ram is desired.

            // reserve n Handles
            myBuffers = AL.GenBuffers(wavFilesNames.Length);
            mySources = new int[wavFilesNames.Length];

            int count = 0;
            foreach (string fileName in wavFilesNames)
	        {
                if (XRam.IsInitialized)
                {
                    XRam.SetBufferMode(1, ref myBuffers[count], XRamExtension.XRamStorage.Hardware); // optional
                }

                // Load a .wav file from disk. See example code at:
                // https://github.com/opentk/opentk/blob/develop/Source/Examples/OpenAL/1.1/Playback.cs#L21
                int channels, bits_per_sample, sample_rate;
                var sound_data = LoadWave(
                    File.Open(fileName, FileMode.Open),
                    out channels,
                    out bits_per_sample,
                    out sample_rate);
                var sound_format =
                    channels == 1 && bits_per_sample == 8 ? ALFormat.Mono8 :
                    channels == 1 && bits_per_sample == 16 ? ALFormat.Mono16 :
                    channels == 2 && bits_per_sample == 8 ? ALFormat.Stereo8 :
                    channels == 2 && bits_per_sample == 16 ? ALFormat.Stereo16 :
                    (ALFormat)0; // unknown

                AL.BufferData(myBuffers[count], sound_format, sound_data, sound_data.Length, sample_rate);
                if (AL.GetError() != ALError.NoError)
                {
                    // respond to load error etc.
                }


                
                AL.GenSources(1, out mySources[count]); // gen 1 Source Handles

                AL.Source(mySources[count], ALSourcei.Buffer, (int)myBuffers[count]); // attach the buffer to a source

                count++;
	        }

            

            /*
            // Create a sinus waveform through parameters, this currently requires Alut.dll in the application directory
            if (XRam.IsInitialized)
            {
                XRam.SetBufferMode(ref MyBuffers[1], XRamStorage.Hardware); // optional
            }
            MyBuffers[1] = Alut.CreateBufferWaveform(AlutWaveform.Sine, 500f, 42f, 1.5f);
            */
            // See next book page how to connect the buffers to sources in order to play them.

            // Cleanup on application shutdown
            //AL.DeleteBuffers(MyBuffers.Length, MyBuffers); // free previously reserved Handles
            //AC.Dispose();
        }

        public void Play(int source, bool looping = false)
        {

            AL.SourcePlay(mySources[source]); // start playback
            if(looping)AL.Source(mySources[source], ALSourceb.Looping, true); // source loops infinitely
            /*
            AL.Source(MySources[1], ALSourcei.Buffer, (int)MyBuffers[1]);
            Vector3 Position = new Vector3(1f, 2f, 3f);
            AL.Source(MySources[1], ALSource3f.Position, ref Position);
            AL.Source(MySources[1], ALSourcef.Gain, 0.85f);
            AL.SourcePlay(MySources[1]);
            */
            //Console.ReadLine(); // wait for keystroke before exiting

            //AL.SourceStop(MySources[0]); // halt playback
            //AL.SourceStop(MySources[1]);

            //AL.DeleteSources(2, ref MySources); // free Handles
            // now delete Buffer Objects and dispose the AudioContext
        }

        // Loads a wave/riff audio file. 
        public static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate) 
         { 
             if (stream == null) 
                 throw new ArgumentNullException("stream"); 
 
 
             using (BinaryReader reader = new BinaryReader(stream)) 
             { 
                 // RIFF header 
                 string signature = new string(reader.ReadChars(4)); 
                 if (signature != "RIFF") 
                     throw new NotSupportedException("Specified stream is not a wave file."); 
 
 
                 int riff_chunck_size = reader.ReadInt32(); 
 
 
                 string format = new string(reader.ReadChars(4)); 
                 if (format != "WAVE") 
                     throw new NotSupportedException("Specified stream is not a wave file."); 
 
 
                 // WAVE header 
                 string format_signature = new string(reader.ReadChars(4)); 
                 if (format_signature != "fmt ") 
                     throw new NotSupportedException("Specified wave file is not supported."); 
 
 
                 int format_chunk_size = reader.ReadInt32(); 
                 int audio_format = reader.ReadInt16(); 
                 int num_channels = reader.ReadInt16(); 
                 int sample_rate = reader.ReadInt32(); 
                 int byte_rate = reader.ReadInt32(); 
                 int block_align = reader.ReadInt16(); 
                 int bits_per_sample = reader.ReadInt16(); 
 
 
                 string data_signature = new string(reader.ReadChars(4)); 
                 if (data_signature != "data") 
                     throw new NotSupportedException("Specified wave file is not supported."); 
 
 
                 int data_chunk_size = reader.ReadInt32(); 
 
 
                 channels = num_channels; 
                 bits = bits_per_sample; 
                 rate = sample_rate; 
 
 
                 return reader.ReadBytes((int)reader.BaseStream.Length); 
             } 
         }


    }
}
