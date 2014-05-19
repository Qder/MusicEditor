using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using NAudio.Dsp;
using NAudio.Lame;
using System.IO;

namespace SoundTrackChanges
{
    public static class WavFileUtils
    {
        public static void TrimWavFile(string innPath, string outtPath, TimeSpan cutFromStart, TimeSpan cutFromEnd)
        {
            string inPath = "C:\\Users\\Administratorius\\Documents\\GitHub\\MusicEditor\\PawellsMusicEditor\\PawellsMusicEditor\\Content\\Songs\\NowEdited.WAV";
            string outPath = "C:\\Users\\Administratorius\\Documents\\GitHub\\MusicEditor\\PawellsMusicEditor\\PawellsMusicEditor\\Content\\Songs\\NowEdited2.WAV";
            Mp3ToWav(innPath, inPath);
            using (WaveFileReader reader = new WaveFileReader(inPath))
            {
                using (WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat))
                {
                    int bytesPerMillisecond = reader.WaveFormat.AverageBytesPerSecond / 1000;

                    int startPos = (int)cutFromStart.TotalMilliseconds * bytesPerMillisecond;
                    startPos = startPos - startPos % reader.WaveFormat.BlockAlign;

                    int endBytes = (int)cutFromEnd.TotalMilliseconds * bytesPerMillisecond;
                    endBytes = endBytes - endBytes % reader.WaveFormat.BlockAlign;
                    int endPos = (int)reader.Length - endBytes;

                    TrimWavFile(reader, writer, startPos, endPos);
                    reader.Dispose();
                    writer.Dispose();
                }
                ConvertWavToMp3(outPath, outtPath);
            }
            File.Delete(inPath);
            File.Delete(outPath);
        }

        private static void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos)
        {
            reader.Position = startPos;
            byte[] buffer = new byte[1024];
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.WriteData(buffer, 0, bytesRead);
                    }
                }
            }
            reader.Dispose();
            writer.Dispose();
        }

        public static void CreateWaveFile2(string filename, IWaveProvider sourceProvider)
        {
            using (var writer = new WaveFileWriter(filename, sourceProvider.WaveFormat))
            {
                long outputLength = 0;
                var buffer = new byte[sourceProvider.WaveFormat.AverageBytesPerSecond * 4];
                while (true)
                {
                    int bytesRead = sourceProvider.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        // end of source provider
                        break;
                    }
                    outputLength += bytesRead;
                    // Write will throw exception if WAV file becomes too large
                    if (bytesRead > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
                writer.Dispose();
            }
        }

        public static void Mp3ToWav(string mp3File, string outputFile)
        {
            using (Mp3FileReader reader = new Mp3FileReader(mp3File))
            {
                CreateWaveFile2(outputFile, reader);
                reader.Dispose();
            }
        }

        public static byte[] ConvertWavToMp3(byte[] wavFile)
        {
            CheckAddBinPath();
            using (var retMs = new MemoryStream())
            using (var ms = new MemoryStream(wavFile))
            using (var rdr = new WaveFileReader(ms))
            using (var wtr = new LameMP3FileWriter(retMs, rdr.WaveFormat, 128))
            {
                rdr.CopyTo(wtr);
                return retMs.ToArray();
            }
        }

        public static void ConvertWavToMp3(string WavFile, string outPutFile)
        {
            CheckAddBinPath();
            WaveFileReader rdr = new WaveFileReader(WavFile);
            using (var wtr = new LameMP3FileWriter(outPutFile, rdr.WaveFormat, 128))
            {
                rdr.CopyTo(wtr);
                rdr.Dispose();
                wtr.Dispose();
                return;
            }
        }

        public static void CheckAddBinPath()
        {
            // find path to 'bin' folder
            var binPath = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "bin" });
            // get current search path from environment
            var path = Environment.GetEnvironmentVariable("PATH") ?? "";

            // add 'bin' folder to search path if not already present
            if (!path.Split(Path.PathSeparator).Contains(binPath, StringComparer.CurrentCultureIgnoreCase))
            {
                path = string.Join(Path.PathSeparator.ToString(), new string[] { path, binPath });
                Environment.SetEnvironmentVariable("PATH", path);
            }
        }

        public static void LowPassFilter(string innPath, string outtPath)
        {
            string inPath = "C:\\Users\\Administratorius\\Documents\\GitHub\\MusicEditor\\PawellsMusicEditor\\PawellsMusicEditor\\Content\\Songs\\NowEdited.WAV";
            string outPath = "C:\\Users\\Administratorius\\Documents\\GitHub\\MusicEditor\\PawellsMusicEditor\\PawellsMusicEditor\\Content\\Songs\\NowEdited3.WAV";
            
            Mp3ToWav(innPath, inPath);
            WaveFileReader reader = new WaveFileReader(inPath);
            var myFilter = BiQuadFilter.LowPassFilter(44F, 0.4F, 0.5F);
            WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat);
            

            reader.Position = 0;
            var endPos = reader.Length;
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    float[] sample = new float[4];
                    for (int i=0; i < 2; i++)
                    {
                        sample[i] = myFilter.Transform(reader.ReadNextSampleFrame()[i]);
                    }
                        writer.WriteSamples(sample, 0, 4);
                }
            }
            reader.Dispose();
            writer.Dispose();
            ConvertWavToMp3(outPath, outtPath);
            File.Delete(inPath);
            File.Delete(outPath);
        }

        public static void HighPassFilter(string innPath, string outtPath)
        {
            string inPath = "C:\\Users\\Administratorius\\Documents\\GitHub\\MusicEditor\\PawellsMusicEditor\\PawellsMusicEditor\\Content\\Songs\\NowEdited.WAV";
            string outPath = "C:\\Users\\Administratorius\\Documents\\GitHub\\MusicEditor\\PawellsMusicEditor\\PawellsMusicEditor\\Content\\Songs\\NowEdited3.WAV";

            Mp3ToWav(innPath, inPath);
            WaveFileReader reader = new WaveFileReader(inPath);
            var myFilter = BiQuadFilter.HighPassFilter(44F, 10F, 0.5F);
            WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat);


            reader.Position = 0;
            var endPos = reader.Length;
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    float[] sample = new float[4];
                    for (int i = 0; i < 2; i++)
                    {
                        sample[i] = myFilter.Transform(reader.ReadNextSampleFrame()[i]);
                    }
                    writer.WriteSamples(sample, 0, 4);
                }
            }
            reader.Dispose();
            writer.Dispose();
            ConvertWavToMp3(outPath, outtPath);
            File.Delete(inPath);
            File.Delete(outPath);
        }
    }
}