// Made by lovely Waveform
using UnityEngine.Rendering.PostProcessing;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Post Processing V2")]
	public class GetChromaticAberration : FsmStateAction
	{
        [ActionSection("Profile")]
        [ObjectType(typeof(PostProcessProfile))]
        public FsmObject Profile;
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject Volume;
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(PostProcessVolume))]
        public FsmObject VolumeProfile;

        //[ActionSection("Enable")]
        //public FsmBool GetEnable;
        [UIHint(UIHint.Variable)]
        public FsmBool EnableValue;

        //[ActionSection("Spectral Lut")]
        //public FsmBool GetSpectralLut;
        [UIHint(UIHint.Variable)]
        public FsmTexture SpectralLutValue;

        //[ActionSection("Intensity")]
        //public FsmBool GetIntensity;
        [UIHint(UIHint.Variable)]
        public FsmFloat IntensityValue;

        //[ActionSection("FastMode")]
        //public FsmBool GetFastMode;
        [UIHint(UIHint.Variable)]
        public FsmBool FastModeValue;


        [ActionSection(" ")]
        public bool everyFrame;


        private PostProcessProfile convert;
        private PostProcessVolume convert2;

        public override void Reset()
        {
            Profile = null;
            convert = null;
            convert2 = null;
            /*
            GetEnable = false;

            GetSpectralLut = false;

            GetIntensity = false;

            GetFastMode = false;
            */
            everyFrame = false;

        }
        public override void OnEnter()
		{

            ggop();

            if(!everyFrame)
            {
                Finish();
            }

        }

        public override void OnUpdate()
        {
            ggop();
        }
        private void ggop()
        {
            if (Profile.Value != null)
            {
                convert = (PostProcessProfile)Profile.Value;
            }
            else if (Volume.Value != null)
            {
                convert2 = (PostProcessVolume)Volume.Value;
                convert = convert2.profile;
                VolumeProfile.Value = convert;
            }
            if (convert == null)
            {
                return;
            }
            else
            {
                convert.TryGetSettings(out ChromaticAberration chromaticAberration);

                if (!EnableValue.IsNone)
                    EnableValue.Value = chromaticAberration.enabled.value;
                if (!SpectralLutValue.IsNone)
                    SpectralLutValue.Value = chromaticAberration.spectralLut.value;
                if (!IntensityValue.IsNone)
                    IntensityValue.Value = chromaticAberration.intensity.value;
                if (!FastModeValue.IsNone)
                    FastModeValue.Value = chromaticAberration.fastMode.value;

            }



        }
	}

}
