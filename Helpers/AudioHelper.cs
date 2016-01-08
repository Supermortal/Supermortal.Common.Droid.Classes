using System;

using Android.Media;

using Supermortal.Common.PCL.Enums;

namespace Supermortal.Common.Droid.Helpers
{
    public class AudioHelper
    {
        public AudioHelper()
        {
        }

        private static int[] _sampleRates = new int[] { 44100, 22050, 11025, 8000 };

        public static AudioRecord FindAudioRecord(ref int sampleRate, ref Android.Media.Encoding audioFormat, ref ChannelIn channelConfig, ref int bufferSize)
        {
            foreach (int sr in _sampleRates)
            {
                foreach (var af in new Android.Media.Encoding[] { Android.Media.Encoding.Pcm16bit, Android.Media.Encoding.Pcm8bit })
                {
                    foreach (var cc in new ChannelIn[] { ChannelIn.Stereo, ChannelIn.Mono })
                    {
                        try
                        {
                            //                            Log.Debug(C.TAG, "Attempting rate " + rate + "Hz, bits: " + audioFormat + ", channel: "
                            //                                + channelConfig);
                            int bs = AudioRecord.GetMinBufferSize(sr, cc, af);

                            if (bs > 0)
                            {
                                // check if we can instantiate and have a success
                                AudioRecord recorder = new AudioRecord(AudioSource.Default, sr, cc, af, bs);

                                if (recorder.State == State.Initialized)
                                {
                                    bufferSize = bs;
                                    sampleRate = sr;
                                    audioFormat = af;
                                    channelConfig = cc;

                                    return recorder;
                                }      
                            }
                        }
                        catch (Exception e)
                        {
                            //                            Log.e(C.TAG, rate + "Exception, keep trying.", e);
                        }
                    }
                }
            }
            return null;
        }

        public static AudioTrack FindAudioTrack(ref int sampleRate, ref Android.Media.Encoding audioFormat, ref ChannelOut channelConfig, ref int bufferSize)
        {
            foreach (var sr in _sampleRates)
            {
                foreach (var af in new Android.Media.Encoding[] { Android.Media.Encoding.Pcm16bit, Android.Media.Encoding.Pcm8bit })
                {
                    foreach (var cc in new ChannelOut[] { ChannelOut.Stereo, ChannelOut.Mono })
                    {
                        foreach (var atm in new AudioTrackMode[] { AudioTrackMode.Static, AudioTrackMode.Stream})
                        {
                            int bs = AudioTrack.GetMinBufferSize(sr, cc, af);

                            if (bs > 0)
                            {
                                var audioTrack = new AudioTrack(Stream.Music, sr, cc, af, bs, atm);

                                if (audioTrack.State == AudioTrackState.Initialized)
                                {
                                    sampleRate = sr;
                                    audioFormat = af;
                                    channelConfig = cc;
                                    bufferSize = bs;

                                    return audioTrack;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        public static Supermortal.Common.PCL.Enums.AudioFormat AndroidAudioFormatToAudioFormat(Android.Media.Encoding audioFormat)
        {
            if (audioFormat == Encoding.Pcm16bit)
                return Supermortal.Common.PCL.Enums.AudioFormat.PCM16Bit;
            if (audioFormat == Encoding.Pcm8bit)
                return Supermortal.Common.PCL.Enums.AudioFormat.PCM8Bit;

            return Supermortal.Common.PCL.Enums.AudioFormat.Unknown;
        }

        public static Android.Media.Encoding AudioFormatToAndroidAudioFormat(Supermortal.Common.PCL.Enums.AudioFormat audioFormat)
        {
            if (audioFormat == Supermortal.Common.PCL.Enums.AudioFormat.PCM16Bit)
                return Encoding.Pcm16bit;
            if (audioFormat == Supermortal.Common.PCL.Enums.AudioFormat.PCM8Bit)
                return Encoding.Pcm8bit;

            return Encoding.Default;
        }

        public static Supermortal.Common.PCL.Enums.ChannelConfiguration AndroidChannelConfigurationToChannelConfiguration(ChannelIn channelConfig)
        {
            if (channelConfig == ChannelIn.Mono)
                return Supermortal.Common.PCL.Enums.ChannelConfiguration.Mono;
            if (channelConfig == ChannelIn.Stereo)
                return Supermortal.Common.PCL.Enums.ChannelConfiguration.Stereo;

            return Supermortal.Common.PCL.Enums.ChannelConfiguration.Unknown;
        }

        public static ChannelOut ChannelConfigurationToAndroidChannelConfiguration(Supermortal.Common.PCL.Enums.ChannelConfiguration channelConfig)
        {
            if (channelConfig == Supermortal.Common.PCL.Enums.ChannelConfiguration.Mono)
                return  ChannelOut.Mono;
            if (channelConfig == Supermortal.Common.PCL.Enums.ChannelConfiguration.Stereo)
                return ChannelOut.Stereo;

            return ChannelOut.Default;
        }
    }
}

